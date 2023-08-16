using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Sale;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtHistoryUpdate;
using EPIC.EventEntites.Dto.EvtOrder;
using EPIC.EventEntites.Dto.EvtOrderDetail;
using EPIC.EventEntites.Entites;
using EPIC.EventRepositories;
using EPIC.InvestEntities.DataEntities;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerHistory;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.EventDomain.Implements
{
    public class EvtOrderValidService : IEvtOrderValidService
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EvtOrderValidService> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly EvtOrderEFRepository _evtOrderEFRepository;
        private readonly EvtOrderDetailEFRepository _evtOrderDetailEFRepository;
        private readonly EvtOrderTicketDetailEFRepository _evtOrderTicketDetailEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly SaleEFRepository _saleEFRepository;
        private readonly IEvtOrderCommonService _evtOrderCommonService;
        private readonly EvtHistoryUpdateEFRepository _evtHistoryUpdateEFRepository;

        public EvtOrderValidService(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<EvtOrderValidService> logger,
            IHttpContextAccessor httpContextAccessor,
            IEvtOrderCommonService evtOrderCommonService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _evtOrderEFRepository = new EvtOrderEFRepository(_dbContext, _logger);
            _evtOrderDetailEFRepository = new EvtOrderDetailEFRepository(_dbContext, _logger);
            _evtOrderTicketDetailEFRepository = new EvtOrderTicketDetailEFRepository(_dbContext, _logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _saleEFRepository = new SaleEFRepository(dbContext, logger);
            _evtOrderCommonService = evtOrderCommonService;
            _evtHistoryUpdateEFRepository = new EvtHistoryUpdateEFRepository(_dbContext, _logger);
        }

        /// <summary>
        /// danh sach ve ban hop le
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<EvtOrderValidDto> FindAll(FilterEvtOrderValidDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            var query = _dbContext.EvtOrders.Include(e => e.EventDetail).ThenInclude(ed => ed.Event)
                                            .Include(e => e.OrderPayments)
                                            .Include(e => e.InvestorIdentification)
                                            .Include(e => e.OrderDetails)
                                            .Include(e => e.Investor)
                                            .Where(t => t.Deleted == YesNo.NO
                                                && t.EventDetail.Event.TradingProviderId == tradingProviderId
                                                && (t.Status == EvtOrderStatus.HOP_LE)
                                                && (input.IsLock == null || t.IsLock == input.IsLock.Value)
                                                && (input.EventId == null || t.EventDetail.EventId == input.EventId)
                                                && (input.Phone == null || t.Investor.Phone.ToLower().Contains(input.Phone.ToLower()))
                                                && (input.Orderer == null ||
                                                    (t.Source == SourceOrder.ONLINE && input.Orderer == SourceOrderFE.KHACH_HANG) ||
                                                    (t.Source == SourceOrder.OFFLINE && t.ReferralSaleId == null
                                                        && input.Orderer == SourceOrderFE.QUAN_TRI_VIEN) ||
                                                    (input.Orderer == SourceOrderFE.SALE))
                                                && (input.Keyword == null || t.Investor.Phone.ToLower().Contains(input.Keyword.ToLower())
                                                || t.InvestorIdentification.Fullname.ToLower().Contains(input.Keyword.ToLower()) || t.EventDetail.Event.Name.ToLower().Contains(input.Keyword.ToLower())
                                                || t.ContractCode.ToLower().Contains(input.Keyword.ToLower()) || t.ContractCodeGen.ToLower().Contains(input.Keyword.ToLower()))
                                                )
                                            .Select(t => new EvtOrderValidDto
                                            {
                                                Id = t.Id,
                                                Fullname = t.InvestorIdentification.Fullname,
                                                EventDetailId = t.EventDetailId,
                                                Quantity = t.OrderDetails.Sum(od => od.Quantity),
                                                ReferralSaleId = t.ReferralSaleId ?? null,
                                                TotalMoney = t.OrderDetails.Sum(od => (od.Quantity * od.Price)),
                                                InvestorId = t.InvestorId,
                                                InvestorIdenId = t.InvestorIdenId,
                                                ExpiredTime = t.ExpiredTime,
                                                IsReceiveHardTicket = t.IsReceiveHardTicket,
                                                IsRequestReceiveRecipt = t.IsRequestReceiveRecipt,
                                                Status = t.Status,
                                                IsLock = t.IsLock,
                                                Source = t.Source,
                                                OrderSource = t.Source == SourceOrder.ONLINE
                                                    ? SourceOrderFE.KHACH_HANG
                                                    : (t.Source == SourceOrder.OFFLINE && t.ReferralSaleId == null)
                                                        ? SourceOrderFE.QUAN_TRI_VIEN
                                                        : SourceOrderFE.SALE,
                                                CreatedDate = t.CreatedDate,
                                                Phone = t.Investor.Phone,
                                                ContractCode = t.ContractCodeGen ?? t.ContractCode,
                                                EventName = t.EventDetail.Event.Name,
                                                ApproveDate = t.OrderPayments.Where(op => op.Deleted == YesNo.NO 
                                                                                && op.Status == OrderPaymentStatus.DA_THANH_TOAN)
                                                                            .Select(op => op.ApproveDate)
                                                                            .Max(),
                                                OrderDetails = _mapper.Map<IEnumerable<EvtOrderDetailDto>>(t.OrderDetails)
                                            });
            var result = new PagingResult<EvtOrderValidDto>();

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
        /// khoa tam so lenh
        /// </summary>
        /// <param name="input"></param>
        public void OrderLock(EvtOrderLockDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(OrderLock)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            var order = _dbContext.EvtOrders.FirstOrDefault(o => o.Id == input.OrderId 
                        && o.Deleted == YesNo.NO 
                        && o.TradingProviderId == tradingProviderId
                        && o.Status == EvtOrderStatus.HOP_LE
                        && !o.IsLock)
                            .ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);

            var transaction = _dbContext.Database.BeginTransaction();
            // luu lai ly do
            _evtHistoryUpdateEFRepository.Entity.Add(new EvtHistoryUpdate
            {
                Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                RealTableId = order.Id,
                OldValue = "false",
                NewValue = "true",
                UpdateTable = EvtHistoryUpdateTables.EVT_ORDER,
                Action = ActionTypes.CAP_NHAT,
                FieldName = EvtFieldName.UPDATE_ORDER_IS_LOCK,
                ActionUpdateType = EvtActionUpdateTypes.UPDATE_ORDER_IS_LOCK,
                Summary = input.Summary,
                CreatedDate = DateTime.Now,
                UpdateReason = input.Reason,
            });

            order.IsLock = true;
            _dbContext.SaveChanges();

            _dbContext.EvtOrderTicketDetails.Include(o => o.OrderDetail)
                                            .Where(o => o.OrderDetail.OrderId == order.Id && o.Status == EvtOrderTicketStatus.CHUA_THAM_GIA)
                                            .ToList()
                                            .ForEach(ticket => ticket.Status = EvtOrderTicketStatus.TAM_KHOA);
            _dbContext.SaveChanges();

            transaction.Commit();
        }

        /// <summary>
        /// khoa tam ve cua khach hang
        /// </summary>
        /// <param name="input"></param>
        public void OrderTickLock(EvtOrderTickLockDto input)
        {
            _logger.LogInformation($"{nameof(OrderLock)}: input = {JsonSerializer.Serialize(input)}");

            var orderTicket = _dbContext.EvtOrderTicketDetails.FirstOrDefault(o => o.Id == input.OrderTickId
                                                                            && o.Status == EvtOrderTicketStatus.CHUA_THAM_GIA)
                                                            .ThrowIfNull(_dbContext, ErrorCode.EvtOrderTicketNotFound);
            // luu lai ly do
            _evtHistoryUpdateEFRepository.Entity.Add(new EvtHistoryUpdate
            {
                Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                RealTableId = input.OrderTickId,
                OldValue = orderTicket.Status.ToString(),
                NewValue = EvtOrderTicketStatus.TAM_KHOA.ToString(),
                UpdateTable = EvtHistoryUpdateTables.EVT_ORDER_TICKET,
                Action = ActionTypes.CAP_NHAT,
                FieldName = EvtFieldName.UPDATE_ORDER_TICKET_STATUS,
                ActionUpdateType = EvtActionUpdateTypes.UPDATE_ORDER_TICKET_STATUS,
                Summary = input.Summary,
                CreatedDate = DateTime.Now,
                UpdateReason = input.Reason,
            });

            orderTicket.Status = EvtOrderTicketStatus.TAM_KHOA;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// mo khoa ve cua lenh, xac nhan tham gia su kien, checkout, checkin su kien 
        /// </summary>
        /// <param name="orderTickId"></param>
        public void OrderTickChangeStatus(int orderTickId, int status = EvtOrderTicketStatus.CHUA_THAM_GIA)
        {
            _logger.LogInformation($"{nameof(OrderLock)}: orderTickId = {orderTickId}, status = {status}");

            var orderTicket = _dbContext.EvtOrderTicketDetails.FirstOrDefault(o => o.Id == orderTickId)
                .ThrowIfNull(_dbContext, ErrorCode.EvtOrderTicketNotFound);

            switch (status)
            {
                case EvtOrderTicketStatus.CHUA_THAM_GIA:
                    if (orderTicket.Status == EvtOrderTicketStatus.TAM_KHOA)
                    {
                        orderTicket.Status = EvtOrderTicketStatus.CHUA_THAM_GIA;
                    } 
                    else
                    {
                        _defErrorEFRepository.ThrowException(ErrorCode.EvtOrderTicketNotLock);
                    }
                    break;

                case EvtOrderTicketStatus.DA_THAM_GIA:
                    if (orderTicket.Status == EvtOrderTicketStatus.TAM_KHOA)
                    {
                        _defErrorEFRepository.ThrowException(ErrorCode.EvtOrderTicketLock);
                    }
                    else if (orderTicket.Status == EvtOrderTicketStatus.CHUA_THAM_GIA)
                    {
                        orderTicket.Status = EvtOrderTicketStatus.DA_THAM_GIA;
                        orderTicket.CheckInType = EvtOrderTicketTypes.THU_CONG;
                        orderTicket.CheckIn = DateTime.Now;
                    }
                    else if (orderTicket.Status == EvtOrderTicketStatus.DA_THAM_GIA)
                    {
                        orderTicket.CheckOutType = EvtOrderTicketTypes.THU_CONG;
                        orderTicket.CheckOut = DateTime.Now;
                    }
                    break;
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// thay doi ma gioi thieu
        /// </summary>
        /// <param name="input"></param>
        public void ChangeReferralCode(UpdateReferralCode input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeReferralCode)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            var order = _dbContext.EvtOrders.FirstOrDefault(o => o.Id == input.OrderId
                        && o.Deleted == YesNo.NO
                        && o.TradingProviderId == tradingProviderId
                        && o.Status == EvtOrderStatus.HOP_LE)
                            .ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);
            
            // Tìm kiếm thông tin sale nếu có mã giới thiệu
            if (string.IsNullOrWhiteSpace(input.SaleReferralCode))
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CoreSaleReferralCodeExistReferralCodeSaleOrder);
            };

            var findSale = _saleEFRepository.AppFindSaleOrderByReferralCode(input.SaleReferralCode, tradingProviderId);
            if (findSale == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CoreSaleNotFound);
            }

            order.ReferralSaleId = findSale.SaleId;

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// danh sach quan ly tham gia
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<EvtOrderTicketDto> FindAllOrderTicket(FilterEvtOrderTicketDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindAllOrderTicket)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            var query = _dbContext.EvtOrderTicketDetails
                         .Include(o => o.OrderDetail).ThenInclude(od => od.Order)
                                                    .ThenInclude(o => o.Investor)
                         .Include(o => o.OrderDetail).ThenInclude(od => od.Order)
                                                    .ThenInclude(o => o.EventDetail)
                                                    .ThenInclude(ed => ed.Event)
                         .Include(o => o.Ticket)
                         .Where(o => o.OrderDetail.Order.Deleted == YesNo.NO
                                && o.OrderDetail.Order.EventDetail.Event.TradingProviderId == tradingProviderId
                                && (input.EventIds == null || input.EventIds.Contains(o.OrderDetail.Order.EventDetail.Event.Id))
                                && (input.Status == null || input.Status.Contains(o.Status))
                                && (input.CheckInType == null || o.CheckInType == input.CheckInType)
                                && (input.TicketCode == null || o.TicketCode.ToLower().Contains(input.TicketCode.ToLower()))
                                && (input.ContractCode == null || o.OrderDetail.Order.ContractCode.ToLower().Contains(input.ContractCode.ToLower()))
                                && (input.Phone == null || o.OrderDetail.Order.Investor.Phone.ToLower().Contains(input.Phone.ToLower()))
                                && (input.Keyword == null || o.OrderDetail.Order.ContractCode.ToLower().Contains(input.Keyword.ToLower())
                                || o.OrderDetail.Order.EventDetail.Event.Name.ToLower().Contains(input.Keyword.ToLower())
                                || o.OrderDetail.Order.ContractCodeGen.ToLower().Contains(input.Keyword.ToLower())
                                || o.TicketCode.ToLower().Contains(input.Keyword.ToLower()))
                         )
                         .Select(o => new EvtOrderTicketDto
                         {
                             Id = o.Id,
                             ContractCode = o.OrderDetail.Order.ContractCodeGen ?? o.OrderDetail.Order.ContractCode,
                             EventName = o.OrderDetail.Order.EventDetail.Event.Name,
                             TicketId = o.TicketId,
                             OrderDetailId = o.OrderDetailId,
                             Name = o.Ticket.Name,
                             TicketCode = o.TicketCode,
                             CheckIn = o.OrderDetail.Order.EventDetail.StartDate,
                             CheckOut = o.OrderDetail.Order.EventDetail.EndDate,
                             CheckInReal = o.CheckIn,
                             CheckOutReal = o.CheckOut,
                             CheckInType = o.CheckInType,
                             Status = o.Status,
                             Phone = o.OrderDetail.Order.Investor.Phone,
                             TicketFilledUrl = o.TicketFilledUrl
                         });

            var result = new PagingResult<EvtOrderTicketDto>();

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
        /// Mở khóa lệnh
        /// </summary>
        /// <param name="id"></param>
        public void OrderUnLock(int id)
        {
            _logger.LogInformation($"{nameof(OrderUnLock)} : id = {id}");
            var transaction = _dbContext.Database.BeginTransaction();
            var order = _dbContext.EvtOrders.FirstOrDefault(o => o.Id == id
                        && o.Deleted == YesNo.NO && o.IsLock).ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);
            order.IsLock = false;
            _dbContext.SaveChanges();

            _dbContext.EvtOrderTicketDetails.Include(o => o.OrderDetail)
                                            .Where(o => o.OrderDetail.OrderId == order.Id && o.Status == EvtOrderTicketStatus.TAM_KHOA)
                                            .ToList()
                                            .ForEach(ticket => ticket.Status = EvtOrderTicketStatus.CHUA_THAM_GIA);
            _dbContext.SaveChanges();
            transaction.Commit();
        }
    }
}

