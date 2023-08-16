using AutoMapper;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.EntitiesBase.Dto;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtEventDescriptionMedia;
using EPIC.EventEntites.Entites;
using EPIC.EventRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using EPIC.EventDomain;
using System.IO;
using EPIC.Entities.DataEntities;

namespace EPIC.EventDomain.Implements
{
    public class EvtAppEventService : IEvtAppEventService
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EvtAppEventService> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly EvtEventEFRepository _evtEventEFRepository;
        private readonly EvtEventDetailEFRepository _evtEventDetailEFRepository;
        private readonly EvtInterestedPersonEFRepository _evtInterestedPersonEFRepository;
        private readonly ILogicInvestorTradingSharedServices _logicInvestorTradingSharedServices;
        private readonly EvtSearchHistoryEFRepository _evtSearchHistoryEFRepository;

        public EvtAppEventService(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<EvtAppEventService> logger,
            IHttpContextAccessor httpContextAccessor,
            ILogicInvestorTradingSharedServices logicInvestorTradingSharedServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _evtEventEFRepository = new EvtEventEFRepository(_dbContext, _logger);
            _evtEventDetailEFRepository = new EvtEventDetailEFRepository(_dbContext, _logger);
            _evtInterestedPersonEFRepository = new EvtInterestedPersonEFRepository(_dbContext, _logger);
            _logicInvestorTradingSharedServices = logicInvestorTradingSharedServices;
            _evtSearchHistoryEFRepository = new EvtSearchHistoryEFRepository(_dbContext, _logger);
        }

