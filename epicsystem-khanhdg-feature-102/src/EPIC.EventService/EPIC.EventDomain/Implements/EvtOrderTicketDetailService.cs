using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtTicket;
using EPIC.EventEntites.Entites;
using EPIC.EventRepositories;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.RealEstate;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Implements
{
    public class EvtOrderTicketDetailService : IEvtOrderTicketDetailService
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EvtOrderTicketDetailService> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly EvtOrderTicketDetailEFRepository _orderTicketDetailEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly EventNotificationServices _eventNotificationServices;
        private readonly IEvtSignalRBroadcastService _evtSignalRBroadcastService;

        public EvtOrderTicketDetailService(EpicSchemaDbContext dbContext,
            IMapper mapper,
            ILogger<EvtOrderTicketDetailService> logger,
            IHttpContextAccessor httpContextAccessor,
            EventNotificationServices eventNotificationServices,
            IEvtSignalRBroadcastService evtSignalRBroadcastService
        )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _httpContext = httpContextAccessor;
            _eventNotificationServices = eventNotificationServices;
            _evtSignalRBroadcastService = evtSignalRBroadcastService;
            _orderTicketDetailEFRepository = new EvtOrderTicketDetailEFRepository(_dbContext, _logger);
            _defErrorEFRepository = new DefErrorEFRepository(_dbContext);
        }

        public async Task<AppQrScanTicketDto> QRScanAdmin(string ticketCode)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(QRScanAdmin)} : input = {JsonSerializer.Serialize(ticketCode)}, investorId = {investorId}");

            var orderTicketDetail = _dbContext.EvtOrderTicketDetails.Include(otd => otd.OrderDetail).Include(e => e.Ticket).ThenInclude(e => e.EventDetail)
                    .FirstOrDefault(e => e.TicketCode.ToLower() == ticketCode.ToLower()).ThrowIfNull(_dbContext, ErrorCode.EvtTicketCodeQRNotValid);
            //Check order hợp lệ
            var order = _dbContext.EvtOrders.FirstOrDefault(e => e.Id == orderTicketDetail.OrderDetail.OrderId && e.Deleted == YesNo.NO && e.Status == EvtOrderStatus.HOP_LE).ThrowIfNull(_dbContext, ErrorCode.EvtOrderIsNotValid);

            //Check trạng thái khung giờ
            var eventDetailFind = _dbContext.EvtEventDetails.FirstOrDefault(e => e.Id == order.EventDetailId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventDetailNotFound);
            if (eventDetailFind.Status != EventDetailStatus.KICH_HOAT)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtEventDetailNotActive);
            }

            //Check khung giờ diễn ra
            if (eventDetailFind.StartDate > DateTime.Now)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtEventDetailNotStart);
            }
            if (eventDetailFind.EndDate < DateTime.Now)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtEventDetailIsNotValid);
            }

            //Check Quyền admin của sự kiện
            var admin = _dbContext.EvtAdminEvents.FirstOrDefault(ad => ad.InvestorId == investorId && ad.EventId == eventDetailFind.EventId).ThrowIfNull(_dbContext, ErrorCode.EvtOrderTicketCannotScanByPermission);

            if (orderTicketDetail.Status == EvtOrderTicketStatus.CHUA_THAM_GIA) 
            {
                orderTicketDetail.Status = EvtOrderTicketStatus.DA_THAM_GIA;
            }
            if (orderTicketDetail.CheckIn == null)
            {
                orderTicketDetail.CheckIn = DateTime.Now;
                orderTicketDetail.CheckInType = EvtOrderTicketTypes.TU_DONG;
                await _eventNotificationServices.SendNotifyJoinEvent(orderTicketDetail.Id);
            }
            else if(orderTicketDetail.CheckIn != null && orderTicketDetail.CheckOut == null)
            {
                orderTicketDetail.CheckOut = DateTime.Now;
                orderTicketDetail.CheckInType = EvtOrderTicketTypes.TU_DONG;
            }
            else if (orderTicketDetail.CheckIn != null && orderTicketDetail.CheckOut != null)
            {
                _orderTicketDetailEFRepository.ThrowException(ErrorCode.EvtOrderTicketScanError);
            }
            
            _dbContext.SaveChanges();

            await _evtSignalRBroadcastService.BroadcastOrderTicketDetail(orderTicketDetail.Id);

            await _evtSignalRBroadcastService.BroadcastEventQuantityTicket(orderTicketDetail.Ticket.EventDetail.EventId);

            return new AppQrScanTicketDto
            {
                Id = orderTicketDetail.Id,
                CheckIn = orderTicketDetail.CheckIn,
                CheckInType = orderTicketDetail.CheckInType,
                CheckOut = orderTicketDetail.CheckOut,
                CheckOutType = orderTicketDetail.CheckOutType,
                Status = orderTicketDetail.Status,
                TicketCode = orderTicketDetail.TicketCode 
            };
        }
    }
}
