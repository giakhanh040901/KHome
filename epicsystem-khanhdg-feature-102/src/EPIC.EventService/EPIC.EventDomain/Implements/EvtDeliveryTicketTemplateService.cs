using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.EventDomain.Interfaces;
using EPIC.EventRepositories;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using EPIC.EventEntites.Dto.EvtDeliveryTicketTemplate;
using EPIC.EventEntites.Entites;
using EPIC.Entities.DataEntities;
using Microsoft.EntityFrameworkCore;
using EPIC.Utils.Linq;
using DocumentFormat.OpenXml.Packaging;
using EPIC.Entities.Dto.ContractData;
using EPIC.OpenXmlLibrary.Dtos;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.DataUtils;
using EPIC.Utils.SharedApiService;
using QRCoder;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EPIC.FileEntities.Settings;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using EPIC.OpenXmlLibrary;

namespace EPIC.EventDomain.Implements
{
    public class EvtDeliveryTicketTemplateService : IEvtDeliveryTicketTemplateService
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EvtDeliveryTicketTemplateService> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly EvtDeliveryTicketTemplateEFRepository _evtDeliveryTicketTemplateEFRepository;
        private readonly SharedMediaApiUtils _sharedMediaApiUtils;
        private readonly IOptions<FileConfig> _fileConfig;

