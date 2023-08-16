using AutoMapper;
using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtEventDescriptionMedia;
using EPIC.EventEntites.Dto.EvtEventHistory;
using EPIC.EventEntites.Dto.EvtOrderTicketDetail;
using EPIC.EventEntites.Entites;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Implements
{
    public class EvtAppEventHistoryServices : IEvtAppEventHistoryServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<EvtAppEventHistoryServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly ILogicInvestorTradingSharedServices _logicInvestorTradingSharedServices;
        private readonly IEvtAppOrderService _evtAppOrderService;

        public EvtAppEventHistoryServices(
            EpicSchemaDbContext dbContext,
            ILogger<EvtAppEventHistoryServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper,
            ILogicInvestorTradingSharedServices logicInvestorTradingSharedServices,
            IEvtAppOrderService evtAppOrderService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _httpContext = httpContext;
            _mapper = mapper;
            _logicInvestorTradingSharedServices = logicInvestorTradingSharedServices;
            _evtAppOrderService = evtAppOrderService;
        }

        /// <summary>
        /// Danh sách lịch sử sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<AppViewEventHistoryDto> FindAll(FilterEvtEventHistoryDto input)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, investorId = {investorId}");

            var result = new PagingResult<AppViewEventHistoryDto>();

            var orders = _dbContext.EvtOrders.Include(o => o.EventDetail).ThenInclude(o => o.Event)
                                             .ThenInclude(o => o.EventMedias.Where(e => e.Deleted == YesNo.NO))
                                             .Include(o => o.OrderPayments)
                                             .Where(e => e.EventDetail.Event.IsShowApp == YesNo.YES
                                                 && (e.EventDetail.Event.IsCheck == YesNo.YES)
                                                 && (e.EventDetail.Event.Status == EventStatus.DANG_MO_BAN)
                                                 && (input.Status == null || e.Status == input.Status)
                                                 && (e.Status != EvtOrderStatus.DA_XOA)
                                                 && ((input.IsTookPlace && e.EventDetail.EndDate < DateTime.Now)
                                                 || (!input.IsTookPlace && e.EventDetail.EndDate >= DateTime.Now))
                                                 && e.InvestorId == investorId
                                                 && (e.ExpiredTime == null || e.ExpiredTime >= DateTime.Now)
                                                 && e.Deleted == YesNo.NO)
                                             .Select(e => new
                                             {
                                                 order = e,
                                                 ticketPrice = e.EventDetail.Tickets.Where(ti => ti.Deleted == YesNo.NO).Select(ti => (decimal?)ti.Price)
                                             })
                                             .Select(e => new AppViewEventHistoryDto
                                             {
                                                 Id = e.order.EventDetail.Event.Id,
                                                 TradingProviderId = e.order.EventDetail.Event.TradingProviderId,
                                                 StartDate = e.order.EventDetail.StartDate,
                                                 EndDate = e.order.EventDetail.EndDate,
                                                 Name = e.order.EventDetail.Event.Name,
                                                 Organizator = e.order.EventDetail.Event.Organizator,
                                                 EventTypes = e.order.EventDetail.Event.EventTypes.Select(e => e.Type),
                                                 Location = e.order.EventDetail.Event.Location,
                                                 ProvinceCode = e.order.EventDetail.Event.ProvinceCode,
                                                 ProvinceName = e.order.EventDetail.Event.Province.Name,
                                                 Address = e.order.EventDetail.Event.Address,
                                                 Latitude = e.order.EventDetail.Event.Latitude,
                                                 Longitude = e.order.EventDetail.Event.Longitude,
                                                 BannerImageUrl = e.order.EventDetail.Event.EventMedias.Where(e => e.Location == EvtMediaLocation.BANNER_EVENT).Select(e => e.UrlImage).FirstOrDefault(),
                                                 AvatarImageUrl = e.order.EventDetail.Event.EventMedias.Where(e => e.Location == EvtMediaLocation.AVATAR_EVENT).Select(e => e.UrlImage).FirstOrDefault(),
                                                 MinTicketPrice = e.ticketPrice.Min() ?? 0,
                                                 MaxTicketPrice = e.ticketPrice.Max() ?? 0,
                                                 IsFree = e.order.IsFree,
                                                 Status = e.order.Status,
                                                 OrderId = e.order.Id,
                                                 ExpiredTime = e.order.ExpiredTime,
                                                 //dùng cho sắp xếp thứ tự lệnh hiển thị trên app
                                                 SortDate = e.order.ExpiredTime <= DateTime.Now ? e.order.ModifiedDate : e.order.CreatedDate,
                                                 Quantity = e.order.OrderDetails.Sum(o => o.Quantity),
                                             });

            result.TotalItems = orders.Count();
            orders = orders.OrderByDescending(o => o.SortDate.HasValue).ThenByDescending(o => o.SortDate);

            if (input.PageSize != -1)
            {
                orders = orders.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = orders;
            return result;
        }

        /// <summary>
        /// Xem chi tiết lịch sử sự kiện
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public AppViewEventHistoryDetailDto FindByOrderId(int orderId)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(FindByOrderId)}: orderId = {orderId}, investorId = {investorId}");

            var order = _dbContext.EvtOrders.Include(o => o.EventDetail).ThenInclude(o => o.Event).Include(o => o.OrderDetails).ThenInclude(e => e.Ticket)
                                                    .Select(e => new AppViewEventHistoryDetailDto
                                                    {
                                                        OrderId = e.Id,
                                                        TicketPurchasePolicy = e.EventDetail.Event.TicketPurchasePolicy,
                                                        PolicyFileType = e.EventDetail.Event.TicketPurchasePolicy != null ? Path.GetExtension(e.EventDetail.Event.TicketPurchasePolicy).TrimStart('.') : null,
                                                        InvestorId = e.InvestorId,
                                                        ContractCode = e.ContractCodeGen ?? e.ContractCode,
                                                        ContractCodeGen = e.ContractCodeGen,
                                                        StartDate = e.EventDetail.StartDate,
                                                        EndDate = e.EventDetail.EndDate,
                                                        Name = e.EventDetail.Event.Name,
                                                        Location = e.EventDetail.Event.Location,
                                                        ProvinceCode = e.EventDetail.Event.ProvinceCode,
                                                        ProvinceName = e.EventDetail.Event.Province.Name,
                                                        Address = e.EventDetail.Event.Address,
                                                        Latitude = e.EventDetail.Event.Latitude,
                                                        Longitude = e.EventDetail.Event.Longitude,
                                                        Status = e.Status,
                                                        Deleted = e.Deleted,
                                                        CreatedDate = e.CreatedDate,
                                                        TotalTicket = e.OrderDetails.Sum(o => o.Quantity),
                                                        CheckIn = e.EventDetail.Tickets.SelectMany(o => o.OrderTicketDetails).Min(o => o.CheckIn),
                                                        CheckOut = e.EventDetail.Tickets.SelectMany(o => o.OrderTicketDetails).Max(o => o.CheckOut),
                                                        OverviewContent = e.EventDetail.Event.OverviewContent,
                                                        ContentType = e.EventDetail.Event.ContentType,
                                                        Email = e.EventDetail.Event.Email,
                                                        Facebook = e.EventDetail.Event.Facebook,
                                                        Phone = e.EventDetail.Event.Phone,
                                                        Website = e.EventDetail.Event.Website,
                                                        IsRequestReceiveRecipt = e.IsRequestReceiveRecipt,
                                                        IsReceiveHardTicket = e.IsReceiveHardTicket,
                                                        IsFree = e.OrderDetails.All(e => e.Ticket.IsFree),
                                                        IsLock = e.IsLock,
                                                        Tickets = e.OrderDetails.Select(od => new AppEvtTicketDto
                                                        {
                                                            Id = od.Ticket.Id,
                                                            Name = od.Ticket.Name,
                                                            Quantity = od.Quantity,
                                                            Price = od.Price,
                                                            EventDetailId = od.Order.EventDetailId,
                                                            MinBuy = od.Ticket.MinBuy,
                                                            MaxBuy = od.Ticket.MaxBuy,
                                                            StartSellDate = od.Ticket.StartSellDate,
                                                            EndSellDate = od.Ticket.EndSellDate,
                                                            IsFree = od.Ticket.IsFree,
                                                            Description = od.Ticket.Description,
                                                            OrderDetailId = od.Id
                                                        }),
                                                        EvtEventDescriptionMedias = _mapper.Map<IEnumerable<AppEvtEventDescriptionMediaDto>>(e.EventDetail.Event.EventDescriptionMedias),
                                                        ExpiredTime = e.ExpiredTime
                                                    }).FirstOrDefault(e => e.OrderId == orderId
                                                        && e.InvestorId == investorId
                                                        && e.Deleted == YesNo.NO);
            return order;
        }

        /// <summary>
        /// Xem thông tin vé trong chi tiết lịch sử sự kiện
        /// </summary>
        /// <param name="orderDetailId"></param>
        /// <returns></returns>
        public IEnumerable<AppViewOrderTicketDetailDto> FindByOrderDetailId(int orderDetailId)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)}: orderDetailId = {orderDetailId}, investorId = {investorId}");

            var ticket = _dbContext.EvtOrderTicketDetails.Include(o => o.OrderDetail).ThenInclude(o => o.Order).ThenInclude(o => o.TradingProvider).ThenInclude(t => t.BusinessCustomer)
                                                    .Where(e => e.OrderDetail.Order.InvestorId == investorId  && e.OrderDetailId == orderDetailId)
                                                    .OrderBy(e => e.Id)
                                                    .Select(e => new AppViewOrderTicketDetailDto
                                                    {
                                                        Id = e.Id,
                                                        OrderId = e.OrderDetail.Order.Id,
                                                        OrderDetailId = e.OrderDetail.Id,
                                                        TicketId = e.TicketId,
                                                        TicketCode = e.TicketCode,
                                                        StartDate = e.Ticket.EventDetail.StartDate,
                                                        EndDate = e.Ticket.EventDetail.EndDate,
                                                        CheckIn = e.CheckIn,
                                                        CheckOut = e.CheckOut,
                                                        Status = e.Status,
                                                        Description = e.Ticket.Description,
                                                        AvatarTradingImageUrl = e.OrderDetail.Order.TradingProvider.BusinessCustomer.AvatarImageUrl,
                                                        TicketFilledUrl = e.TicketFilledUrl
                                                    });
            return ticket;
        }

        public async Task<IEnumerable<AppTradingBankAccountDto>> FindTradingBankAccountOfEvent(int orderId)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(FindTradingBankAccountOfEvent)}: orderId = {orderId}, investorId = {investorId}");
            IEnumerable<AppTradingBankAccountDto> result = null;
            var orderFind = _dbContext.EvtOrders.Include(o => o.EventDetail).ThenInclude(o => o.Event).FirstOrDefault(o => o.Id == orderId && o.InvestorId == investorId && o.Deleted == YesNo.NO);
            decimal totalMoney = _dbContext.EvtOrderDetails
                    .Where(od => od.OrderId == orderFind.Id)
                    .Sum(od => od.Quantity * od.Price);
            if (orderFind != null)
            {
                result = await _evtAppOrderService.TradingBankAccountOfEvent(orderFind.Id, orderFind.EventDetail.Event.Id, orderFind.ContractCodeGen ?? orderFind.ContractCode, totalMoney);
            }

            return result;
        }
    }
}