        /// <summary>
        /// Tìm kiếm sự kiện theo tên
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<AppEvtSearchEventDto> SearchEvent(AppFilterEventDto input)
        {

            _logger.LogInformation($"{nameof(SearchEvent)} : input = {JsonSerializer.Serialize(input)}");
            int? investorId = null;
            try
            {
                investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            }
            catch
            {
                investorId = null;
            }
            //List danh sách trading của investor
            var listInvestorTradingIds = _logicInvestorTradingSharedServices.FindListInvestorTradingProviderForApp(investorId);
            //Danh sách trading của sale
            var listSaleTradingIds = _logicInvestorTradingSharedServices.FindListSaleTradingProviderForApp(investorId);
            var result = new PagingResult<AppEvtSearchEventDto>();
            var query = _dbContext.EvtEvents.Include(e => e.EventDetails)
                                             .Include(e => e.EventMedias)
                                             .Where(e => e.Deleted == YesNo.NO
                                                     && e.IsShowApp == YesNo.YES
                                                     && (input.Name == null || e.Name.ToLower().Contains(input.Name.ToLower()))
                                                     && e.Status != EventStatus.HUY_SU_KIEN
                                                      && ((e.Viewing == EvtEventViewer.INVESTOR_TRADED && listInvestorTradingIds.Contains(e.TradingProviderId))
                                                        || (e.Viewing == EvtEventViewer.INVESTOR_NOT_TRADED && listInvestorTradingIds.Count() == 0)
                                                        || (e.Viewing == EvtEventViewer.SALE && listSaleTradingIds.Contains(e.TradingProviderId))
                                                        || ((e.Viewing == EvtEventViewer.ALL && listInvestorTradingIds.Count() == 0)
                                                        || (e.Viewing == EvtEventViewer.ALL && (listInvestorTradingIds.Contains(e.TradingProviderId) || listSaleTradingIds.Contains(e.TradingProviderId))))
                                                       ))
                                             .Select(e => new
                                             {
                                                 Event = e,
                                                 MinStartDate = e.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.EndDate >= DateTime.Now)
                                                         .Select(ed => (DateTime?)ed.StartDate)
                                                         .Min(),
                                                 MaxEndDate = e.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.EndDate >= DateTime.Now)
                                                         .Select(ed => (DateTime?)ed.EndDate)
                                                         .Max(),
                                             })
                                             .Select(e => new AppEvtSearchEventDto
                                             {
                                                 Id = e.Event.Id,
                                                 StartDate = e.MinStartDate,
                                                 EndDate = e.MaxEndDate,
                                                 Name = e.Event.Name,
                                                 UrlImage = e.Event.EventMedias.Where(em => em.Deleted == YesNo.NO)
                                                                               .Select(em => em.UrlImage)
                                                                               .FirstOrDefault()
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

        /// <summary>
        /// danh sách lịch sử tìm kiếm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<AppEvtSearchEventDto> SearchHistoryEvent(PagingRequestBaseDto input)
        {
            _logger.LogInformation($"{nameof(SearchHistoryEvent)}, ");

            var result = new PagingResult<AppEvtSearchEventDto>();

            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var query = _dbContext.EvtEvents.Include(e => e.EventDetails)
                                         .Include(e => e.SearchHistorys)
                                         .Include(e => e.EventMedias)
                                         .Where(e => e.Deleted == YesNo.NO
                                                 && e.IsShowApp == YesNo.YES
                                                 && e.Status != EventStatus.HUY_SU_KIEN
                                                 && e.SearchHistorys.Any(s => s.InvestorId == investorId)
                                                 )
                                         .Select(e => new
                                         {
                                             Event = e,
                                             MinStartDate = e.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.EndDate >= DateTime.Now && ed.Status == EventDetailStatus.KICH_HOAT)
                                                     .Select(ed => (DateTime?)ed.StartDate)
                                                     .Min(),
                                             MaxEndDate = e.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.EndDate >= DateTime.Now && ed.Status == EventDetailStatus.KICH_HOAT)
                                                     .Select(ed => (DateTime?)ed.EndDate)
                                                     .Max(),
                                         })
                                         .Select(e => new AppEvtSearchEventDto
                                         {
                                             Id = e.Event.Id,
                                             StartDate = e.MinStartDate,
                                             EndDate = e.MaxEndDate,
                                             Name = e.Event.Name,
                                             UrlImage = e.Event.EventMedias.Where(em => em.Deleted == YesNo.NO)
                                                                           .Select(em => em.UrlImage)
                                                                           .FirstOrDefault()
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

        /// <summary>
        /// lưu lịch sử tìm kiếm
        /// </summary>
        /// <param name="eventId"></param>
        public void AddSearchHistoryEvent(int eventId)
        {
            _logger.LogInformation($"{nameof(AddSearchHistoryEvent)} : eventId = {eventId}");


            var eventFind = _dbContext.EvtEvents.FirstOrDefault(i => i.Id == eventId).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            var isInterested = _dbContext.EvtSearchHistorys.FirstOrDefault(i => i.InvestorId == investorId && i.EventId == eventId);
            if (isInterested != null)
            {
                _dbContext.EvtSearchHistorys.Remove(isInterested);
            }

            var insert = new EvtSearchHistory
            {
                Id = (int)_evtSearchHistoryEFRepository.NextKey(),
                InvestorId = (int)investorId,
                EventId = eventId,
            };

            _dbContext.EvtSearchHistorys.Add(insert);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// xóa tất cả lịch sử tìm kiếm của investor
        /// </summary>
        public void DeleteSearchHistoryEvent()
        {
            _logger.LogInformation($"{nameof(DeleteSearchHistoryEvent)}");

            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var isInterested = _dbContext.EvtSearchHistorys.Where(i => i.InvestorId == investorId);
            _dbContext.EvtSearchHistorys.RemoveRange(isInterested);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// lấy list đại lý của investor
        /// </summary>
        /// <param name="isSaleView"></param>
        /// <returns></returns>
        private List<int> GetListTradingProviderIds(bool isSaleView)
        {
            int? investorId = null;
            try
            {
                investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            }
            catch
            {
                investorId = null;
            }
            var listTradingProviderIds = _logicInvestorTradingSharedServices.FindListTradingProviderForApp(investorId, isSaleView);
            return listTradingProviderIds;
        }

        /// <summary>
        /// Chi tiết event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AppEventDetailsDto FindEventDetailsById(int id, bool isSaleView)
        {
            _logger.LogInformation($"{nameof(FindEventDetailsById)} : id = {id}, isSaleView = {isSaleView}");

            int? investorId = null;
            try
            {
                investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
                var isInterested = _dbContext.EvtInterestedPeople.Any(i => i.InvestorId == investorId && i.EventId == id);
                if (!isInterested)
                {
                    var insert = new EvtInterestedPerson
                    {
                        Id = (int)_evtInterestedPersonEFRepository.NextKey(),
                        InvestorId = (int)investorId,
                        EventId = id,
                    };
                    _ = _dbContext.EvtInterestedPeople.Add(insert);
                    _dbContext.SaveChanges();
                }
            }
            catch
            {
                investorId = null;
            }

            //List danh sách trading của investor
            var listInvestorTradingIds = _logicInvestorTradingSharedServices.FindListInvestorTradingProviderForApp(investorId);
            //Danh sách trading của sale
            var listSaleTradingIds = _logicInvestorTradingSharedServices.FindListSaleTradingProviderForApp(investorId);
            var evtEvent = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == id && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);

            var result = new AppEventDetailsDto();
            var query = _dbContext.EvtEvents.Include(e => e.Province)
                                        .Include(e => e.InterestedPeople).ThenInclude(i => i.Investor)
                                        .Include(e => e.EventMedias)
                                        .Include(e => e.EventTypes)
                                        .Include(e => e.EventDetails).ThenInclude(ed => ed.Tickets).ThenInclude(t => t.TicketMedias)
                                        .Include(e => e.EventDetails).ThenInclude(ed => ed.Orders).ThenInclude(o => o.OrderDetails)
                                        .Include(e => e.EventDescriptionMedias)
                                        .Where(e => e.Deleted == YesNo.NO && e.IsShowApp == YesNo.YES
                                        && e.Id == id
                                          && ((e.Viewing == EvtEventViewer.INVESTOR_TRADED && listInvestorTradingIds.Contains(e.TradingProviderId))
                                                        || (e.Viewing == EvtEventViewer.INVESTOR_NOT_TRADED && listInvestorTradingIds.Count() == 0)
                                                        || (e.Viewing == EvtEventViewer.SALE && listSaleTradingIds.Contains(e.TradingProviderId))
                                                        || ((e.Viewing == EvtEventViewer.ALL && listInvestorTradingIds.Count() == 0)
                                                        || (e.Viewing == EvtEventViewer.ALL && (listInvestorTradingIds.Contains(e.TradingProviderId) || listSaleTradingIds.Contains(e.TradingProviderId))))
                                        ))
                                        .Select(e => new AppEventDetailsDto
                                        {
                                            Id = e.Id,
                                            MinPrice = e.EventDetails.Where(ed => ed.Deleted == YesNo.NO
                                                            && ed.Status == EventDetailStatus.KICH_HOAT
                                                            && ed.Tickets.Any(t => t.Deleted == YesNo.NO))
                                                            .SelectMany(ed => ed.Tickets).Where(ti => ti.Deleted == YesNo.NO)
                                                            .Select(ti => ti.Price).Min(),
                                            StartDate = e.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.StartDate != null && ed.StartDate >= DateTime.Now
                                                                                && ed.Status == EventDetailStatus.KICH_HOAT)
                                                                      .OrderBy(ed => ed.StartDate)
                                                                      .Select(e => e.StartDate)
                                                                      .FirstOrDefault()
                                                     ?? e.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.StartDate != null && ed.Status == EventDetailStatus.KICH_HOAT)
                                                                      .OrderBy(ed => ed.StartDate)
                                                                      .Select(e => e.StartDate)
                                                                      .LastOrDefault(),
                                            EndDate = e.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.EndDate != null && ed.EndDate >= DateTime.Now
                                                                            && ed.Status == EventDetailStatus.KICH_HOAT)
                                                                    .OrderByDescending(ed => ed.EndDate)
                                                                    .Select(e => e.EndDate)
                                                                    .FirstOrDefault()
                                                     ?? e.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.EndDate != null && ed.Status == EventDetailStatus.KICH_HOAT)
                                                                      .OrderBy(ed => ed.EndDate)
                                                                      .Select(e => e.EndDate)
                                                                      .LastOrDefault(),
                                            Name = e.Name,
                                            IsFree = !e.EventDetails.Any(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT && ed.Tickets.Any(t => t.Status == EvtTicketStatus.KICH_HOAT && !t.IsFree)),
                                            EventTypes = e.EventTypes.Select(e => e.Type),
                                            Location = e.Location,
                                            ProvinvceCode = e.Province.Code,
                                            ProvinvceName = e.Province.Name,
                                            Address = e.Address,
                                            Latitude = e.Latitude,
                                            Longitude = e.Longitude,
                                            Website = e.Website,
                                            Facebook = e.Facebook,
                                            Phone = e.Phone,
                                            Email = e.Email,
                                            CanExportTicket = e.CanExportTicket,
                                            CanExportRequestRecipt = e.CanExportRequestRecipt,
                                            IsHighlight = e.IsHighlight,
                                            OverviewContent = e.OverviewContent,
                                            ContentType = e.ContentType,
                                            TicketPurchasePolicy = e.TicketPurchasePolicy,
                                            PolicyFileType = e.TicketPurchasePolicy != null ? Path.GetExtension(e.TicketPurchasePolicy).TrimStart('.') : null,
                                            InterestedPeople = e.InterestedPeople.Count() + EvtOrderMoreConst.GIA_LAP_SO_NGUOI_QUAN_TAM,
                                            CanBuy = e.EventDetails.SelectMany(ed => ed.Tickets)
                                                .Where(t => t.Deleted == YesNo.NO && t.EventDetail.Status == EventDetailStatus.KICH_HOAT && t.EventDetail.Deleted == YesNo.NO)
                                                .Sum(t => t.Quantity) -
                                                e.EventDetails.SelectMany(ed => ed.Orders)
                                                .Where(o => (o.Status == EvtOrderStatus.HOP_LE || o.Status == EvtOrderStatus.CHO_THANH_TOAN) 
                                                                && o.EventDetail.Status == EventDetailStatus.KICH_HOAT 
                                                                && o.EventDetail.Deleted != YesNo.NO)
                                                .SelectMany(o => o.OrderDetails)
                                                .Sum(od => od.Quantity),
                                            Tickets = e.EventDetails.SelectMany(ed => ed.Tickets.Where(e => e.Status == EvtTicketStatus.KICH_HOAT)).Where(t => t.Deleted == YesNo.NO && t.IsShowApp == YesNo.YES
                                                               && t.EventDetail.Deleted == YesNo.NO
                                                               && t.EventDetail.Status == EventDetailStatus.KICH_HOAT)
                                                           .OrderBy(t => t.StartSellDate)
                                                           .Select(t => new AppEvtTicketDto
                                                           {
                                                               Id = t.Id,
                                                               EventDetailId = t.EventDetailId,
                                                               Name = t.Name,
                                                               IsFree = t.IsFree,
                                                               Price = t.Price,
                                                               Quantity = t.Quantity,
                                                               MinBuy = t.MinBuy,
                                                               MaxBuy = t.MaxBuy,
                                                               StartSellDate = t.StartSellDate,
                                                               EndSellDate = t.EndSellDate,
                                                               Description = t.Description,
                                                               UrlImages = t.TicketMedias.Select(tm => tm.UrlImage),
                                                               RemainingTickets = t.Quantity - (t.OrderDetails.Where(od => od.Order.Status == EvtOrderStatus.HOP_LE
                                                                               || (od.Order.Status == EvtOrderStatus.CHO_THANH_TOAN
                                                                                   && od.Order.ExpiredTime >= DateTime.Now)).Sum(t => t.Quantity)),
                                                               IsShowRemaingTicketApp = t.EventDetail.IsShowRemaingTicketApp
                                                           }),
                                            EvtEventDescriptionMedias = _mapper.Map<IEnumerable<AppEvtEventDescriptionMediaDto>>(e.EventDescriptionMedias)
                                        })
                                        .FirstOrDefault();
            query.Images = _dbContext.EvtInterestedPeople
                                        .Include(e => e.Investor)
                                        .Where(ip => ip.EventId == id)
                                        .Select(ip => ip.Investor.AvatarImageUrl).Take(5);

            result = query;
            return result;
        }

        /// <summary>
        /// event liên quan
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<AppRelatedEventsDto> FindRelatedEventsById(AppFilterEventDto input)
        {
            _logger.LogInformation($"{nameof(FindRelatedEventsById)} : input = {JsonSerializer.Serialize(input)}");

            int? investorId = null;
            try
            {
                investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            }
            catch
            {
                investorId = null;
            }
            //List danh sách trading của investor
            var listInvestorTradingIds = _logicInvestorTradingSharedServices.FindListInvestorTradingProviderForApp(investorId);
            //Danh sách trading của sale
            var listSaleTradingIds = _logicInvestorTradingSharedServices.FindListSaleTradingProviderForApp(investorId);

            var evtEvent = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == input.EventId && e.Deleted == YesNo.NO
                                                 && ((e.Viewing == EvtEventViewer.INVESTOR_TRADED && listInvestorTradingIds.Contains(e.TradingProviderId))
                                                        || (e.Viewing == EvtEventViewer.INVESTOR_NOT_TRADED && listInvestorTradingIds.Count() == 0)
                                                        || (e.Viewing == EvtEventViewer.SALE && listSaleTradingIds.Contains(e.TradingProviderId))
                                                        || ((e.Viewing == EvtEventViewer.ALL && listInvestorTradingIds.Count() == 0)
                                                        || (e.Viewing == EvtEventViewer.ALL && (listInvestorTradingIds.Contains(e.TradingProviderId) || listSaleTradingIds.Contains(e.TradingProviderId))))
                                                       )).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);

            var result = new PagingResult<AppRelatedEventsDto>();

            var query = _dbContext.EvtEvents.Include(e => e.Province)
                         .Include(e => e.EventDetails).ThenInclude(ed => ed.Tickets)
                         .Include(e => e.EventMedias)
                         .Where(e => e.Deleted == YesNo.NO && e.IsShowApp == YesNo.YES 
                                        && e.Id != input.EventId
                                          && ((e.Viewing == EvtEventViewer.INVESTOR_TRADED && listInvestorTradingIds.Contains(e.TradingProviderId))
                                                        || (e.Viewing == EvtEventViewer.INVESTOR_NOT_TRADED && listInvestorTradingIds.Count() == 0)
                                                        || (e.Viewing == EvtEventViewer.SALE && listSaleTradingIds.Contains(e.TradingProviderId))
                                                        || ((e.Viewing == EvtEventViewer.ALL && listInvestorTradingIds.Count() == 0)
                                                        || (e.Viewing == EvtEventViewer.ALL && (listInvestorTradingIds.Contains(e.TradingProviderId) || listSaleTradingIds.Contains(e.TradingProviderId))))
                                                       )
                                        && e.EventDetails.Any(ed => ed.EndDate >= DateTime.Now 
                                                                && ed.Status == EventDetailStatus.KICH_HOAT 
                                                                && ed.Deleted == YesNo.NO))
                         .Select(e => new
                         {
                             Event = e,
                             MinStartDate = e.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT)
                                 .Select(ed => (DateTime?)ed.StartDate)
                                 .Min(),
                             MaxEndDate = e.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT)
                                 .Select(ed => (DateTime?)ed.EndDate)
                                 .Max(),
                             UrlImage = e.EventMedias.Where(em => em.Deleted == YesNo.NO && em.UrlImage != null)
                                 .Select(em => em.UrlImage)
                                 .FirstOrDefault(),
                             Price = e.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT && ed.Tickets != null && ed.Tickets.Any(ti => ti.Deleted == YesNo.NO))
                                .SelectMany(ed => ed.Tickets.Where(ti => ti.Deleted == YesNo.NO).Select(ti => (decimal?)ti.Price)),
                         })
                         .Select(e => new AppRelatedEventsDto
                         {
                             Id = e.Event.Id,
                             StartDate = e.MinStartDate,
                             EndDate = e.MaxEndDate,
                             Name = e.Event.Name,
                             UrlImage = e.UrlImage,
                             ProvinceCode = e.Event.ProvinceCode,
                             ProvinceName = e.Event.Province.Name,
                             MinPrice = e.Price.Min() ?? 0,
                             MaxPrice = e.Price.Max() ?? 0,
                             IsFree = e.Event.EventDetails.Where(ed => ed.Status == EventDetailStatus.KICH_HOAT && ed.Deleted == YesNo.NO).All(e => e.Tickets.All(t => t.IsFree))
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

        /// <summary>
        /// Danh sách sự kiện nổi bật đang và sắp diễn ra
        /// </summary>
        /// <param name="isSaleView"></param>
        /// <returns></returns>
        public AppEvtEventDto AppFindHighlightEvent(bool isSaleView)
        {
            int? investorId = null;
            try
            {
                investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            }
            catch
            {
                investorId = null;
            }

            _logger.LogInformation($"{nameof(AppFindHighlightEvent)}: investorId = {investorId}");

            var result = new AppEvtEventDto();

            //List danh sách trading của investor
            var listInvestorTradingIds = _logicInvestorTradingSharedServices.FindListInvestorTradingProviderForApp(investorId);
            //Danh sách trading của sale
            var listSaleTradingIds = _logicInvestorTradingSharedServices.FindListSaleTradingProviderForApp(investorId);

            var highlightEvent = _dbContext.EvtEvents.Include(e => e.EventDetails).ThenInclude(e => e.Tickets)
                                                     .Include(e => e.EventMedias).ThenInclude(e => e.EventMediaDetails)
                                                     .Include(e => e.Province)
                                                     .Where(e => e.Deleted == YesNo.NO
                                                     && e.IsCheck == YesNo.YES
                                                     && e.IsShowApp == YesNo.YES
                                                     && (e.IsHighlight == true)
                                                     && (e.Status == EventStatus.DANG_MO_BAN)
                                                     && (e.EventDetails.Any(e => e.EndDate >= DateTime.Now && e.Deleted == YesNo.NO && e.Status == EventDetailStatus.KICH_HOAT))
                                                     && ((e.Viewing == EvtEventViewer.INVESTOR_TRADED && listInvestorTradingIds.Contains(e.TradingProviderId))
                                                        || (e.Viewing == EvtEventViewer.INVESTOR_NOT_TRADED && listInvestorTradingIds.Count() == 0)
                                                        || (e.Viewing == EvtEventViewer.SALE && listSaleTradingIds.Contains(e.TradingProviderId))
                                                        || ((e.Viewing == EvtEventViewer.ALL && listInvestorTradingIds.Count() == 0)
                                                        || (e.Viewing == EvtEventViewer.ALL && (listInvestorTradingIds.Contains(e.TradingProviderId) || listSaleTradingIds.Contains(e.TradingProviderId))))
                                                       ))
                                                     .Select(e => new
                                                     {
                                                         evt = e,
                                                         ticketPrice = e.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT && ed.Tickets != null && ed.Tickets.Any(ti => ti.Deleted == YesNo.NO))
                                                                                        .SelectMany(ed => ed.Tickets.Where(ti => ti.Deleted == YesNo.NO).Select(ti => (decimal?)ti.Price)),
                                                     })
                                                     .Select(e => new AppViewEventDto
                                                     {
                                                         Id = e.evt.Id,
                                                         TradingProviderId = e.evt.TradingProviderId,
                                                         StartDate = e.evt.EventDetails.Where(ed => ed.Status == EventDetailStatus.KICH_HOAT && ed.Deleted == YesNo.NO && ed.StartDate != null && ed.StartDate >= DateTime.Now).OrderBy(ed => ed.StartDate).Select(e => e.StartDate).FirstOrDefault()
                                                                ?? e.evt.EventDetails.Where(ed => ed.Status == EventDetailStatus.KICH_HOAT && ed.Deleted == YesNo.NO && ed.StartDate != null).OrderBy(ed => ed.StartDate).Select(e => e.StartDate).LastOrDefault(),
                                                         EndDate = e.evt.EventDetails.Where(ed => ed.Status == EventDetailStatus.KICH_HOAT && ed.Deleted == YesNo.NO && ed.EndDate != null && ed.EndDate >= DateTime.Now).OrderByDescending(ed => ed.EndDate).Select(e => e.EndDate).FirstOrDefault(),
                                                         Name = e.evt.Name,
                                                         Organizator = e.evt.Organizator,
                                                         EventTypes = e.evt.EventTypes.Select(e => e.Type),
                                                         Location = e.evt.Location,
                                                         ProvinceCode = e.evt.ProvinceCode,
                                                         ProvinceName = e.evt.Province.Name,
                                                         Address = e.evt.Address,
                                                         Latitude = e.evt.Latitude,
                                                         Longitude = e.evt.Longitude,
                                                         BannerImageUrl = e.evt.EventMedias.Where(e => e.Location == EvtMediaLocation.BANNER_EVENT).Select(e => e.UrlImage).FirstOrDefault(),
                                                         AvatarImageUrl = e.evt.EventMedias.Where(e => e.Location == EvtMediaLocation.AVATAR_EVENT).Select(e => e.UrlImage).FirstOrDefault(),
                                                         MinTicketPrice = e.ticketPrice.Min() ?? 0,
                                                         MaxTicketPrice = e.ticketPrice.Max() ?? 0,
                                                         IsFree = e.evt.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT).All(e => e.Tickets.All(e => e.IsFree))
                                                     });

            var eventQuery = _dbContext.EvtEvents.Include(e => e.EventDetails)
                                                .Include(e => e.EventTypes)
                                                .Where(e => e.Deleted == YesNo.NO
                                                && e.IsCheck == YesNo.YES
                                                && e.IsShowApp == YesNo.YES
                                                && (e.Status == EventStatus.DANG_MO_BAN)
                                                && ((e.Viewing == EvtEventViewer.INVESTOR_TRADED && listInvestorTradingIds.Contains(e.TradingProviderId))
                                                        || (e.Viewing == EvtEventViewer.INVESTOR_NOT_TRADED && listInvestorTradingIds.Count() == 0)
                                                        || (e.Viewing == EvtEventViewer.SALE && listSaleTradingIds.Contains(e.TradingProviderId))
                                                        || e.Viewing == EvtEventViewer.ALL)
                                                && (e.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT).Select(e => e.EndDate).Any(e => e >= DateTime.Now)))
                                                .Select(e => new
                                                {
                                                    e.EventTypes
                                                });
            var eventTypeQuery = _dbContext.EvtEventTypes.Include(e => e.Event).ThenInclude(e => e.EventDetails)
                                                    .Where(e => e.Event.Deleted == YesNo.NO
                                                    && e.Event.IsCheck == YesNo.YES
                                                    && e.Event.IsShowApp == YesNo.YES
                                                    && (e.Event.Status == EventStatus.DANG_MO_BAN)
                                                    && ((e.Event.Viewing == EvtEventViewer.INVESTOR_TRADED && listInvestorTradingIds.Contains(e.Event.TradingProviderId))
                                                        || (e.Event.Viewing == EvtEventViewer.INVESTOR_NOT_TRADED && listInvestorTradingIds.Count() == 0)
                                                        || (e.Event.Viewing == EvtEventViewer.SALE && listSaleTradingIds.Contains(e.Event   .TradingProviderId))
                                                        || e.Event.Viewing == EvtEventViewer.ALL)
                                                    && (e.Event.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT).Select(e => e.EndDate).Any(e => e >= DateTime.Now)))
                                                    .Select(e => new AppCountEvent
                                                    {
                                                        EventType = e.Type,
                                                    })
                                                    .Distinct()
                                                    .Select(e => new AppCountEvent
                                                    {
                                                        EventType = e.EventType,
                                                        TotalEvent = eventQuery.Where(eq => eq.EventTypes.Any(eqq => eqq.Type == e.EventType)).Count()
                                                    }).OrderByDescending(e => e.TotalEvent);

            var numberEventTypes = new List<AppCountEvent>();

            result.Events = highlightEvent;
            result.NumberEventTypes = eventTypeQuery;
            return result;
        }

        /// <summary>
        /// Danh sách sự kiện đang và sắp diễn ra
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<AppViewEventDto> AppFindEvent(AppFilterEvtEventDto input)
        {
            int? investorId = null;
            try
            {
                investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            }
            catch
            {
                investorId = null;
            }

            _logger.LogInformation($"{nameof(AppFindEvent)}: input = {JsonSerializer.Serialize(input)}, investorId = {investorId}");

            var result = new PagingResult<AppViewEventDto>();

            //List danh sách trading của investor
            var listInvestorTradingIds = _logicInvestorTradingSharedServices.FindListInvestorTradingProviderForApp(investorId);
            //Danh sách trading của sale
            var listSaleTradingIds = _logicInvestorTradingSharedServices.FindListSaleTradingProviderForApp(investorId);

            var events = _dbContext.EvtEvents.Include(e => e.EventDetails).ThenInclude(ed => ed.Tickets)
                                                     .Include(e => e.EventMedias).ThenInclude(em => em.EventMediaDetails)
                                                     .Where(e => e.IsShowApp == YesNo.YES
                                                     && e.Deleted == YesNo.NO
                                                     && (e.IsCheck == YesNo.YES)
                                                     && (e.Status == EventStatus.DANG_MO_BAN)
                                                     && (e.EventDetails.Where(e => e.Status == EventDetailStatus.KICH_HOAT && e.Deleted == YesNo.NO).Select(e => e.EndDate).Any(e => e >= DateTime.Now))
                                                     && (input.EventType == null || e.EventTypes.Select(e => e.Type).Contains(input.EventType.Value))
                                                     && ((e.Viewing == EvtEventViewer.INVESTOR_TRADED && listInvestorTradingIds.Contains(e.TradingProviderId))
                                                        || (e.Viewing == EvtEventViewer.INVESTOR_NOT_TRADED && listInvestorTradingIds.Count() == 0)
                                                        || (e.Viewing == EvtEventViewer.SALE && listSaleTradingIds.Contains(e.TradingProviderId))
                                                        || ((e.Viewing == EvtEventViewer.ALL && listInvestorTradingIds.Count() == 0)
                                                        || (e.Viewing == EvtEventViewer.ALL && (listInvestorTradingIds.Contains(e.TradingProviderId) || listSaleTradingIds.Contains(e.TradingProviderId))))
                                                       ))
                                                    .Select(e => new
                                                    {
                                                        evt = e,
                                                        ticketPrice = e.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT && ed.Tickets != null && ed.Tickets.Any(ti => ti.Deleted == YesNo.NO))
                                                                                    .SelectMany(ed => ed.Tickets.Where(ti => ti.Deleted == YesNo.NO).Select(ti => (decimal?)ti.Price))
                                                    })
                                                     .Select(e => new AppViewEventDto
                                                     {
                                                         Id = e.evt.Id,
                                                         TradingProviderId = e.evt.TradingProviderId,
                                                         StartDate = e.evt.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT && ed.StartDate != null && ed.StartDate >= DateTime.Now).OrderBy(ed => ed.StartDate).Select(e => e.StartDate).FirstOrDefault()
                                                                ?? e.evt.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT && ed.StartDate != null).OrderBy(ed => ed.StartDate).Select(e => e.StartDate).LastOrDefault(),
                                                         EndDate = e.evt.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT && ed.EndDate != null && ed.EndDate >= DateTime.Now).OrderByDescending(ed => ed.EndDate).Select(e => e.EndDate).FirstOrDefault(),
                                                         Name = e.evt.Name,
                                                         Organizator = e.evt.Organizator,
                                                         EventTypes = e.evt.EventTypes.Select(e => e.Type),
                                                         Location = e.evt.Location,
                                                         ProvinceCode = e.evt.ProvinceCode,
                                                         ProvinceName = e.evt.Province.Name,
                                                         Address = e.evt.Address,
                                                         Latitude = e.evt.Latitude,
                                                         Longitude = e.evt.Longitude,
                                                         BannerImageUrl = e.evt.EventMedias.Where(e => e.Location == EvtMediaLocation.BANNER_EVENT).Select(e => e.UrlImage).FirstOrDefault(),
                                                         AvatarImageUrl = e.evt.EventMedias.Where(e => e.Location == EvtMediaLocation.AVATAR_EVENT).Select(e => e.UrlImage).FirstOrDefault(),
                                                         MinTicketPrice = e.ticketPrice.Min() ?? 0,
                                                         MaxTicketPrice = e.ticketPrice.Max() ?? 0,
                                                         IsFree = e.evt.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT).All(e => e.Tickets.All(e => e.IsFree))
                                                     });

            var resultItems = events.OrderBy(e => e.StartDate).AsQueryable();

            result.TotalItems = events.Count();
            if (input.PageSize != -1)
            {
                resultItems = resultItems.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = resultItems;
            return result;
        }
    }
}
