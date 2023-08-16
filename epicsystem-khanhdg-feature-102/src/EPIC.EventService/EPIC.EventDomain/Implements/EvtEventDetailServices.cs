using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtEventDetail;
using EPIC.EventEntites.Dto.EvtTicket;
using EPIC.EventEntites.Entites;
using EPIC.EventRepositories;
using EPIC.FileDomain.Services;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.Linq;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.EventDomain.Implements
{
    public class EvtEventDetailServices : IEvtEventDetailServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EvtEventDetailServices> _logger;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly EventNotificationServices _eventNotificationServices;
        private readonly EvtBackgroundJobServices _evtBackgroundJobServices;
        private readonly EvtEventEFRepository _evtEventEFRepository;
        private readonly EvtEventDetailEFRepository _evtEventDetailEFRepository;
        private readonly IFileServices _fileServices;

        public EvtEventDetailServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<EvtEventDetailServices> logger,
            IHttpContextAccessor httpContextAccessor,
            IFileServices fileServices,
            IBackgroundJobClient backgroundJobs,
            EventNotificationServices eventNotificationServices,
            EvtBackgroundJobServices evtBackgroundJobServices
           )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _backgroundJobs = backgroundJobs;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _eventNotificationServices = eventNotificationServices;
            _evtBackgroundJobServices = evtBackgroundJobServices;
            _evtEventEFRepository = new EvtEventEFRepository(_dbContext, _logger);
            _evtEventDetailEFRepository = new EvtEventDetailEFRepository(_dbContext, _logger);
            _fileServices = fileServices;
        }

        #region Chi tiết sự kiện (Các khung giờ sự kiện)

        public EvtEventDetailDto AddEventDetail(CreateEvtEventDetailDto input)
        {
            _logger.LogInformation($"{nameof(AddEventDetail)} : input = {JsonSerializer.Serialize(input)}");
            EvtEventDetail inputInsert = _mapper.Map<EvtEventDetail>(input);
            inputInsert.Id = (int)_evtEventDetailEFRepository.NextKey();
            var result = _dbContext.EvtEventDetails.Add(inputInsert);

            //Background chạy sự kiện kết thúc
            EventFinishedJob(input.EventId);

            _dbContext.SaveChanges();
            return _mapper.Map<EvtEventDetailDto>(inputInsert);
        }

        public void DeleteEventDetail(int id)
        {
            _logger.LogInformation($"{nameof(DeleteEventDetail)} : id = {id}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var eventDetailFind = _dbContext.EvtEventDetails.Include(e => e.Event)
                                        .FirstOrDefault(e => e.Id == id && e.Event.TradingProviderId == tradingProviderId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventDetailNotFound);


            var tickets = _dbContext.EvtTickets.Where(t => t.EventDetailId == id);
            foreach (var ticket in tickets)
            {
                ticket.Deleted = YesNo.NO;
                var ticketMedias = _dbContext.EvtTicketMedias.Where(tm => tm.TicketId == ticket.Id);
                foreach (var ticketMedia in ticketMedias)
                {
                    _dbContext.EvtTicketMedias.Remove(ticketMedia);
                    _dbContext.SaveChanges();

                    var image = _dbContext.EvtTicketMedias.Where(e => e.UrlImage == ticketMedia.UrlImage);
                    if (!image.Any())
                    {
                        //Xóa ảnh
                        _fileServices.DeleteFile(ticketMedia.UrlImage);
                    }
                }
            }

            //Nếu là bản ghi mới mà không bấm lưu hoặc không tao tác gì thì xóa hẳn bản ghi và các dữ liệu vé bên trong
            if (eventDetailFind.Status == EventDetailStatus.NHAP)
            {
                _dbContext.EvtEventDetails.Remove(eventDetailFind);
                foreach (var ticket in tickets)
                {
                    _dbContext.EvtTickets.Remove(ticket);
                }
            }
            if (eventDetailFind.UpcomingJobId != null)
            {
                // Xóa job cũ, sinh job mới
                _backgroundJobs.Delete(eventDetailFind.UpcomingJobId);
            }
            eventDetailFind.Deleted = YesNo.YES;
            //Background chạy sự kiện kết thúc
            EventFinishedJob(eventDetailFind.EventId);
            _dbContext.SaveChanges();
        }

        public PagingResult<EvtEventDetailDto> FindAllEventDetail(FilterEvtEventDetailDto input)
        {
            _logger.LogInformation($"{nameof(FindAllEventDetail)} : input = {JsonSerializer.Serialize(input)}");
            var result = new PagingResult<EvtEventDetailDto>();
            var query = _dbContext.EvtEventDetails.Include(e => e.Tickets)
                                            .Include(e => e.Orders).ThenInclude(o => o.OrderDetails).ThenInclude(od => od.OrderTicketDetails)
                                            .Where(e => e.Deleted == YesNo.NO && e.EventId == input.EventId && e.Status == EventDetailStatus.KICH_HOAT)
                                            .Select(e => new EvtEventDetailDto
                                            {
                                                Id = e.Id,
                                                StartDate = e.StartDate,
                                                EndDate = e.EndDate,
                                                CreatedBy = e.CreatedBy,
                                                Status = e.Status,
                                                TicketTypeQuantity = e.Tickets.Where(t => t.Deleted == YesNo.NO).Count(),
                                                TicketQuantity = e.Tickets.Sum(t => t.Quantity),
                                                RegisterQuantity = e.Orders.Where(o => o.Deleted == YesNo.NO).Sum(o => o.OrderDetails.Sum(od => od.Quantity)),
                                                PayQuantity = e.Orders.Where(o => o.Status == EvtOrderStatus.HOP_LE).Sum(o => o.OrderDetails.Sum(o => o.Quantity)),
                                                PaymentWaitingTime = e.PaymentWaittingTime,
                                                IsShowRemaingTicketApp = e.IsShowRemaingTicketApp,
                                                Tickets = e.Tickets.Where(t => t.Deleted == YesNo.NO)
                                                        .Select(t => new EvtTicketDto
                                                        {
                                                            Id = t.Id,
                                                            StartSellDate = t.StartSellDate,
                                                            EndSellDate = t.EndSellDate,
                                                            EventDetailId = t.EventDetailId,
                                                            OverviewContent = t.OverviewContent,
                                                            ContentType = t.ContentType,
                                                            Name = t.Name,
                                                            CreatedBy = t.CreatedBy,
                                                            Description = t.Description,
                                                            IsFree = t.IsFree,
                                                            IsShowApp = t.IsShowApp,
                                                            MaxBuy = t.MaxBuy,
                                                            MinBuy = t.MinBuy,
                                                            Price = t.Price,
                                                            Status = t.Status,
                                                            Quantity = t.Quantity,
                                                            PayQuantity = _dbContext.EvtOrderDetails.Include(o => o.Order)
                                                                    .Where(od => od.Order.Status == EvtOrderStatus.HOP_LE && od.Order.Deleted == YesNo.NO
                                                                    && od.Order.EventDetailId == e.Id && od.TicketId == t.Id)
                                                                    .Sum(e => e.Quantity),
                                                            RegisterQuantity = _dbContext.EvtOrderDetails.Include(o => o.Order)
                                                                    .Where(od => od.Order.Deleted == YesNo.NO && od.Order.EventDetailId == e.Id && od.TicketId == t.Id
                                                                    && (od.Order.Status == EvtOrderStatus.KHOI_TAO || od.Order.Status == EvtOrderStatus.CHO_XU_LY || od.Order.Status == EvtOrderStatus.CHO_THANH_TOAN))
                                                                    .Sum(e => e.Quantity),
                                                        })
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

        public EvtEventDetailDto UpdateEventDetail(UpdateEvtEventDetailDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateEventDetail)} : input = {JsonSerializer.Serialize(input)}");
            var eventDetailFind = _dbContext.EvtEventDetails.FirstOrDefault(e => e.Id == input.Id && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventDetailNotFound);
            eventDetailFind.StartDate = input.StartDate;
            eventDetailFind.EndDate = input.EndDate;
            eventDetailFind.IsShowRemaingTicketApp = input.IsShowRemaingTicketApp;
            eventDetailFind.PaymentWaittingTime = input.PaymentWaittingTime;
            if (eventDetailFind.Status == EventDetailStatus.NHAP)
            {
                eventDetailFind.Status = EventDetailStatus.KICH_HOAT;
            }

            var configTimeSendNoti = _dbContext.TradingProviderConfigs.FirstOrDefault(c => c.TradingProviderId == tradingProviderId && c.Key == TradingProviderConfigKeys.EventTimeSendNotiEventUpcomingForCustomer
                                   && c.Deleted == YesNo.NO);
            if (configTimeSendNoti != null && eventDetailFind.StartDate != null)
            {
                var configDaySendNoti = _dbContext.TradingProviderConfigs.FirstOrDefault(c => c.TradingProviderId == tradingProviderId && c.Key == TradingProviderConfigKeys.EventDaySendNotiEventUpcomingForCustomer
                                        && c.Deleted == YesNo.NO);

                string[] time = configTimeSendNoti.Value.Split(':');
                int hours = int.Parse(time[0]);
                int minutes = int.Parse(time[1]);

                int day = int.TryParse(configDaySendNoti?.Value, out day) ? day : 1;
                // Lấy giờ gửi theo múi giờ UTC+7
                DateTime timeSend = input.StartDate.Value.Date.AddDays(-day).AddHours(hours).AddMinutes(minutes);

                if (eventDetailFind.UpcomingJobId != null)
                {
                    _backgroundJobs.Delete(eventDetailFind.UpcomingJobId);
                    eventDetailFind.UpcomingJobId = null;
                }

                if (timeSend > DateTime.Now)
                {
                    string jobId = _backgroundJobs.Schedule(() => _evtBackgroundJobServices.SendEventUpComing(eventDetailFind.Id), timeSend);
                    eventDetailFind.UpcomingJobId = jobId;
                }
                _dbContext.SaveChanges();
            }

            //Background chạy sự kiện kết thúc
            EventFinishedJob(eventDetailFind.EventId);

            _dbContext.SaveChanges();
            return _mapper.Map<EvtEventDetailDto>(eventDetailFind);
        }

        public void ChangeStatusEventDetail(int id)
        {
            _logger.LogInformation($"{nameof(ChangeStatusEventDetail)} : id = {id}");
            var eventDetailFind = _dbContext.EvtEventDetails.FirstOrDefault(e => e.Id == id && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventDetailNotFound);
            if (eventDetailFind.Status == EventDetailStatus.KICH_HOAT)
            {
                eventDetailFind.Status = EventDetailStatus.HUY_KICH_HOAT;
            }
            else
            {
                eventDetailFind.Status = EventDetailStatus.KICH_HOAT;
            }
            _dbContext.SaveChanges();
            EventFinishedJob(eventDetailFind.EventId);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Lấy khung giờ theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EvtEventDetailDto FindDetailById(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindDetailById)}: id = {id}, tradingProviderId = {tradingProviderId}");
            var eventDetailFind = _dbContext.EvtEventDetails.Include(e => e.Tickets).Include(e => e.Orders)
                                                            .Where(ed => ed.Deleted == YesNo.NO)
                                                            .Select(ed => new EvtEventDetailDto
                                                            {
                                                                Id = ed.Id,
                                                                StartDate = ed.StartDate,
                                                                EndDate = ed.EndDate,
                                                                EventId = ed.EventId,
                                                                TicketTypeQuantity = ed.Tickets.Where(t => t.Deleted == YesNo.NO).Count(),
                                                                TicketQuantity = ed.Tickets.Sum(t => t.Quantity),
                                                                RegisterQuantity = ed.Orders.Where(o => o.Deleted == YesNo.NO).Sum(o => o.OrderDetails.Sum(od => od.Quantity)),
                                                                PayQuantity = ed.Orders.Where(o => o.Status == EvtOrderStatus.HOP_LE).Sum(o => o.OrderDetails.Sum(o => o.Quantity)),
                                                                PaymentWaitingTime = ed.PaymentWaittingTime,
                                                                IsShowRemaingTicketApp = ed.IsShowRemaingTicketApp,
                                                                Tickets = ed.Tickets.Where(t => t.Deleted == YesNo.NO)
                                                                        .Select(t => new EvtTicketDto
                                                                        {
                                                                            Id = t.Id,
                                                                            StartSellDate = t.StartSellDate,
                                                                            EndSellDate = t.EndSellDate,
                                                                            EventDetailId = t.EventDetailId,
                                                                            OverviewContent = t.OverviewContent,
                                                                            ContentType = t.ContentType,
                                                                            Name = t.Name,
                                                                            CreatedBy = t.CreatedBy,
                                                                            Description = t.Description,
                                                                            IsFree = t.IsFree,
                                                                            IsShowApp = t.IsShowApp,
                                                                            MaxBuy = t.MaxBuy,
                                                                            MinBuy = t.MinBuy,
                                                                            Price = t.Price,
                                                                            Status = t.Status,
                                                                            Quantity = t.Quantity,
                                                                            PayQuantity = _dbContext.EvtOrderDetails.Include(o => o.Order)
                                                                                    .Where(od => od.Order.Status == EvtOrderStatus.HOP_LE && od.Order.Deleted == YesNo.NO
                                                                                    && od.Order.EventDetailId == ed.Id && od.TicketId == t.Id)
                                                                                    .Sum(e => e.Quantity),
                                                                            RegisterQuantity = ed.Orders.Sum(e => e.OrderDetails.Where(od => od.TicketId == t.Id).Sum(od => od.Quantity))
                                                                        })
                                                            })
                                                            .FirstOrDefault(e => e.Id == id);
            return eventDetailFind;
        }

        /// <summary>
        /// Lấy danh sách sự kiện các khung giờ đang hoạt động cho đặt lệnh
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isApp"></param>
        /// <returns></returns>
        public IEnumerable<EvtEventDetailDto> FindDetailActiveById(int id, bool? isApp = null)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindDetailActiveById)} : id = {id}");
            var result = new List<EvtEventDetailDto>();
            var eventDetailFind = _dbContext.EvtEventDetails.Include(e => e.Tickets)
                                                            .Include(e => e.Event).ThenInclude(ev => ev.EventTypes)
                                                            .Where(e => e.EventId == id && e.Deleted == YesNo.NO && e.Status == EventDetailStatus.KICH_HOAT)
                                                            .Select(ed => new EvtEventDetailDto
                                                            {
                                                                Id = ed.Id,
                                                                StartDate = ed.StartDate,
                                                                EndDate = ed.EndDate,
                                                                EventTypes = ed.Event.EventTypes.Select(e => e.Type),
                                                                EventId = ed.EventId,
                                                                RemainingTickets = ed.Tickets.Sum(t => t.Quantity)
                                                                    - ed.Orders.Where(o => (o.Status == EvtOrderStatus.HOP_LE || o.Status == EvtOrderStatus.CHO_THANH_TOAN) && o.Deleted == YesNo.NO).Sum(o => o.OrderDetails.Sum(od => od.Quantity)),
                                                                Tickets = ed.Tickets.Where(t => t.Deleted == YesNo.NO && t.Status == EvtTicketStatus.KICH_HOAT
                                                                                    && (isApp == null || t.IsShowApp == YesNo.YES)
                                                                                    )
                                                                .Select(t => new EvtTicketDto
                                                                {
                                                                    Id = t.Id,
                                                                    StartSellDate = t.StartSellDate,
                                                                    EndSellDate = t.EndSellDate,
                                                                    EventDetailId = t.EventDetailId,
                                                                    OverviewContent = t.OverviewContent,
                                                                    ContentType = t.ContentType,
                                                                    Name = t.Name,
                                                                    CreatedBy = t.CreatedBy,
                                                                    Description = t.Description,
                                                                    IsFree = t.IsFree,
                                                                    IsShowApp = t.IsShowApp,
                                                                    MaxBuy = t.MaxBuy,
                                                                    MinBuy = t.MinBuy,
                                                                    Price = t.Price,
                                                                    Status = t.Status,
                                                                    RemainingTickets = t.Quantity - ed.Orders.Where(o => (o.Status == EvtOrderStatus.HOP_LE || o.Status == EvtOrderStatus.CHO_THANH_TOAN) && o.Deleted == YesNo.NO)
                                                                                                             .Sum(o => o.OrderDetails.Where(od => od.TicketId == t.Id).Sum(od => od.Quantity))
                                                                })
                                                            });
            if (eventDetailFind.Any())
            {
                result = _mapper.Map<List<EvtEventDetailDto>>(eventDetailFind);
            }
            return result;
        }

        #endregion

        private void EventFinishedJob(int eventId)
        {
            //Background chạy sự kiện kết thúc
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == eventId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);
            var eventDetails = _dbContext.EvtEventDetails.Include(e => e.Tickets)
                                                         .Include(e => e.Orders).ThenInclude(o => o.OrderDetails).ThenInclude(od => od.OrderTicketDetails)
                                                         .Where(e => e.EventId == eventId && e.Deleted == YesNo.NO && e.Status == EventDetailStatus.KICH_HOAT);
            if (eventFind.EventFinishedJobId != null)
            {
                // Xóa job cũ, sinh job mới
                _backgroundJobs.Delete(eventFind.EventFinishedJobId);
            }
            if (eventDetails.Any())
            {
                var jobId = _backgroundJobs.Schedule(() => _evtBackgroundJobServices.SendEventFinished(eventFind.Id), eventDetails.Max(e => e.EndDate).Value);
                eventFind.EventFinishedJobId = jobId;
            }
        }
    }
}
