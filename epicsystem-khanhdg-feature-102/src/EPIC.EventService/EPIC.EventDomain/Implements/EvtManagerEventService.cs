using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtAdminEvent;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtHistoryUpdate;
using EPIC.EventEntites.Entites;
using EPIC.EventRepositories;
using EPIC.FileDomain.Services;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Implements
{
    public class EvtManagerEventService : IEvtManagerEventService
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EvtManagerEventService> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly EvtEventEFRepository _evtEventEFRepository;
        private readonly EvtEventDetailEFRepository _evtEventDetailEFRepository;
        private readonly EvtEventTypeEFRepository _evtEventTypeEFRepository;
        private readonly EvtHistoryUpdateEFRepository _evtHistoryUpdateEFRepository;
        private readonly EvtEventBankAccountEFRepository _evtEventBankAccountEFRepository;
        private readonly EvtTicketTemplateEFRepository _evtTicketTemplateEFRepository;
        private readonly EventNotificationServices _eventNotificationServices;
        private readonly SaleEFRepository _saleEFRepository;
        private readonly IFileServices _fileService;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly EvtBackgroundJobServices _evtBackgroundJobServices;
        private readonly EvtAdminEventEFRepository _evtAdminEventEFRepository;

        public EvtManagerEventService(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<EvtManagerEventService> logger,
            IHttpContextAccessor httpContextAccessor,
            EventNotificationServices eventNotificationServices,
            IFileServices fileService,
            IBackgroundJobClient backgroundJobs,
            EvtBackgroundJobServices evtBackgroundJobServices
           )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _eventNotificationServices = eventNotificationServices;
            _fileService = fileService;
            _evtEventEFRepository = new EvtEventEFRepository(_dbContext, _logger);
            _evtEventDetailEFRepository = new EvtEventDetailEFRepository(_dbContext, _logger);
            _evtEventTypeEFRepository = new EvtEventTypeEFRepository(_dbContext, _logger);
            _evtHistoryUpdateEFRepository = new EvtHistoryUpdateEFRepository(_dbContext, _logger);
            _evtEventBankAccountEFRepository = new EvtEventBankAccountEFRepository(_dbContext, _logger);
            _saleEFRepository = new SaleEFRepository(_dbContext, _logger);
            _evtTicketTemplateEFRepository = new EvtTicketTemplateEFRepository(_dbContext, _logger);
            _backgroundJobs = backgroundJobs; 
            _evtBackgroundJobServices = evtBackgroundJobServices;
            _evtAdminEventEFRepository = new EvtAdminEventEFRepository(_dbContext, _logger);
        }

        #region Sự kiện

        /// <summary>
        /// lay duong dan file ve mau mac dinh
        /// </summary>
        /// <returns></returns>
        public string UploadFileTicketTemplate()
        {
            string fileName = "Ticket_Template.docx";
            string filePath = Path.Combine(Environment.CurrentDirectory, "Data", "TicketTemplate", fileName);

            // Đảm bảo tệp tin đã tồn tại
            if (!File.Exists(filePath))
            {
                _evtTicketTemplateEFRepository.ThrowException(ErrorCode.FileNotFound);
            }

            byte[] fileBytes = File.ReadAllBytes(filePath);
            MemoryStream memoryStream = new MemoryStream(fileBytes);
            IFormFile formFile = new FormFile(memoryStream, 0, memoryStream.Length, "Ticket_Template", fileName);

            var uploadedFilePath = _fileService.UploadFile(new ImageAPI.Models.UploadFileModel
            {
                File = formFile,
                Folder = Path.Combine(FileFolder.Events, "ticket"),
            });

            return uploadedFilePath;
        }

        public EvtEventDto Add(CreateEvtEventDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Add)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");
            EvtEvent inputInsert = new EvtEvent
            {
                TradingProviderId = tradingProviderId,
                Name = input.Name,
                Organizator = input.Organizator,
                IsShowApp = YesNo.NO,
                IsCheck = YesNo.YES, //Tạm thời bỏ check đi, vì đã where theo điều kiện check nên set là YES sau cần thì đổi thành NO
            };
            var transaction = _dbContext.Database.BeginTransaction();
            inputInsert.Id = (int)_evtEventEFRepository.NextKey();
            inputInsert.TradingProviderId = tradingProviderId;
            var eventResult = _dbContext.EvtEvents.Add(inputInsert);

            _dbContext.SaveChanges();

            string fileUrl = UploadFileTicketTemplate();

            EvtTicketTemplate insertTicketTemplate = new EvtTicketTemplate
            {
                Id = (int)_evtTicketTemplateEFRepository.NextKey(),
                EventId = eventResult.Entity.Id,
                Name = eventResult.Entity.Name,
                Status = StatusCommon.ACTIVE,
                FileUrl = fileUrl
            };
            _dbContext.EvtTicketTemplates.Add(insertTicketTemplate);
            _dbContext.SaveChanges();

            foreach (var item in input.EventTypes)
            {
                var insertType = new EvtEventType
                {
                    Id = (int)_evtEventTypeEFRepository.NextKey(),
                    EventId = eventResult.Entity.Id,
                    Type = item,
                };
                _dbContext.EvtEventTypes.Add(insertType);
            }
            _evtHistoryUpdateEFRepository.Entity.Add(new EvtHistoryUpdate
            {
                Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                RealTableId = inputInsert.Id,
                OldValue = null,
                NewValue = "Khởi tạo",
                FieldName = "Trạng thái",
                UpdateTable = EvtHistoryUpdateTables.EVT_EVENT,
                Action = ActionTypes.THEM_MOI,
                Summary = "Khởi tạo sự kiện"
            });
            _dbContext.SaveChanges();
            transaction.Commit();
            return new EvtEventDto
            {
                Id = inputInsert.Id,
                Name = inputInsert.Name,
                Organizator = inputInsert.Organizator
            };
        }

        public void Delete(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Delete)} : id = {id}, tradingProviderId = {tradingProviderId}");
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == id && e.TradingProviderId == tradingProviderId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);
            if (eventFind.EventFinishedJobId != null)
            {
                // Xóa job cũ, sinh job mới
                _backgroundJobs.Delete(eventFind.EventFinishedJobId);
            }
            eventFind.Deleted = YesNo.YES;
            _evtHistoryUpdateEFRepository.Entity.Add(new EvtHistoryUpdate
            {
                Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                RealTableId = eventFind.Id,
                UpdateTable = EvtHistoryUpdateTables.EVT_EVENT,
                Action = ActionTypes.XOA,
                Summary = "Xoá sự kiện"
            });
            _dbContext.SaveChanges();
        }

        public PagingResult<EvtEventDto> FindAll(FilterEvtEventDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            var result = new PagingResult<EvtEventDto>();
            var query = _dbContext.EvtEvents.Include(e => e.EventDetails).ThenInclude(e => e.Tickets.Where(e => e.Deleted == YesNo.NO))
                                            .Include(e => e.EventTypes)
                                            .Include(e => e.Province)
                                            .Where(e => e.Deleted == YesNo.NO
                                                && (input.Keyword == null || e.Name.ToLower().Contains(input.Keyword.ToLower())
                                                    || e.Organizator.ToLower().Contains(input.Keyword.ToLower()))
                                                && (input.StartDate == null || e.EventDetails.Any(e => e.StartDate.Value.Date == input.StartDate.Value.Date))
                                                && (input.EventTypes == null || e.EventTypes.Select(e => e.Type).Any(element => input.EventTypes.Contains(element)))
                                                && (input.Status == null || input.Status.Contains(e.Status))
                                                && (e.TradingProviderId == tradingProviderId)
                                                && (input.Organizator == null || e.Organizator.ToLower().Contains(input.Organizator.ToLower())))
                                            .Select(e => new
                                            {
                                                evt = e,
                                                eventDetail = e.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT),
                                            })
                                            .Select(e => new EvtEventDto
                                            {
                                                Id = e.evt.Id,
                                                Address = e.evt.Address,
                                                BackAccountId = e.evt.BackAccountId,
                                                CanExportRequestRecipt = e.evt.CanExportRequestRecipt,
                                                CanExportTicket = e.evt.CanExportTicket,
                                                ProvinceCode = e.evt.ProvinceCode,
                                                ProvinceName = e.evt.Province.Name,
                                                ConfigContractCodeId = e.evt.ConfigContractCodeId,
                                                ContentType = e.evt.ContentType,
                                                Description = e.evt.Description,
                                                Email = e.evt.Email,
                                                Facebook = e.evt.Facebook,
                                                IsCheck = e.evt.IsCheck,
                                                IsHighlight = e.evt.IsHighlight,
                                                IsShowApp = e.evt.IsShowApp,
                                                Latitude = e.evt.Latitude,
                                                Location = e.evt.Location,
                                                Longitude = e.evt.Longitude,
                                                Name = e.evt.Name,
                                                Organizator = e.evt.Organizator,
                                                OverviewContent = e.evt.OverviewContent,
                                                Phone = e.evt.Phone,
                                                Status = e.evt.Status,
                                                Viewing = e.evt.Viewing,
                                                Website = e.evt.Website,
                                                CreatedBy = e.evt.CreatedBy,
                                                CreatedDate = e.evt.CreatedDate,
                                                EventTypes = e.evt.EventTypes.Select(e => e.Type),
                                                TicketQuantity = e.eventDetail.Sum(ed => ed.Tickets.Sum(t => t.Quantity)),
                                                RegisterQuantity = e.eventDetail.Sum(ed => ed.Orders.Sum(o => o.OrderDetails.Sum(od => od.Quantity))),
                                                ValidQuantity = e.eventDetail.Sum(ed => ed.Orders.Where(o => o.Status == EvtOrderStatus.HOP_LE).Sum(o => o.OrderDetails.Sum(od => od.Quantity))),
                                                ParticipateQuantity = e.eventDetail.Sum(ed => ed.Orders.Sum(o => o.OrderDetails.Sum(od => od.OrderTicketDetails.Where(otd => otd.Status == EvtOrderTicketStatus.DA_THAM_GIA).Count()))),
                                                RemainingTickets = e.eventDetail.Sum(ed => ed.Tickets.Sum(t => t.Quantity))
                                                                    - (e.eventDetail.Sum(e => e.Orders.Where(o => (o.Status == EvtOrderStatus.HOP_LE || o.Status == EvtOrderStatus.CHO_THANH_TOAN) && o.Deleted == YesNo.NO).Sum(o => o.OrderDetails.Sum(od => od.Quantity)))),
                                                StartDate = e.evt.EventDetails.Where(ed => ed.Status == EventDetailStatus.KICH_HOAT && ed.Deleted == YesNo.NO && ed.StartDate != null && ed.StartDate >= DateTime.Now).OrderBy(ed => ed.StartDate).Select(e => e.StartDate).FirstOrDefault()
                                                                ?? e.evt.EventDetails.Where(ed => ed.Status == EventDetailStatus.KICH_HOAT && ed.Deleted == YesNo.NO && ed.StartDate != null).OrderBy(ed => ed.StartDate).Select(e => e.StartDate).LastOrDefault(),
                                                EndDate = e.evt.EventDetails.Where(ed => ed.Status == EventDetailStatus.KICH_HOAT && ed.Deleted == YesNo.NO).OrderByDescending(ed => ed.EndDate).Select(e => e.EndDate).FirstOrDefault()
                                            });

            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = query;
            return result;
        }

        public EvtEventDto Update(UpdateEvtEventDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Update)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");
            var transaction = _dbContext.Database.BeginTransaction();
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == input.Id && e.Deleted == YesNo.NO && e.TradingProviderId == tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);
            eventFind.Name = input.Name;
            eventFind.Organizator = input.Organizator;
            eventFind.Location = input.Location;
            eventFind.ProvinceCode = input.ProvinceCode;
            eventFind.Address = input.Address;
            eventFind.Latitude = input.Latitude;
            eventFind.Longitude = input.Longitude;
            eventFind.Viewing = input.Viewing;
            eventFind.ConfigContractCodeId = input.ConfigContractCodeId;
            eventFind.BackAccountId = input.BackAccountId;
            eventFind.Website = input.Website;
            eventFind.Facebook = input.Facebook;
            eventFind.Phone = input.Phone;
            eventFind.Email = input.Email;
            eventFind.IsShowApp = input.IsShowApp;
            eventFind.IsHighlight = input.IsHighlight;
            eventFind.CanExportRequestRecipt = input.CanExportRequestRecipt;
            eventFind.CanExportTicket = input.CanExportTicket;
            eventFind.Description = input.Description;
            eventFind.TicketPurchasePolicy = input.TicketPurchasePolicy;

            var eventTypes = _dbContext.EvtEventTypes.Where(e => e.EventId == eventFind.Id);

            foreach (var item in eventTypes)
            {
                if (!input.EventTypes.Contains(item.Type))
                {
                    _dbContext.EvtEventTypes.Remove(item);
                }
            }

            foreach (var item in input.EventTypes)
            {
                if (!eventTypes.Select(e => e.Type).Contains(item))
                {
                    var insertType = new EvtEventType
                    {
                        Id = (int)_evtEventTypeEFRepository.NextKey(),
                        EventId = eventFind.Id,
                        Type = item,
                    };
                    _dbContext.EvtEventTypes.Add(insertType);
                }
            }

            var adminEventFind = _dbContext.EvtAdminEvents.Where(s => s.EventId == eventFind.Id);
            _dbContext.EvtAdminEvents.RemoveRange(adminEventFind);
            _dbContext.SaveChanges();
            if (input.TicketInspectorIds != null)
            {
                foreach (var item in input.TicketInspectorIds)
                {
                    var insert = new EvtAdminEvent
                    {
                        Id = (int)_evtAdminEventEFRepository.NextKey(),
                        EventId = eventFind.Id,
                        InvestorId = item
                    };
                    _dbContext.EvtAdminEvents.Add(insert);
                }
                _dbContext.SaveChanges();
            }

            // Thêm nhiều tài khoản ngân hàng thu và chi
            UpdateTradingBankAccount(eventFind.Id, input.TradingBankAccounts);
            _evtHistoryUpdateEFRepository.Entity.Add(new EvtHistoryUpdate
            {
                Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                RealTableId = eventFind.Id,
                UpdateTable = EvtHistoryUpdateTables.EVT_EVENT,
                Action = ActionTypes.CAP_NHAT,
                Summary = "Cập nhật sự kiện"
            });
            _dbContext.SaveChanges();
            transaction.Commit();
            return new EvtEventDto
            {
                Id = eventFind.Id,
                Name = eventFind.Name,
                Organizator = eventFind.Organizator,
                Location = eventFind.Location,
                ProvinceCode = eventFind.ProvinceCode,
                Address = eventFind.Address,
                Latitude = input.Latitude,
                Longitude = eventFind.Longitude,
                Viewing = eventFind.Viewing,
                ConfigContractCodeId = eventFind.ConfigContractCodeId,
                BackAccountId = eventFind.BackAccountId,
                Website = eventFind.Website,
                Facebook = eventFind.Facebook,
                Phone = eventFind.Phone,
                Email = eventFind.Email,
                IsShowApp = eventFind.IsShowApp,
                IsHighlight = eventFind.IsHighlight,
                CanExportRequestRecipt = eventFind.CanExportRequestRecipt,
                CanExportTicket = eventFind.CanExportTicket,
                Description = eventFind.Description,
            };
        }

        public void UpdateStatus(int id, int status)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateStatus)} : id = {id}, status = {status}, tradingProviderId = {tradingProviderId}");
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == id && e.Deleted == YesNo.NO && e.TradingProviderId == tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);
            eventFind.Status = status;
            _evtHistoryUpdateEFRepository.Entity.Add(new EvtHistoryUpdate
            {
                Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                RealTableId = eventFind.Id,
                OldValue = "Khởi tạo",
                NewValue = "Mở bán",
                FieldName = "Trạng thái",
                UpdateTable = EvtHistoryUpdateTables.EVT_EVENT,
                Action = ActionTypes.CAP_NHAT,
                Summary = "Mở bán vé sự kiện"
            });
            _dbContext.SaveChanges();
        }

        public void ChangeIsShowApp(int id)
        {
            var tradingproviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeIsShowApp)} : id = {id}, tradingproviderId = {tradingproviderId}");
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == id && e.Deleted == YesNo.NO && e.TradingProviderId == tradingproviderId).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);
            if (eventFind.IsShowApp == YesNo.NO)
            {
                eventFind.IsShowApp = YesNo.YES;
                _evtHistoryUpdateEFRepository.Entity.Add(new EvtHistoryUpdate
                {
                    Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                    RealTableId = eventFind.Id,
                    FieldName = "IsShowApp",
                    UpdateTable = EvtHistoryUpdateTables.EVT_EVENT,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = "Bật show app"
                });
            }
            else
            {
                eventFind.IsShowApp = YesNo.NO;
                _evtHistoryUpdateEFRepository.Entity.Add(new EvtHistoryUpdate
                {
                    Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                    RealTableId = eventFind.Id,
                    FieldName = "IsShowApp",
                    UpdateTable = EvtHistoryUpdateTables.EVT_EVENT,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = "Tắt show app"
                });
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật Mô tả tổng quan sự kiện
        /// </summary>
        public void UpdateEventOverviewContent(UpdateEventOverviewContentDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateEventOverviewContent)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId={tradingProviderId}");
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == input.Id && e.Deleted == YesNo.NO && e.TradingProviderId == tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);
            eventFind.OverviewContent = input.OverviewContent;
            eventFind.ContentType = input.ContentType;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Get theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EvtEventDto FindById(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindById)} : id = {id}");

            var eventFind = _dbContext.EvtEvents.Include(e => e.EventTypes).Include(e => e.Province).Include(e => e.EvtEventBankAccounts)
                .Include(e => e.AdminEvents).ThenInclude(e => e.Investor).ThenInclude(i => i.InvestorIdentifications)
                .Where(e => e.Deleted == YesNo.NO && e.TradingProviderId == tradingProviderId)
                .Select(e => new EvtEventDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Organizator = e.Organizator,
                    Location = e.Location,
                    ProvinceCode = e.ProvinceCode,
                    Address = e.Address,
                    Latitude = e.Latitude,
                    Longitude = e.Longitude,
                    Viewing = e.Viewing,
                    ConfigContractCodeId = e.ConfigContractCodeId,
                    BackAccountId = e.BackAccountId,
                    Website = e.Website,
                    Facebook = e.Facebook,
                    Phone = e.Phone,
                    IsCheck = e.IsCheck,
                    Email = e.Email,
                    Status = e.Status,
                    IsShowApp = e.IsShowApp,
                    IsHighlight = e.IsHighlight,
                    CanExportRequestRecipt = e.CanExportRequestRecipt,
                    CanExportTicket = e.CanExportTicket,
                    Description = e.Description,
                    OverviewContent = e.OverviewContent,
                    ContentType = e.ContentType,
                    TicketPurchasePolicy = e.TicketPurchasePolicy,
                    ProvinceName = e.Province.Name,
                    TradingBankAccounts = e.EvtEventBankAccounts.Select(e => e.BusinessCustomerBankAccId),
                    TicketInspector = e.AdminEvents.Select(ae => new EvtAdminEventDto
                    {
                        Id = ae.Id,
                        EventId = ae.EventId,
                        EventName = ae.Event.Name,
                        InvestorId = ae.InvestorId,
                        Phone = ae.Investor.Phone,
                        FullName = ae.Investor.InvestorIdentifications.Where(i => i.IsDefault == YesNo.YES && i.Deleted == YesNo.NO).Select(e => e.Fullname).FirstOrDefault(),
                    }),
                    EventTypes = e.EventTypes.Select(e => e.Type)
                })
                .FirstOrDefault(e => e.Id == id).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);
            return eventFind;
        }

        /// <summary>
        /// Lấy danh sách sự kiện đang mở bán cho đặt lệnh
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvtEventDto> FindEventActive(bool? isApp = null)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindEventActive)} : tradingProviderId = {tradingProviderId}");
            var events = _dbContext.EvtEvents.Include(e => e.EventDetails).Include(e => e.Province).Where(e => e.Deleted == YesNo.NO && e.Status == EventStatus.DANG_MO_BAN
                                                && (isApp == null || e.IsShowApp == YesNo.YES)
                                                && e.EventDetails.Any(ed => ed.EndDate >= DateTime.Now)
                                                && e.TradingProviderId == tradingProviderId)
                                                .OrderByDescending(e => e.Id);
            return _mapper.Map<IEnumerable<EvtEventDto>>(events);
        }

        /// <summary>
        /// tạm dừng sự kiện và lý do tạm dừng
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task PauseEvent(CreateHistoryUpdateDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(PauseEvent)} : input = {JsonSerializer.Serialize(input)},tradingProviderId = {tradingProviderId}");
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == input.Id && e.Deleted == YesNo.NO && e.TradingProviderId == tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);
            var reasonPause = new EvtHistoryUpdate
            {
                Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                RealTableId = input.Id,
                OldValue = eventFind.Status.ToString(),
                NewValue = EventStatus.TAM_DUNG.ToString(),
                FieldName = EvtFieldName.EVENT_STATUS,
                UpdateTable = EvtHistoryUpdateTables.EVT_EVENT,
                Action = ActionTypes.CAP_NHAT,
                ActionUpdateType = EvtActionUpdateTypes.PAUSE_EVENT,
                UpdateReason = input.Reason,
                Summary = input.Summary,
            };
            eventFind.Status = EventStatus.TAM_DUNG;

            await _eventNotificationServices.SendNotifyPauseEvent(input.Id);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Hủy sự kiện và lý do hủy
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void CancelEvent(CreateHistoryUpdateDto input)
        {
            var tradingproviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(PauseEvent)} : input = {JsonSerializer.Serialize(input)},tradingproviderId = {tradingproviderId}");
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == input.Id && e.Deleted == YesNo.NO && e.TradingProviderId == tradingproviderId).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);
            var reasonPause = new EvtHistoryUpdate
            {
                Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                RealTableId = input.Id,
                OldValue = eventFind.Status.ToString(),
                NewValue = EventStatus.TAM_DUNG.ToString(),
                FieldName = EvtFieldName.EVENT_STATUS,
                UpdateTable = EvtHistoryUpdateTables.EVT_EVENT,
                Action = ActionTypes.CAP_NHAT,
                ActionUpdateType = EvtActionUpdateTypes.CANCEL_EVENT,
                UpdateReason = input.Reason,
                Summary = input.Summary,
            };
            eventFind.Status = EventStatus.HUY_SU_KIEN;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Thêm nhiều tài khoản ngân hàng thụ hưởng
        /// Chưa có thì thêm
        /// Có rồi thì giữ nguyên
        /// Đã có trong DB nhưng không truyền vào thì xóa
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="tradingBankAccounts"></param>
        /// <param name="username"></param>
        private void UpdateTradingBankAccount(int eventId, List<int> tradingBankAccounts)
        {
            _logger.LogInformation($"{nameof(UpdateTradingBankAccount)}: tradingBankAccounts = {JsonSerializer.Serialize(tradingBankAccounts)}");

            //Lấy danh sách Tài khoản ngân hàng thụ hưởng của đại lý
            var updateTradingBankAccountFind = _evtEventBankAccountEFRepository.Entity.Where(b => b.EventId == eventId && b.Deleted == YesNo.NO);

            if (tradingBankAccounts != null && tradingBankAccounts.Count != 0)
            {
                //Xóa đi những ngân hàng ko được truyền vào
                var updateTradingBankAccountRemove = updateTradingBankAccountFind.Where(p => !tradingBankAccounts.Contains(p.BusinessCustomerBankAccId));
                foreach (var bankAccountItem in updateTradingBankAccountRemove)
                {
                    bankAccountItem.Deleted = YesNo.YES;
                }

                foreach (var item in tradingBankAccounts)
                {
                    // Nếu là thêm mới thì thêm vào
                    // Nếu loại ngân hàng chưa có trong list thì thêm vào, nếu đã có thì giữ nguyên
                    if (!updateTradingBankAccountFind.Select(p => p.BusinessCustomerBankAccId).Contains(item))
                    {
                        var findBank = _dbContext.BusinessCustomerBanks.FirstOrDefault(b => b.BusinessCustomerBankAccId == item && b.Deleted == YesNo.NO);
                        if (findBank == null)
                        {
                            _logger.LogError($" {nameof(UpdateTradingBankAccount)}:Không tìm thấy ngân hàng doanh nghiệp BusinessCustomerBankAccId = {item}");
                            continue;
                        }
                        _evtEventBankAccountEFRepository.Entity.Add(new EvtEventBankAccount
                        {
                            Id = (int)_evtEventBankAccountEFRepository.NextKey(),
                            EventId = eventId,
                            BusinessCustomerBankAccId = item,
                        });
                    }
                }
            }
        }

        public void ChangeIsCheck(int id)
        {
            var tradingproviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeIsCheck)} : id = {id}, tradingproviderId = {tradingproviderId}");
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == id && e.Deleted == YesNo.NO && e.TradingProviderId == tradingproviderId).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);
            if (eventFind.IsCheck == YesNo.NO)
            {
                eventFind.IsCheck = YesNo.YES;
            }
            else
            {
                eventFind.IsCheck = YesNo.NO;
            }
            _dbContext.SaveChanges();
        }

        public IEnumerable<ViewEventByTradingDto> FindEventSellBehalfByTrading()
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var (tradingProviderId, tradingBanHo) = userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER
             ? (CommonUtils.GetCurrentTradingProviderId(_httpContext), _saleEFRepository.FindTradingProviderBanHo(CommonUtils.GetCurrentTradingProviderId(_httpContext)))
             : (0, new List<int>());

            var result = _dbContext.EvtEvents
                .Include(e => e.EventDetails)
                .Where(e => (userType == UserTypes.ROOT_EPIC 
                                || (tradingBanHo.Contains(e.TradingProviderId) || e.TradingProviderId == tradingProviderId)) 
                            && e.IsShowApp == YesNo.YES 
                            && e.Status == EventStatus.DANG_MO_BAN 
                            && e.Deleted == YesNo.NO)
                .Select(e => new ViewEventByTradingDto
                {
                    Id = e.Id,
                    Code = e.Organizator,
                    Name = e.Name,
                    IsSalePartnership = tradingBanHo.Contains(e.TradingProviderId)
                })
                .AsEnumerable();

            return result;
        }
        #endregion
    }
}