        public EvtDeliveryTicketTemplateService(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<EvtDeliveryTicketTemplateService> logger,
            IHttpContextAccessor httpContextAccessor,
            SharedMediaApiUtils sharedMediaApiUtils,
            IOptions<FileConfig> fileConfig
           )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _evtDeliveryTicketTemplateEFRepository = new EvtDeliveryTicketTemplateEFRepository(_dbContext, _logger);
            _sharedMediaApiUtils = sharedMediaApiUtils;
            _fileConfig = fileConfig;
        }

        public EvtDeliveryTicketTemplateDto Add(CreateDeliveryTicketTemplateDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Add)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == input.EventId && e.Deleted == YesNo.NO && e.TradingProviderId == tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);
            var insert = new EvtDeliveryTicketTemplate
            {
                Id = (int)_evtDeliveryTicketTemplateEFRepository.NextKey(),
                Name = input.Name,
                EventId = input.EventId,
                FileUrl = input.FileUrl,
            };
            if (_dbContext.EvtDeliveryTicketTemplates.Any(e => e.Deleted == YesNo.NO && e.EventId == input.EventId && e.Status == StatusCommon.ACTIVE))
            {
                insert.Status = StatusCommon.DEACTIVE;
            };
            _dbContext.EvtDeliveryTicketTemplates.Add(insert);
            _dbContext.SaveChanges();
            return _mapper.Map<EvtDeliveryTicketTemplateDto>(insert);
        }

        public void ChangeStatus(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeStatus)} : id = {id}, tradingProviderId = {tradingProviderId}");
            var deliveryTicketTemplate = _dbContext.EvtDeliveryTicketTemplates.FirstOrDefault(e => e.Id == id && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtDeliveryTicketTemplateActiveNotFound);
            if (deliveryTicketTemplate.Status == StatusCommon.ACTIVE)
            {
                deliveryTicketTemplate.Status = StatusCommon.DEACTIVE;
            }
            else
            {
                deliveryTicketTemplate.Status = StatusCommon.ACTIVE;
                var deliveryTicketTemplates = _dbContext.EvtDeliveryTicketTemplates.Where(e => e.EventId == deliveryTicketTemplate.EventId && e.Status == StatusCommon.ACTIVE && e.Deleted == YesNo.NO && e.Id != deliveryTicketTemplate.Id);
                foreach (var item in deliveryTicketTemplates)
                {
                    item.Status = StatusCommon.DEACTIVE;
                }
            }
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Delete)} : id = {id}, tradingProviderId = {tradingProviderId}");
            var deliveryTicketTemplate = _dbContext.EvtDeliveryTicketTemplates.FirstOrDefault(e => e.Id == id && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtDeliveryTicketTemplateActiveNotFound);
            deliveryTicketTemplate.Deleted = YesNo.NO;
            _dbContext.SaveChanges();
        }

        public PagingResult<EvtDeliveryTicketTemplateDto> FindAll(FilterDeliveryTicketTemplateDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");
            var result = new PagingResult<EvtDeliveryTicketTemplateDto>();
            var deliveryTicketTemplates = _dbContext.EvtDeliveryTicketTemplates.Include(e => e.Event)
                                         .Where(e => e.EventId == input.EventId && e.Deleted == YesNo.NO
                                         && e.Event.TradingProviderId == tradingProviderId)
                                         .Select(e => new EvtDeliveryTicketTemplateDto
                                         {
                                             Id = e.Id,
                                             EventId = e.EventId,
                                             Name = e.Name,
                                             FileUrl = e.FileUrl,
                                             CreatedBy = e.CreatedBy,
                                             CreatedDate = e.CreatedDate,
                                             Status = e.Status
                                         });
            result.TotalItems = deliveryTicketTemplates.Count();
            deliveryTicketTemplates = deliveryTicketTemplates.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                deliveryTicketTemplates = deliveryTicketTemplates.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = deliveryTicketTemplates;
            return result;
        }

        public EvtDeliveryTicketTemplateDto FindById(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindById)} : id = {id}, tradingProviderId = {tradingProviderId}");
            var deliveryTicketTemplate = _dbContext.EvtDeliveryTicketTemplates.FirstOrDefault(e => e.Id == id && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtDeliveryTicketTemplateActiveNotFound);
            return _mapper.Map<EvtDeliveryTicketTemplateDto>(deliveryTicketTemplate);
        }

        public EvtDeliveryTicketTemplateDto Update(UpdateDeliveryTicketTemplateDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Update)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == input.EventId && e.Deleted == YesNo.NO && e.TradingProviderId == tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);
            var deliveryTicketTemplate = _dbContext.EvtDeliveryTicketTemplates.FirstOrDefault(e => e.Id == input.Id && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtDeliveryTicketTemplateActiveNotFound);
            deliveryTicketTemplate.Name = input.Name;
            deliveryTicketTemplate.FileUrl = input.FileUrl;
            _dbContext.SaveChanges();
            return _mapper.Map<EvtDeliveryTicketTemplateDto>(deliveryTicketTemplate);
        }

        /// <summary>
        /// Fill mẫu giao nhận vé
        /// </summary>
        /// <param name="deliveryTicketTemplateId"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> FillDeliveryTemplateTicketPdf(int deliveryTicketTemplateId)
        {
            _logger.LogInformation($"{nameof(FillDeliveryTemplateTicketPdf)} : deliveryTicketTemplateId = {deliveryTicketTemplateId}");
            string filePathInit = _fileConfig.Value.Path;

            //Data mẫu
            string customerName = "Nguyễn Văn A";
            string contractCode = "EV000000000";
            string deliveryCode = "EV-00000000000";
            string customerResidentAddress = "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội";

            //lấy mẫu file
            var deliveryTicketTemplate = _dbContext.EvtDeliveryTicketTemplates
                .Select(t => new
                {
                    t.Id,
                    t.FileUrl,
                    t.EventId,
                    t.Name,
                    t.Deleted,
                })
                .FirstOrDefault(t => t.Id == deliveryTicketTemplateId && t.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtDeliveryTicketTemplateActiveNotFound);

            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == deliveryTicketTemplate.EventId && e.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);

            var templatePhysical = FileUtils.GetPhysicalPath(deliveryTicketTemplate.FileUrl, filePathInit);
            string resultFolderPath = templatePhysical.Folder;

            QRCodeGenerator qrGenerator = new();
            //fill cho từng vé
            using FileStream fileTemplate = new(templatePhysical.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using MemoryStream ms = new();
            fileTemplate.CopyTo(ms);

            QRCode qrCode = new QRCode(qrGenerator.CreateQrCode(deliveryCode, QRCodeGenerator.ECCLevel.Q));
            var image = qrCode.GetGraphic(20);
            using MemoryStream msQrCode = new();
            image.SaveAsPng(msQrCode);

            var listReplace = new List<ReplaceTextDto>()
            {
                new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_NAME, eventFind.Name),
                new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, contractCode),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NAME, customerName),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_RESIDENT_ADDRESS, customerResidentAddress),
                new ReplaceTextDto(EvtPropertiesTicketFill.DELIVERY_QR_CODE, msQrCode, "png", 2.3, 2.3),
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

            var fileNamePdf = ContractDataUtils.GenerateNewFileName(deliveryTicketTemplate.Name) + ContractFileExtensions.PDF;

            var pdfBytes = await _sharedMediaApiUtils.ConvertWordToPdfAsync(ms.ToArray());
            var result = new ExportResultDto
            {
                fileData = pdfBytes,
                fileDownloadName = fileNamePdf,
            };
            return result;
        }

        /// <summary>
        /// Fill mẫu giao nhận vé
        /// </summary>
        /// <param name="deliveryTicketTemplateId"></param>
        /// <returns></returns>
        public ExportResultDto FillDeliveryTemplateTicketWord(int deliveryTicketTemplateId)
        {
            _logger.LogInformation($"{nameof(FillDeliveryTemplateTicketWord)} : deliveryTicketTemplateId = {deliveryTicketTemplateId}");
            string filePathInit = _fileConfig.Value.Path;

            //Data mẫu
            string customerName = "Nguyễn Văn A";
            string contractCode = "EV000000000";
            string deliveryCode = "EV-00000000000";
            string customerResidentAddress = "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội";

            //lấy mẫu file
            var deliveryTicketTemplate = _dbContext.EvtDeliveryTicketTemplates
                .Select(t => new
                {
                    t.Id,
                    t.FileUrl,
                    t.EventId,
                    t.Name,
                    t.Deleted,
                })
                .FirstOrDefault(t => t.Id == deliveryTicketTemplateId && t.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtDeliveryTicketTemplateActiveNotFound);

            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == deliveryTicketTemplate.EventId && e.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);

            var templatePhysical = FileUtils.GetPhysicalPath(deliveryTicketTemplate.FileUrl, filePathInit);
            string resultFolderPath = templatePhysical.Folder;

            QRCodeGenerator qrGenerator = new();
            //fill cho từng vé
            using FileStream fileTemplate = new(templatePhysical.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using MemoryStream ms = new();
            fileTemplate.CopyTo(ms);

            QRCode qrCode = new QRCode(qrGenerator.CreateQrCode(deliveryCode, QRCodeGenerator.ECCLevel.Q));
            var image = qrCode.GetGraphic(20);
            using MemoryStream msQrCode = new();
            image.SaveAsPng(msQrCode);

            var listReplace = new List<ReplaceTextDto>()
            {
                new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_NAME, eventFind.Name),
                new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, contractCode),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NAME, customerName),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_RESIDENT_ADDRESS, customerResidentAddress),
                new ReplaceTextDto(EvtPropertiesTicketFill.DELIVERY_QR_CODE, msQrCode, "png", 2.3, 2.3),
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

            var fileNamePdf = ContractDataUtils.GenerateNewFileName(deliveryTicketTemplate.Name) + ContractFileExtensions.DOCX;
            var result = new ExportResultDto
            {
                fileData = ms.ToArray(),
                fileDownloadName = fileNamePdf,
            };
            return result;
        }

        /// <summary>
        /// Fill mẫu giao nhận vé
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> FillDeliveryTicket(int orderId)
        {
            _logger.LogInformation($"{nameof(FillDeliveryTicket)} : orderId = {orderId}");
            string filePathInit = _fileConfig.Value.Path;

            //Order
            var order = _dbContext.EvtOrders.Include(e => e.EventDetail)
                                            .ThenInclude(e => e.Event)
                                            .ThenInclude(e => e.DeliveryTicketTemplates)
                                            .Include(o => o.Investor)
                                            .Include(o => o.InvestorIdentification)
                                            .Include(o => o.ContractAddress)
                                            .FirstOrDefault(e => e.Id == orderId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);

            //lấy mẫu file
            var deliveryTicketTemplate = _dbContext.EvtDeliveryTicketTemplates
                .Where(t => t.Status == StatusCommon.ACTIVE)
                .Select(t => new
                {
                    t.Id,
                    t.FileUrl,
                    t.EventId,
                    t.Name,
                    t.Deleted,
                })
                .FirstOrDefault(t => t.EventId == order.EventDetail.EventId && t.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtDeliveryTicketTemplateActiveNotFound);

            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == deliveryTicketTemplate.EventId && e.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);

            var templatePhysical = FileUtils.GetPhysicalPath(deliveryTicketTemplate.FileUrl, filePathInit);
            string resultFolderPath = templatePhysical.Folder;

            QRCodeGenerator qrGenerator = new();
            //fill cho từng vé
            using FileStream fileTemplate = new(templatePhysical.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using MemoryStream ms = new();
            fileTemplate.CopyTo(ms);

            QRCode qrCode = new QRCode(qrGenerator.CreateQrCode(order.DeliveryCode, QRCodeGenerator.ECCLevel.Q));
            var image = qrCode.GetGraphic(20);
            using MemoryStream msQrCode = new();
            image.SaveAsPng(msQrCode);

            var listReplace = new List<ReplaceTextDto>()
            {
                new ReplaceTextDto(EvtPropertiesTicketFill.EVENT_NAME, eventFind.Name),
                new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, order.ContractCode),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NAME, order.InvestorIdentification.Fullname),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_RESIDENT_ADDRESS, order.ContractAddress.ContactAddress),
                new ReplaceTextDto(EvtPropertiesTicketFill.DELIVERY_QR_CODE, msQrCode, "png", 2.3, 2.3),
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

            var fileNamePdf = ContractDataUtils.GenerateNewFileName(deliveryTicketTemplate.Name) + ContractFileExtensions.PDF;
            string filePathPdf = Path.Combine(filePathInit, resultFolderPath, fileNamePdf);

            var pdfBytes = await _sharedMediaApiUtils.ConvertWordToPdfAsync(ms.ToArray());
            var result = new ExportResultDto
            {
                fileData = pdfBytes,
                fileDownloadName = fileNamePdf,
            };
            return result;
        }
    }
}
