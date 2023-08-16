using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using EPIC.DataAccess.Base;
using EPIC.Entities.Dto.ContractData;
using EPIC.EventDomain.Interfaces;
using EPIC.EventRepositories;
using EPIC.FileEntities.Settings;
using EPIC.OpenXmlLibrary;
using EPIC.OpenXmlLibrary.Dtos;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.ConstantVariables.Hangfire;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.EnumType;
using EPIC.Utils.Filter;
using EPIC.Utils.SharedApiService;
using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QRCoder;
using SixLabors.ImageSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Implements
{
    public class EvtOrderTicketFillService : IEvtOrderTicketFillService
    {
        private readonly ILogger _logger;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContext;
        private readonly SharedMediaApiUtils _sharedMediaApiUtils;
        private readonly IOptions<FileConfig> _fileConfig;

        public EvtOrderTicketFillService(
            EpicSchemaDbContext dbContext,
            ILogger<EvtOrderTicketFillService> logger,
            IHttpContextAccessor httpContext,
            SharedMediaApiUtils sharedMediaApiUtils,
            IOptions<FileConfig> fileConfig)
        {
            _dbContext = dbContext;
            _logger = logger;
            _httpContext = httpContext;
            _sharedMediaApiUtils = sharedMediaApiUtils;
            _fileConfig = fileConfig;
        }

        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [Queue(HangfireQueues.Event)]
        [HangfireLogEverything]
        public async Task FillOrderTicket(int orderId)
        {
            _logger.LogInformation($"{nameof(FillOrderTicket)} : orderId = {orderId}");

            string filePathInit = _fileConfig.Value.Path;
            var orderFind = _dbContext.EvtOrders
                .Include(o => o.EventDetail)
                .Select(o => new
                {
                    o.Id,
                    o.Status,
                    o.EventDetail.EventId,
                    o.TradingProviderId,
                    o.Deleted,
                    o.ContractCode,
                    o.ContractCodeGen,
                    o.CreatedDate,
                    o.TicketJobId
                })
                .FirstOrDefault(o => o.Id == orderId && o.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);

            if (orderFind.TicketJobId != null)
            {
                IStorageConnection connection = JobStorage.Current.GetConnection();
                JobData jobData = connection.GetJobData(orderFind.TicketJobId);
                if (jobData != null && jobData.State == EnumBackgroundJobState.Processing)
                {
                    throw new FaultException(new FaultReason($"Đang tạo vé, vui lòng chờ trong giây lát"), new FaultCode(((int)ErrorCode.EvtOrderTicketUrlIsCreating).ToString()), "");
                }
            }

            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == orderFind.EventId && e.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);

            var eventAvatar = _dbContext.EvtEventMedias.FirstOrDefault(e => e.EventId == eventFind.Id && e.Location == EvtMediaLocation.AVATAR_EVENT && e.Deleted == YesNo.NO);
            var avatarEventPhysicalPath = FileUtils.GetPhysicalPath(eventAvatar.UrlImage, filePathInit);
            FileStream avatarEventPhysicalStream = new(avatarEventPhysicalPath.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            var tradingProvider = _dbContext.TradingProviders
                .Include(t => t.BusinessCustomer)
                .Where(t => t.TradingProviderId == orderFind.TradingProviderId)
                .Select(t => new
                {
                    t.BusinessCustomer.Name,
                    t.BusinessCustomer.AvatarImageUrl,
                })
                .FirstOrDefault();

            var avatarTradingPhysicalPath = FileUtils.GetPhysicalPath(tradingProvider.AvatarImageUrl, filePathInit);
            FileStream avatarTradingPhysicalStream = new(avatarTradingPhysicalPath.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            var orderTicketDetail = _dbContext.EvtOrderTicketDetails
                .Include(o => o.OrderDetail)
                    .ThenInclude(o => o.Ticket)
                    .ThenInclude(o => o.EventDetail)
                .Include(o => o.Ticket)
                .Where(o => o.OrderDetail.OrderId == orderId)
                .Select(o => new
                {
                    OrderTicketDetail = o,
                    o.OrderDetail.Ticket.EventDetail.StartDate,
                    o.Ticket.Name,
                })
                .ToList();

            //lấy mẫu file
            var ticketTemplate = _dbContext.EvtTicketTemplates
                .Select(t => new
                {
                    t.FileUrl,
                    t.EventId,
                    t.Name,
                    t.Deleted
                })
                .FirstOrDefault(t => t.EventId == eventFind.Id && t.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtTicketTemplateActiveNotFound);

            var templatePhysical = FileUtils.GetPhysicalPath(ticketTemplate.FileUrl, filePathInit);
            string resultFolderPath = templatePhysical.Folder;

            QRCodeGenerator qrGenerator = new();
            //fill cho từng vé
            foreach (var ticket in orderTicketDetail)
            {
                using FileStream fileTemplate = new(templatePhysical.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using MemoryStream ms = new();
                fileTemplate.CopyTo(ms);

                QRCode qrCode = new QRCode(qrGenerator.CreateQrCode(ticket.OrderTicketDetail.TicketCode, QRCodeGenerator.ECCLevel.Q));
                var image = qrCode.GetGraphic(20);
                using MemoryStream msQrCode = new();
                image.SaveAsPng(msQrCode);

                var listReplace = new List<ReplaceTextDto>()
                {
                    new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_NAME, eventFind.Name),
                    new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_AVATAR, avatarEventPhysicalStream, Path.GetExtension(avatarEventPhysicalPath.FullPath), 2, 3),
                    new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_START_DATE, ticket.StartDate, "dd/MM/yyyy HH:mm"),
                    new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_LOCATION, eventFind.Location),
                    new ReplaceTextDto(EvtPropertiesTicketFill.TICKET_QR_CODE, msQrCode, "png", 1, 1),
                    new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_AVATAR, avatarTradingPhysicalStream, Path.GetExtension(avatarTradingPhysicalPath.FullPath), 0.75, 0.75),
                    new ReplaceTextDto(EvtPropertiesTicketFill.TICKET_NAME, ticket.Name),
                    new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, orderFind.ContractCodeGen ?? orderFind.ContractCode),
                    new ReplaceTextDto(EvtPropertiesTicketFill.ORDER_CREATED_DATE, orderFind.CreatedDate, "dd/MM/yyyy HH:mm"),
                    new ReplaceTextDto(EvtPropertiesTicketFill.TICKET_CHECK_IN, ticket.StartDate, "dd/MM/yyyy HH:mm"),

                };
                //fill vào file
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, true))
                {
                    wordDoc.ReplaceTextPlaceHolder(listReplace.Select(o => new InputReplaceDto
                    {
                        FindText = o.FindText,
                        ReplaceText = o.ReplaceText,
                        ReplaceImage = o.ReplaceImage,
                        ReplaceImageExtension = o.ReplaceImageExtension,
                        ReplaceImageWidth = o.ReplaceImageWidth,
                        ReplaceImageHeight = o.ReplaceImageHeight,
                    }));
                }

                var fileNamePdf = ContractDataUtils.GenerateNewFileName(ticketTemplate.Name) + ContractFileExtensions.PDF;
                string filePathPdf = Path.Combine(filePathInit, resultFolderPath, fileNamePdf);

                ticket.OrderTicketDetail.TicketFilledUrl = ContractDataUtils.GetEndPoint(resultFolderPath, fileNamePdf);

                var pdfBytes = await _sharedMediaApiUtils.ConvertWordToPdfAsync(ms.ToArray());
                using var filePdf = new FileStream(filePathPdf, FileMode.CreateNew);
                filePdf.Write(pdfBytes, 0, pdfBytes.Length);
            }
            _dbContext.SaveChanges();
        }

        //Fill data vào vé và lưu đường dẫn
        public async Task FillTicket(int orderTicketDetailId)
        {
            _logger.LogInformation($"{nameof(FillOrderTicket)} : orderTicketDetailId = {orderTicketDetailId}");
            string filePathInit = _fileConfig.Value.Path;

            var orderTicketDetail = _dbContext.EvtOrderTicketDetails
                .Include(o => o.OrderDetail)
                    .ThenInclude(o => o.Ticket)
                    .ThenInclude(o => o.EventDetail)
                .Include(o => o.Ticket)
                .Select(o => new
                {
                    OrderTicketDetail = o,
                    o.Id,
                    o.OrderDetail.Ticket.EventDetail.StartDate,
                    o.Ticket.Name,
                    o.OrderDetail.Ticket.EventDetail.EventId,
                    o.OrderDetail.OrderId
                })
                .FirstOrDefault(e => e.Id == orderTicketDetailId);

            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == orderTicketDetail.EventId && e.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);

            var eventAvatar = _dbContext.EvtEventMedias.FirstOrDefault(e => e.EventId == eventFind.Id && e.Location == EvtMediaLocation.AVATAR_EVENT && e.Deleted == YesNo.NO);
            var avatarEventPhysicalPath = FileUtils.GetPhysicalPath(eventAvatar.UrlImage, filePathInit);
            FileStream avatarEventPhysicalStream = new(avatarEventPhysicalPath.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            var orderFind = _dbContext.EvtOrders
                .Include(o => o.EventDetail)
                .Select(o => new
                {
                    o.Id,
                    o.Status,
                    o.EventDetail.EventId,
                    o.TradingProviderId,
                    o.Deleted,
                    o.ContractCode,
                    o.ContractCodeGen,
                    o.CreatedDate
                })
                .FirstOrDefault(o => o.Id == orderTicketDetail.OrderId && o.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);
            var tradingProvider = _dbContext.TradingProviders
                .Include(t => t.BusinessCustomer)
                .Where(t => t.TradingProviderId == orderFind.TradingProviderId)
                .Select(t => new
                {
                    t.BusinessCustomer.Name,
                    t.BusinessCustomer.AvatarImageUrl,
                })
                .FirstOrDefault();

            var avatarTradingPhysicalPath = FileUtils.GetPhysicalPath(tradingProvider.AvatarImageUrl, filePathInit);
            FileStream avatarTradingPhysicalStream = new(avatarTradingPhysicalPath.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            
            //lấy mẫu file
            var ticketTemplate = _dbContext.EvtTicketTemplates
                .Select(t => new
                {
                    t.FileUrl,
                    t.EventId,
                    t.Name,
                    t.Deleted
                })
                .FirstOrDefault(t => t.EventId == eventFind.Id && t.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtTicketTemplateActiveNotFound);

            var templatePhysical = FileUtils.GetPhysicalPath(ticketTemplate.FileUrl, filePathInit);
            string resultFolderPath = templatePhysical.Folder;

            QRCodeGenerator qrGenerator = new();
            //fill cho từng vé
            using FileStream fileTemplate = new(templatePhysical.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using MemoryStream ms = new();
            fileTemplate.CopyTo(ms);

            QRCode qrCode = new QRCode(qrGenerator.CreateQrCode(orderTicketDetail.OrderTicketDetail.TicketCode, QRCodeGenerator.ECCLevel.Q));
            var image = qrCode.GetGraphic(20);
            using MemoryStream msQrCode = new();
            image.SaveAsPng(msQrCode);

            var listReplace = new List<ReplaceTextDto>()
            {
                new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_NAME, eventFind.Name),
                new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_AVATAR, avatarEventPhysicalStream, Path.GetExtension(avatarEventPhysicalPath.FullPath), 2, 3),
                new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_START_DATE, orderTicketDetail.StartDate, "dd/MM/yyyy HH:mm"),
                new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_LOCATION, eventFind.Location),
                new ReplaceTextDto(EvtPropertiesTicketFill.TICKET_QR_CODE, msQrCode, "png", 1, 1),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_AVATAR, avatarTradingPhysicalStream, Path.GetExtension(avatarTradingPhysicalPath.FullPath), 0.75, 0.75),
                new ReplaceTextDto(EvtPropertiesTicketFill.TICKET_NAME, orderTicketDetail.Name),
                new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, orderFind.ContractCodeGen ?? orderFind.ContractCode),
                new ReplaceTextDto(EvtPropertiesTicketFill.ORDER_CREATED_DATE, orderFind.CreatedDate, "dd/MM/yyyy HH:mm"),
                new ReplaceTextDto(EvtPropertiesTicketFill.TICKET_CHECK_IN, orderTicketDetail.StartDate, "dd/MM/yyyy HH:mm"),

            };
            //fill vào file
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, true))
            {
                wordDoc.ReplaceTextPlaceHolder(listReplace.Select(o => new InputReplaceDto
                {
                    FindText = o.FindText,
                    ReplaceText = o.ReplaceText,
                    ReplaceImage = o.ReplaceImage,
                    ReplaceImageExtension = o.ReplaceImageExtension,
                    ReplaceImageWidth = o.ReplaceImageWidth,
                    ReplaceImageHeight = o.ReplaceImageHeight,
                }));
            }

            var fileNamePdf = ContractDataUtils.GenerateNewFileName(ticketTemplate.Name) + ContractFileExtensions.PDF;
            string filePathPdf = Path.Combine(filePathInit, resultFolderPath, fileNamePdf);

            orderTicketDetail.OrderTicketDetail.TicketFilledUrl = ContractDataUtils.GetEndPoint(resultFolderPath, fileNamePdf);

            var pdfBytes = await _sharedMediaApiUtils.ConvertWordToPdfAsync(ms.ToArray());
            using var filePdf = new FileStream(filePathPdf, FileMode.CreateNew);
            filePdf.Write(pdfBytes, 0, pdfBytes.Length);
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// Fill mẫu vé sự kiện pdf
        /// </summary>
        /// <param name="ticketTemplateId"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> FillTemplateTicketPdf(int ticketTemplateId)
        {
            _logger.LogInformation($"{nameof(FillTemplateTicketPdf)} : ticketTemplateId = {ticketTemplateId}");
            string filePathInit = _fileConfig.Value.Path;

            //Data mẫu
            string ticketCode = "EV-ABCD1234EFGH";
            string ticketName = "Vé VIP1";
            string ContractCode = "EV000000000";
            var orderCreatedDate = DateTime.Now;
            var startDate = DateTime.Now;

            //lấy mẫu file
            var ticketTemplate = _dbContext.EvtTicketTemplates
                .Select(t => new
                {
                    t.Id,
                    t.FileUrl,
                    t.EventId,
                    t.Name,
                    t.Deleted,
                })
                .FirstOrDefault(t => t.Id == ticketTemplateId && t.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtTicketTemplateActiveNotFound);

            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == ticketTemplate.EventId && e.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);

            var eventStartDate = _dbContext.EvtEventDetails.Where(e => e.EventId == eventFind.Id && e.Deleted == YesNo.NO);
            if (eventStartDate.Any())
            {
                startDate = eventStartDate.Min(e => e.StartDate).Value;
            }

            var eventAvatar = _dbContext.EvtEventMedias.FirstOrDefault(e => e.EventId == eventFind.Id && e.Location == EvtMediaLocation.AVATAR_EVENT && e.Deleted == YesNo.NO);
            var listReplace = new List<ReplaceTextDto>()
            {
                new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_NAME, eventFind.Name),
                new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_START_DATE, startDate, "dd/MM/yyyy HH:mm"),
                new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_LOCATION, eventFind.Location),
                new ReplaceTextDto(EvtPropertiesTicketFill.TICKET_NAME, ticketName),
                new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, ContractCode),
                new ReplaceTextDto(EvtPropertiesTicketFill.ORDER_CREATED_DATE, orderCreatedDate, "dd/MM/yyyy HH:mm"),
                new ReplaceTextDto(EvtPropertiesTicketFill.TICKET_CHECK_IN, startDate, "dd/MM/yyyy HH:mm"),

            };
            if (eventAvatar != null && eventAvatar.UrlImage != null)
            {
                var avatarEventPhysicalPath = FileUtils.GetPhysicalPath(eventAvatar.UrlImage, filePathInit);
                FileStream avatarEventPhysicalStream = new(avatarEventPhysicalPath.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                listReplace.Add(new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_AVATAR, avatarEventPhysicalStream, Path.GetExtension(avatarEventPhysicalPath.FullPath), 2, 3));
            }
        
            var templatePhysical = FileUtils.GetPhysicalPath(ticketTemplate.FileUrl, filePathInit);
            string resultFolderPath = templatePhysical.Folder;

            QRCodeGenerator qrGenerator = new();
            //fill cho từng vé
            using FileStream fileTemplate = new(templatePhysical.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using MemoryStream ms = new();
            fileTemplate.CopyTo(ms);

            QRCode qrCode = new QRCode(qrGenerator.CreateQrCode(ticketCode, QRCodeGenerator.ECCLevel.Q));
            var image = qrCode.GetGraphic(20);
            using MemoryStream msQrCode = new();
            listReplace.Add(new ReplaceTextDto(EvtPropertiesTicketFill.TICKET_QR_CODE, msQrCode, "png", 1, 1));
            image.SaveAsPng(msQrCode);

            //fill vào file
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, true))
            {
                wordDoc.ReplaceTextPlaceHolder(listReplace.Select(o => new InputReplaceDto
                {
                    FindText = o.FindText,
                    ReplaceText = o.ReplaceText,
                    ReplaceImage = o.ReplaceImage,
                    ReplaceImageExtension = o.ReplaceImageExtension,
                    ReplaceImageWidth = o.ReplaceImageWidth,
                    ReplaceImageHeight = o.ReplaceImageHeight,
                }));
            }

            var fileNamePdf = ContractDataUtils.GenerateNewFileName(ticketTemplate.Name) + ContractFileExtensions.PDF;
            string filePathPdf = Path.Combine(filePathInit, resultFolderPath, fileNamePdf);

            var pdfBytes = await _sharedMediaApiUtils.ConvertWordToPdfAsync(ms.ToArray());
            var result = new ExportResultDto
            {
                fileData = pdfBytes,
                fileDownloadName = fileNamePdf,
            };
            return result;
        }

        /// <summary>
        /// Fill mẫu vé sự kiện word
        /// </summary>
        /// <param name="ticketTemplateId"></param>
        /// <returns></returns>
        public ExportResultDto FillTemplateTicketWord(int ticketTemplateId)
        {
            _logger.LogInformation($"{nameof(FillTemplateTicketWord)} : ticketTemplateId = {ticketTemplateId}");
            string filePathInit = _fileConfig.Value.Path;

            //Data mẫu
            string ticketCode = "EV-ABCD1234EFGH";
            string ticketName = "Vé VIP1";
            string ContractCode = "EV000000000";
            var orderCreatedDate = DateTime.Now;
            var startDate = DateTime.Now;

            //lấy mẫu file
            var ticketTemplate = _dbContext.EvtTicketTemplates
                .Select(t => new
                {
                    t.Id,
                    t.FileUrl,
                    t.EventId,
                    t.Name,
                    t.Deleted,
                })
                .FirstOrDefault(t => t.Id == ticketTemplateId && t.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtTicketTemplateActiveNotFound);

            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == ticketTemplate.EventId && e.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);

            var eventStartDate = _dbContext.EvtEventDetails.Where(e => e.EventId == eventFind.Id && e.Deleted == YesNo.NO);
            if (eventStartDate.Any())
            {
                startDate = eventStartDate.Min(e => e.StartDate).Value;
            }

            var eventAvatar = _dbContext.EvtEventMedias.FirstOrDefault(e => e.EventId == eventFind.Id && e.Location == EvtMediaLocation.AVATAR_EVENT && e.Deleted == YesNo.NO);
            var listReplace = new List<ReplaceTextDto>()
            {
                new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_NAME, eventFind.Name),
                new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_START_DATE, startDate, "dd/MM/yyyy HH:mm"),
                new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_LOCATION, eventFind.Location),
                new ReplaceTextDto(EvtPropertiesTicketFill.TICKET_NAME, ticketName),
                new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, ContractCode),
                new ReplaceTextDto(EvtPropertiesTicketFill.ORDER_CREATED_DATE, orderCreatedDate, "dd/MM/yyyy HH:mm"),
                new ReplaceTextDto(EvtPropertiesTicketFill.TICKET_CHECK_IN, startDate, "dd/MM/yyyy HH:mm"),

            };

            if (eventAvatar != null && eventAvatar.UrlImage != null)
            {
                var avatarEventPhysicalPath = FileUtils.GetPhysicalPath(eventAvatar.UrlImage, filePathInit);
                FileStream avatarEventPhysicalStream = new(avatarEventPhysicalPath.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                listReplace.Add(new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_AVATAR, avatarEventPhysicalStream, Path.GetExtension(avatarEventPhysicalPath.FullPath), 2, 3));
            }

            var templatePhysical = FileUtils.GetPhysicalPath(ticketTemplate.FileUrl, filePathInit);
            string resultFolderPath = templatePhysical.Folder;

            QRCodeGenerator qrGenerator = new();
            //fill cho từng vé
            using FileStream fileTemplate = new(templatePhysical.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using MemoryStream ms = new();
            fileTemplate.CopyTo(ms);

            QRCode qrCode = new QRCode(qrGenerator.CreateQrCode(ticketCode, QRCodeGenerator.ECCLevel.Q));
            var image = qrCode.GetGraphic(20);
            using MemoryStream msQrCode = new();
            listReplace.Add(new ReplaceTextDto(EvtPropertiesTicketFill.TICKET_QR_CODE, msQrCode, "png", 1, 1));
            image.SaveAsPng(msQrCode);

            //fill vào file
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, true))
            {
                wordDoc.ReplaceTextPlaceHolder(listReplace.Select(o => new InputReplaceDto
                {
                    FindText = o.FindText,
                    ReplaceText = o.ReplaceText,
                    ReplaceImage = o.ReplaceImage,
                    ReplaceImageExtension = o.ReplaceImageExtension,
                    ReplaceImageWidth = o.ReplaceImageWidth,
                    ReplaceImageHeight = o.ReplaceImageHeight,
                }));
            }

            var fileNamePdf = ContractDataUtils.GenerateNewFileName(ticketTemplate.Name) + ContractFileExtensions.DOCX;

            var result = new ExportResultDto
            {
                fileData = ms.ToArray(),
                fileDownloadName = fileNamePdf,
            };
            return result;
        }
    }
}
