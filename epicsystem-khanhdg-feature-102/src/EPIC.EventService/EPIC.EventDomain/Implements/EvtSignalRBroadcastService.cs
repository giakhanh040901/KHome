using EPIC.DataAccess.Base;
using EPIC.EventDomain.Interfaces;
using EPIC.EventDomain.SingalR.Hubs;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtOrderTicketDetail;
using EPIC.RealEstateRepositories;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.EventEntites.Dto.EvtOrder;
using EPIC.EventEntites.Dto.EvtOrderPayment;
using EPIC.Entities.DataEntities;

namespace EPIC.EventDomain.Implements
{
    public class EvtSignalRBroadcastService : IEvtSignalRBroadcastService
    {
        private readonly ILogger _logger;
        private readonly IHubContext<EventHub> _hub;
        private readonly EpicSchemaDbContext _dbContext;

        public EvtSignalRBroadcastService(
            ILogger<EvtSignalRBroadcastService> logger,
            IHubContext<EventHub> hub,
            EpicSchemaDbContext dbContext)
        {
            _logger = logger;
            _hub = hub;
            _dbContext = dbContext;
        }

        public async Task BroadcastEventQuantityTicket(int eventId)
        {
            var dataPush = _dbContext.EvtEvents.Include(e => e.EventDetails).ThenInclude(e => e.Tickets)
                                               .Include(e => e.EventDetails).ThenInclude(e => e.Orders).ThenInclude(o => o.OrderDetails).ThenInclude(od => od.OrderTicketDetails)
                                               .Include(e => e.EventTypes)
                                               .Select(e => new SignalREvtEventDto
                                               {
                                                   Id = e.Id,
                                                   TradingProviderId = e.TradingProviderId,
                                                   CreatedBy = e.CreatedBy,
                                                   CreatedDate = e.CreatedDate,
                                                   EventTypes = e.EventTypes.Select(e => e.Type),
                                                   Status = e.Status,
                                                   Name = e.Name,
                                                   Organizator = e.Organizator,
                                                   TicketQuantity = e.EventDetails.Sum(ed => ed.Tickets.Sum(t => t.Quantity)),
                                                   RegisterQuantity = e.EventDetails.Sum(ed => ed.Orders.Sum(o => o.OrderDetails.Sum(od => od.Quantity))),
                                                   ValidQuantity = e.EventDetails.Sum(ed => ed.Orders.Where(o => o.Status == EvtOrderStatus.HOP_LE).Sum(o => o.OrderDetails.Sum(od => od.Quantity))),
                                                   ParticipateQuantity = e.EventDetails.Sum(ed => ed.Orders.Sum(o => o.OrderDetails.Sum(od => od.OrderTicketDetails.Where(otd => otd.Status == EvtOrderTicketStatus.DA_THAM_GIA).Count()))),
                                                   RemainingTickets = e.EventDetails.Sum(ed => ed.Tickets.Sum(t => t.Quantity))
                                                                    - (e.EventDetails.Sum(e => e.Orders.Where(o => (o.Status == EvtOrderStatus.HOP_LE || o.Status == EvtOrderStatus.CHO_THANH_TOAN) && o.Deleted == YesNo.NO).Sum(o => o.OrderDetails.Sum(od => od.Quantity))))
                                               })
                                               .FirstOrDefault(e => e.Id == eventId);
            await _hub.Clients.Group(UserGroupNames.PrefixTradingProvider + dataPush.TradingProviderId)
                    .SendAsync(EvtSignalRMethods.UpdateEventTicket, dataPush);
        }

        public async Task BroadcastOrderTicketDetail(int orderTicketDetailId)
        {
            var dataPushOrderTicketDetail = _dbContext.EvtOrderTicketDetails.Include(otd => otd.Ticket).ThenInclude(t => t.EventDetail).ThenInclude(ed => ed.Event)
                                                        .Select(o => new SingalREvtOrderTicketDetailDto
                                                        {
                                                            Id = o.Id,
                                                            CheckIn = o.CheckIn,
                                                            CheckInType = o.CheckInType,
                                                            CheckOut = o.CheckOut,
                                                            CheckOutType = o.CheckOutType,
                                                            OrderDetailId = o.OrderDetailId,
                                                            Status = o.Status,
                                                            TicketCode = o.TicketCode,
                                                            TicketId = o.TicketId,
                                                            TradingProviderId = o.Ticket.EventDetail.Event.TradingProviderId
                                                        }).FirstOrDefault(e => e.Id == orderTicketDetailId);
            
            await _hub.Clients.Group(UserGroupNames.PrefixTradingProvider + dataPushOrderTicketDetail.TradingProviderId)
                    .SendAsync(EvtSignalRMethods.UpdateOrderTickeDetail, dataPushOrderTicketDetail);
        }

        public async Task BroadcastOrderExpiredTime(int orderId)
        {
            var dataPush = _dbContext.EvtOrders
                                        .Select(o => new SingalREvtOrderDto
                                        {
                                            Id = o.Id,
                                            InvestorId = o.InvestorId,
                                            Status = o.Status,
                                            ExpiredTime = o.ExpiredTime
                                        }).FirstOrDefault(o => o.Id == orderId);
            await _hub.Clients.Group(UserGroupNames.Investor + dataPush.InvestorId)
                    .SendAsync(EvtSignalRMethods.UpdateOrderExpiredTime, dataPush);
        }

        public async Task BroadcastOrderActive(int orderId)
        {
            var dataPush = _dbContext.EvtOrders
                                        .Select(o => new SingalREvtOrderActiveDto
                                        {
                                            Id = o.Id,
                                            InvestorId = o.InvestorId,
                                            TradingProviderId = o.TradingProviderId,
                                            Status = o.Status
                                        }).FirstOrDefault(o => o.Id == orderId);
            if (dataPush == null) {
                return;
            }
            await _hub.Clients.Group(UserGroupNames.PrefixTradingProvider + dataPush.TradingProviderId)
                    .SendAsync(EvtSignalRMethods.UpdateOrderStatus, dataPush);
            await _hub.Clients.Group(UserGroupNames.Investor + dataPush.InvestorId)
                    .SendAsync(EvtSignalRMethods.UpdateOrderStatus, dataPush);
        }

        public async Task BroadcastOrderPaymentActive(int orderPaymentId)
        {
            var dataPush = _dbContext.EvtOrderPayments
                                        .Select(o => new SingalREvtOrderOrderPaymentActiveDto
                                        {
                                            Id = o.Id,
                                            TradingProviderId = o.TradingProviderId,
                                            Status = o.Status
                                        }).FirstOrDefault(o => o.Id == orderPaymentId);
            if (dataPush == null)
            {
                return;
            }
            await _hub.Clients.Group(UserGroupNames.PrefixTradingProvider + dataPush.TradingProviderId)
                    .SendAsync(EvtSignalRMethods.UpdateOrderPaymentStatus, dataPush);
        }
    }
}
