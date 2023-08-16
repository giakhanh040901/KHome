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
using EPIC.Notification.Services;
using MySqlX.XDevAPI.Common;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace EPIC.EventDomain.Implements
{
    public class EvtOrderService : IEvtOrderService
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EvtOrderService> _logger;
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
        private readonly EventNotificationServices _eventNotificationServices;
        private readonly IEvtSignalRBroadcastService _evtSignalRBroadcastService;
        private readonly IEvtOrderTicketFillService _orderTicketFillService;
        private readonly IBackgroundJobClient _backgroundJobs;

        public EvtOrderService(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<EvtOrderService> logger,
            IHttpContextAccessor httpContextAccessor,
            IEvtOrderCommonService evtOrderCommonService,
            EventNotificationServices eventNotificationServices,
            IEvtSignalRBroadcastService evtSignalRBroadcastService,
            IEvtOrderTicketFillService orderTicketFillService,
            IBackgroundJobClient backgroundJobs
            )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _evtSignalRBroadcastService = evtSignalRBroadcastService;
            _evtOrderEFRepository = new EvtOrderEFRepository(_dbContext, _logger);
            _evtOrderDetailEFRepository = new EvtOrderDetailEFRepository(_dbContext, _logger);
            _evtOrderTicketDetailEFRepository = new EvtOrderTicketDetailEFRepository(_dbContext, _logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _saleEFRepository = new SaleEFRepository(dbContext, logger);
            _evtOrderCommonService = evtOrderCommonService;
            _evtHistoryUpdateEFRepository = new EvtHistoryUpdateEFRepository(_dbContext, _logger);
            _eventNotificationServices = eventNotificationServices;
            _orderTicketFillService = orderTicketFillService;
            _backgroundJobs = backgroundJobs;
        }

        /// <summary>
        /// thêm sổ lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<int> Add(CreateEvtOrderDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var ipAddress = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            _logger.LogInformation($"{nameof(Add)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            var investor = _dbContext.Investors.FirstOrDefault(e => e.InvestorId == input.InvestorId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.InvestorNotFound);
            if(input.ContractAddressId != null && input.IsReceiveHardTicket && !(_dbContext.InvestorContactAddresses.Where(c => c.Deleted == YesNo.NO && c.ContactAddressId == input.ContractAddressId && c.InvestorId == input.InvestorId).Any()))
            {
                _evtOrderEFRepository.ThrowException(ErrorCode.InvestorContractNotFound);
            }

            if (input.ContractAddressId == null && (input.IsReceiveHardTicket || input.IsRequestReceiveRecipt))
            {
                _evtOrderEFRepository.ThrowException(ErrorCode.EvtOrderContractAddressNotNull);
            }

            var investorIden = _dbContext.InvestorIdentifications.FirstOrDefault(e => e.InvestorId == input.InvestorId && e.Id == input.InvestorIdenId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.InvestorIdentificationNotFound);
            var eventDetail = _dbContext.EvtEventDetails.FirstOrDefault(e => e.Id == input.EventDetailId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventDetailNotFound);
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == eventDetail.EventId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);
            if (eventFind.Status != EventStatus.DANG_MO_BAN)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtEventIsNotActive);
            }
            if (input.IsReceiveHardTicket && !eventFind.CanExportTicket)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtOrderReceiveHardTicketNotFound);
            }
            if (input.IsRequestReceiveRecipt && !eventFind.CanExportRequestRecipt)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtOrderRequestReceiveReciptNotFound);
            }
            var transaction = _dbContext.Database.BeginTransaction();

            var orderId = (int)_evtOrderEFRepository.NextKey();

            var contractCode = _evtOrderCommonService.GenContractCode(new OrderContractCodeDto
            {
                OrderId = orderId,
                ConfigContractCodeId = eventFind.ConfigContractCodeId,
                BuyDate = DateTime.Now,
            });

            var insert = new EvtOrder
            {
                Id = orderId,
                Source = SourceOrder.OFFLINE,
                ContractCode = _evtOrderCommonService.ContractCode(orderId),
                ContractCodeGen = contractCode,
                TradingProviderId = tradingProviderId,
                EventDetailId = input.EventDetailId,
                InvestorId = input.InvestorId,
                ContractAddressId = input.ContractAddressId,
                InvestorIdenId = input.InvestorIdenId,
                ExpiredTime = eventDetail.PaymentWaittingTime != null ? DateTime.Now.AddSeconds((double)eventDetail.PaymentWaittingTime) : null,
                IsReceiveHardTicket = input.IsReceiveHardTicket,
                IsRequestReceiveRecipt = input.IsRequestReceiveRecipt,
                IpAddressCreated = ipAddress,
                DeliveryCode = _evtOrderEFRepository.FuncVerifyCodeGenerate()
            };

            // Tìm kiếm thông tin sale nếu có mã giới thiệu
            if (!string.IsNullOrWhiteSpace(input.SaleReferralCode))
            {
                var findSale = _saleEFRepository.AppFindSaleOrderByReferralCode(input.SaleReferralCode, tradingProviderId);
                if (findSale != null)
                {
                    insert.ReferralSaleId = findSale.SaleId;
                    insert.DepartmentId = findSale.DepartmentId;
                }
            }

            var result = _dbContext.EvtOrders.Add(insert);
            _evtHistoryUpdateEFRepository.Entity.Add(new EvtHistoryUpdate
            {
                Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                RealTableId = insert.Id,
                OldValue = null,
                NewValue = "Khởi tạo",
                FieldName = "Trạng thái",
                UpdateTable = EvtHistoryUpdateTables.EVT_ORDER,
                Action = ActionTypes.THEM_MOI,
                Summary = "Thêm mới sổ lệnh"
            });
            _dbContext.SaveChanges();

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int countFree = 0;
            if (input.OrderDetails != null)
            {
                foreach (var item in input.OrderDetails)
                {
                    var ticket = _dbContext.EvtTickets.FirstOrDefault(e => e.Id == item.TicketId
                                                                        && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtTicketNotFound);

                    if (!_evtOrderCommonService.CheckRemainingTickets(item.TicketId))
                    {
                        _defErrorEFRepository.ThrowException(ErrorCode.EvtTicketsSoldOut, ticket.Name);
                    };

                    if (ticket.IsFree)
                    {
                        countFree++;
                    };

                    if (countFree == input.OrderDetails.Count)
                    {
                        result.Entity.Status = EvtOrderStatus.HOP_LE;
                        result.Entity.ApproveDate = DateTime.Now;
                        result.Entity.ApproveBy = username;
                        if (result.Entity.IsReceiveHardTicket)
                        {
                            result.Entity.PendingDate = DateTime.Now;
                            result.Entity.PendingDateModifiedBy = username;
                            result.Entity.DeliveryStatus = EventDeliveryStatus.WAITING;
                        }

                        if(result.Entity.IsRequestReceiveRecipt)
                        {
                            result.Entity.DeliveryInvoiceStatus = EventDeliveryStatus.WAITING;
                            result.Entity.PendingInvoiceDate = DateTime.Now;
                            result.Entity.PendingInvoiceDateModifiedBy = username;
                        }
                      
                        _dbContext.SaveChanges();
                    }

                    if (ticket != null)
                    {
                        
                        var orderDetailId = (int)_evtOrderDetailEFRepository.NextKey();
                        _dbContext.EvtOrderDetails.Add(new EvtOrderDetail
                        {
                            Id = orderDetailId,
                            OrderId = result.Entity.Id,
                            TicketId = item.TicketId,
                            Quantity = item.Quantity,
                            Price = ticket.IsFree ? 0 : ticket.Price
                        });
                        _dbContext.SaveChanges();

                        //lay ra list TicketCode da random
                        List<string> listTicketCode = _evtOrderCommonService.ListTicketCodeGenerate(item.Quantity, EvtOrderMoreConst.DO_DAI_CHUOI_TICKET_CODE);
                        for (int i = 0; i < item.Quantity; i++)
                        {
                            _dbContext.EvtOrderTicketDetails.Add(new EvtOrderTicketDetail
                            {
                                Id = (int)_evtOrderTicketDetailEFRepository.NextKey(),
                                OrderDetailId = orderDetailId,
                                TicketId = item.TicketId,
                                Status = EvtOrderTicketStatus.CHUA_THAM_GIA,
                                TicketCode = listTicketCode[i],
                            });
                        }
                        _dbContext.SaveChanges();
                    }
                }
            }
            if (result.Entity.Status == EvtOrderStatus.HOP_LE)
            {
                await _eventNotificationServices.SendNotifyRegisterTicket(result.Entity.Id);
                result.Entity.TicketJobId = _backgroundJobs.Enqueue(() => _orderTicketFillService.FillOrderTicket(result.Entity.Id));
                await _evtSignalRBroadcastService.BroadcastEventQuantityTicket(eventFind.Id);
                if (input.IsRequestReceiveRecipt)
                {
                    await _eventNotificationServices.SendAdminNotifyReceiveRecipt(result.Entity.Id);
                }
                if (input.IsReceiveHardTicket)
                {
                    await _eventNotificationServices.SendAdminNotifyReceiveHardTicket(result.Entity.Id);
                }
            }
            _dbContext.SaveChanges();
            transaction.Commit();
            return result.Entity.Id;
        }

        /// <summary>
        /// xóa sổ lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            var tradingproviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Delete)} : id = {id}, tradingproviderId = {tradingproviderId}");
            var order = _dbContext.EvtOrders.Include(o => o.OrderPayments).Include(o => o.OrderDetails)
                                            .Include(e => e.EventDetail).ThenInclude(ed => ed.Event)
                                            .FirstOrDefault(e => e.Id == id && e.Deleted == YesNo.NO && e.TradingProviderId == tradingproviderId).ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);
            if (order.OrderPayments.Count > 0)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtOrderCanNotDeleteBecauseHavePayment);
            }
            _dbContext.EvtOrders.Remove(order);
            _evtHistoryUpdateEFRepository.Entity.Add(new EvtHistoryUpdate
            {
                Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                RealTableId = order.Id,
                UpdateTable = EvtHistoryUpdateTables.EVT_ORDER,
                Action = ActionTypes.XOA,
                Summary = "Xoá sổ lệnh"
            });
            _dbContext.SaveChanges();
            //Xử lý gọi hàm cập nhật realtime
            await _evtSignalRBroadcastService.BroadcastEventQuantityTicket(order.EventDetail.EventId);
        }

        /// <summary>
        /// Hủy sổ lệnh
        /// </summary>
        /// <param name="orderIds"></param>
        public void Cancel(List<int> orderIds)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Cancel)} : orderIds = {JsonSerializer.Serialize(orderIds)}, tradingProviderId = {tradingProviderId}");
            if (orderIds == null)
            {
                return;
            }
            foreach (var id in orderIds)
            {
                if (_dbContext.EvtOrderPayments.Where(p => p.OrderId == id && p.Status == OrderPaymentStatus.DA_THANH_TOAN && p.Deleted == YesNo.NO).Any())
                {
                    _evtOrderEFRepository.ThrowException(ErrorCode.EvtOrderPaymentNotFound);
                }
                var order = _dbContext.EvtOrders.Include(e => e.OrderDetails)
                                            .FirstOrDefault(e => e.Id == id && (e.Status == EvtOrderStatus.KHOI_TAO || e.Status == EvtOrderStatus.CHO_THANH_TOAN)
                                                        && e.Deleted == YesNo.NO
                                                        && e.TradingProviderId == tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);
                _evtHistoryUpdateEFRepository.Entity.Add(new EvtHistoryUpdate
                {
                    Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                    RealTableId = order.Id,
                    OldValue = order.Status.ToString(),
                    NewValue = "Đã xoá",
                    FieldName = "Trạng thái",
                    UpdateTable = EvtHistoryUpdateTables.EVT_ORDER,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = "Huỷ sổ lệnh"
                });
                order.Deleted = YesNo.YES;
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// gia han thoi gian
        /// <see cref="EvtUpdateReason"/>
        /// </summary>
        /// <param name="input"></param>
        public void ExtendedTime(EvtOrderExtendedTimeDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ExtendedTime)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");
            
            var order = _dbContext.EvtOrders.FirstOrDefault(o => o.Id == input.OrderId && o.Deleted == YesNo.NO && o.TradingProviderId == tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);

            if (!new int[] { EvtOrderStatus.KHOI_TAO, EvtOrderStatus.CHO_THANH_TOAN,
                 EvtOrderStatus.CHO_XU_LY }.Contains(order.Status))
            {
                _defErrorEFRepository.ThrowException(ErrorCode.RstOrderCannotExtendedKeepTime);
            }
            var transaction = _dbContext.Database.BeginTransaction();
            // luu lai ly do
            _evtHistoryUpdateEFRepository.Entity.Add(new EvtHistoryUpdate
            {
                Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                RealTableId = order.Id,
                OldValue = "",
                NewValue = input.KeepTime.ToString(),
                UpdateTable = EvtHistoryUpdateTables.EVT_ORDER,
                Action = ActionTypes.CAP_NHAT,
                FieldName = EvtFieldName.UPDATE_ORDER_EXPIRED_TIME,
                ActionUpdateType = EvtActionUpdateTypes.UPDATE_ORDER_EXPIRED_TIME,
                Summary = input.Summary,
                CreatedDate = DateTime.Now,
                UpdateReason = input.Reason,
            });
            _dbContext.SaveChanges();
            
            order.ExpiredTime = DateTime.Now.AddSeconds(input.KeepTime);
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// phê duyệt số lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task Approve(int orderId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Approve)} : orderIds = {orderId}, tradingProviderId = {tradingProviderId}");
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            var order = _dbContext.EvtOrders.Include(o => o.EventDetail).FirstOrDefault(o => (o.Status == EvtOrderStatus.KHOI_TAO
                                                                    || o.Status == EvtOrderStatus.CHO_THANH_TOAN
                                                                    || o.Status == EvtOrderStatus.CHO_XU_LY)
                                                                    && o.Id == orderId && o.Deleted == YesNo.NO
                                                                    && o.TradingProviderId == tradingProviderId)
                                                                    .ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);

            var totalMoney = _dbContext.EvtOrderDetails.Where(o => o.OrderId == orderId).Sum(o => o.Quantity * o.Price);
            var totalPaymentValue = _dbContext.EvtOrderPayments.Where(x => x.OrderId == order.Id && x.Status == OrderPaymentStatus.DA_THANH_TOAN
                                        && x.Deleted == YesNo.NO).Sum(op => op.PaymentAmount);
            if (totalPaymentValue == totalMoney)
            {
                order.Status = EvtOrderStatus.HOP_LE;
                order.ApproveDate = DateTime.Now;
                order.ApproveBy = userName;

                if(order.IsRequestReceiveRecipt)
                {
                    order.DeliveryInvoiceStatus = EventDeliveryStatus.WAITING;
                    order.PendingInvoiceDate = DateTime.Now;
                    order.PendingInvoiceDateModifiedBy = userName;
                }

                if (order.IsReceiveHardTicket)
                {
                    order.DeliveryStatus = EventDeliveryStatus.WAITING;
                    order.PendingDate = DateTime.Now;
                    order.PendingDateModifiedBy = userName;
                } 
                    
                await _eventNotificationServices.SendNotifyRegisterTicket(order.Id);
                order.TicketJobId = _backgroundJobs.Enqueue(() => _orderTicketFillService.FillOrderTicket(order.Id));
                _dbContext.SaveChanges();
                if (order.IsRequestReceiveRecipt)
                {
                    await _eventNotificationServices.SendAdminNotifyReceiveRecipt(order.Id);
                }
                if(order.IsReceiveHardTicket)
                {
                    await _eventNotificationServices.SendAdminNotifyReceiveHardTicket(order.Id);
                }
            }
            else
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtOrderCanNotAppove);
            }

            //Xử lý gọi hàm cập nhật realtime
            await _evtSignalRBroadcastService.BroadcastEventQuantityTicket(order.EventDetail.EventId);
        }

        /// <summary>
        /// cập nhật sổ lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public void Update(UpdateEvtOrderDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Update)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            var transaction = _dbContext.Database.BeginTransaction();
            var order = _dbContext.EvtOrders.FirstOrDefault(e => e.Status != EvtOrderStatus.DA_XOA && e.Id == input.Id && e.Deleted == YesNo.NO && e.TradingProviderId == tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);
            
            // cms linh dong cho update 
            //if (_dbContext.EvtOrderPayments.Where(c => c.Deleted == YesNo.NO && c.OrderId == input.Id && c.Status == OrderPaymentStatus.DA_THANH_TOAN).Any())
            //{
            //    _evtOrderEFRepository.ThrowException(ErrorCode.EvtOrderCanNotUpdateBecauseHavePayment);
            //}
            var eventDetail = _dbContext.EvtEventDetails.FirstOrDefault(e => e.Id == order.EventDetailId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventDetailNotFound);
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == eventDetail.EventId && e.Deleted == YesNo.NO);
            if (input.ContractAddressId != null && input.IsReceiveHardTicket)
            {
                var contractAddressFind = _dbContext.InvestorContactAddresses.FirstOrDefault(c => c.Deleted == YesNo.NO && c.ContactAddressId == input.ContractAddressId && c.InvestorId == input.InvestorId).ThrowIfNull(_dbContext, ErrorCode.InvestorContractNotFound);
                
            }
            if (input.ContractAddressId != null && input.IsReceiveHardTicket && !(_dbContext.InvestorContactAddresses.Where(c => c.Deleted == YesNo.NO && c.ContactAddressId == input.ContractAddressId && c.InvestorId == input.InvestorId).Any()))
            {
                _evtOrderEFRepository.ThrowException(ErrorCode.InvestorContractNotFound);
            }
            if (input.ContractAddressId == null && (input.IsReceiveHardTicket || input.IsRequestReceiveRecipt))
            {
                _evtOrderEFRepository.ThrowException(ErrorCode.EvtOrderContractAddressNotNull);
            }

            order.ContractAddressId = input.ContractAddressId;
 

            if (input.IsReceiveHardTicket && !eventFind.CanExportTicket)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtOrderReceiveHardTicketNotFound);
            }
            if (input.IsRequestReceiveRecipt && !eventFind.CanExportRequestRecipt)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtOrderRequestReceiveReciptNotFound);
            }

            // Tìm kiếm thông tin sale nếu có mã giới thiệu
            if (!string.IsNullOrWhiteSpace(input.SaleReferralCode))
            {
                var findSale = _saleEFRepository.AppFindSaleOrderByReferralCode(input.SaleReferralCode, tradingProviderId);
                if (findSale != null)
                {
                    order.ReferralSaleId = findSale.SaleId;
                    order.DepartmentId = findSale.DepartmentId;
                }
            }

            _dbContext.SaveChanges();

            int countFree = 0;

            if (input.OrderDetails != null)
            {
                foreach (var item in input.OrderDetails)
                {
                    var orderDetail = _dbContext.EvtOrderDetails.FirstOrDefault(od => od.Id == item.Id);

                    if (orderDetail != null)
                    {
                        var ticket = _dbContext.EvtTickets.FirstOrDefault(e => e.Id == item.TicketId
                                                                            && e.EndSellDate >= DateTime.Now
                                                                            && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtTicketNotFound);
                        if (ticket.IsFree)
                        {
                            countFree++;
                        };

                        if (countFree == input.OrderDetails.Count)
                        {
                            order.Status = EvtOrderStatus.HOP_LE;
                            order.ApproveDate = DateTime.Now;
                            order.ApproveBy = userName;
                        }

                        if (ticket != null)
                        {
                            orderDetail.TicketId = item.TicketId;
                            orderDetail.Quantity = item.Quantity;
                            orderDetail.Price = ticket.IsFree ? 0 : ticket.Price;

                            // Xóa các EvtOrderTicketDetail cũ của OrderDetail
                            var oldTicketDetails = _dbContext.EvtOrderTicketDetails.Where(td => td.OrderDetailId == orderDetail.Id);
                            _dbContext.EvtOrderTicketDetails.RemoveRange(oldTicketDetails);

                            //lay ra list TicketCode da random
                            List<string> listTicketCode = _evtOrderCommonService.ListTicketCodeGenerate(item.Quantity, EvtOrderMoreConst.DO_DAI_CHUOI_TICKET_CODE);
                            for (int i = 0; i < item.Quantity; i++)
                            {
                                _dbContext.EvtOrderTicketDetails.Add(new EvtOrderTicketDetail
                                {
                                    Id = (int)_evtOrderTicketDetailEFRepository.NextKey(),
                                    OrderDetailId = orderDetail.Id,
                                    TicketId = item.TicketId,
                                    Status = EvtOrderTicketStatus.CHUA_THAM_GIA,
                                    TicketCode = listTicketCode[i],
                                });
                            }
                        }
                    }
                }
                _dbContext.SaveChanges();
            }

            _evtHistoryUpdateEFRepository.Entity.Add(new EvtHistoryUpdate
            {
                Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                RealTableId = order.Id,
                UpdateTable = EvtHistoryUpdateTables.EVT_ORDER,
                Action = ActionTypes.CAP_NHAT,
                Summary = "Cập nhật sổ lệnh"
            });
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// danh sach sổ lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<EvtOrderDto> FindAll(FilterEvtOrderDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            var query = _dbContext.EvtOrders.AsNoTracking()
                                    .Include(e => e.InvestorIdentification)
                                    .Include(e => e.EventDetail).ThenInclude(ed => ed.Event)
                                    .Include(e => e.OrderDetails)
                                    .Include(e => e.OrderPayments)
                                    .Include(e => e.Investor)
                                    .Include(e => e.ContractAddress)
                                    .Where(t => t.Deleted == YesNo.NO
                                        && t.EventDetail.Event.TradingProviderId == tradingProviderId
                                        && ((input.Statuses == null || input.Statuses.Count == 0) || input.Statuses.Contains(t.Status))
                                        && ((input.EventIds == null || input.EventIds.Count == 0) || input.EventIds.Contains(t.EventDetail.EventId))
                                        && ((input.Status == null && t.Status != EvtOrderStatus.CHO_XU_LY && t.Status != EvtOrderStatus.HOP_LE 
                                            && t.Status != EvtOrderStatus.DA_XOA) 
                                           || (input.Status != null && t.Status == input.Status))
                                        && (input.TimeOut != true || t.ExpiredTime < DateTime.Now) 
                                        && (input.EventId == null || t.EventDetail.EventId == input.EventId)
                                        && (input.Phone == null || t.Investor.Phone.ToLower().Contains(input.Phone.ToLower()))
                                        && (input.Keyword == null || t.Investor.Phone.ToLower().Contains(input.Keyword.ToLower())
                                            || t.InvestorIdentification.Fullname.ToLower().Contains(input.Keyword.ToLower()) || t.EventDetail.Event.Name.ToLower().Contains(input.Keyword.ToLower())
                                            || t.ContractCode.ToLower().Contains(input.Keyword.ToLower()) || t.ContractCodeGen.ToLower().Contains(input.Keyword.ToLower()))
                                        && (input.Orderer == null ||
                                            (t.Source == SourceOrder.ONLINE && input.Orderer == SourceOrderFE.KHACH_HANG) ||
                                            (t.Source == SourceOrder.OFFLINE && t.ReferralSaleId == null
                                                && input.Orderer == SourceOrderFE.QUAN_TRI_VIEN) ||
                                            (input.Orderer == SourceOrderFE.SALE))
                                        && ((input.Statuses == null || input.Statuses.Count == 0) || (!input.Orderers.Contains(t.Source) && input.Orderers.Count == 1)
                                            || (input.Orderers.Count == 2 && ((input.Orderers.Contains(SourceOrderFE.QUAN_TRI_VIEN) && t.Source == SourceOrder.OFFLINE)
                                            || (input.Orderers.Contains(SourceOrderFE.KHACH_HANG) && t.Source == SourceOrder.ONLINE)))))
                                    .SelectEvtOrderDto(_mapper);

            var result = new PagingResult<EvtOrderDto>();

            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = query;
            return result;
        }

        private static int CalcCurrentTickets(EpicSchemaDbContext dbContext, int ticketId)
        {
            int orderDetail = dbContext.EvtOrderDetails
                                .Include(od => od.Order).Include(od => od.Ticket)
                                .Where(od => od.TicketId == ticketId
                                            && od.Ticket.EndSellDate >= DateTime.Now
                                            && od.Order.Status == EvtOrderStatus.HOP_LE
                                            || (od.Order.Status == EvtOrderStatus.CHO_THANH_TOAN && od.Order.ExpiredTime >= DateTime.Now))
                                .Sum(od => od.Quantity);

            return orderDetail;
        }

        /// <summary>
        /// chi tiết sổ lệnh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EvtOrderDto GetById(int id)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(GetById)}: id = {id}");

            var order = _dbContext.EvtOrders.FirstOrDefault(e => e.Id == id && e.Deleted == YesNo.NO && e.TradingProviderId == tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);
            
            var query = _dbContext.EvtOrders
                    .Include(e => e.Sale)
                    .Include(e => e.ContractAddress)
                    .Include(e => e.Department)
                    .Include(e => e.EventDetail).ThenInclude(ed => ed.Event).ThenInclude(ev => ev.EventTypes)
                    .Include(e => e.EventDetail).ThenInclude(ed => ed.Event).ThenInclude(ev => ev.Province)
                    .Include(t => t.OrderDetails).ThenInclude(od => od.Ticket)
                    .Include(t => t.Investor)
                    .Include(t => t.InvestorIdentification)
                    .Where(t => t.Deleted == YesNo.NO && t.Id == id && t.TradingProviderId == tradingProviderId)
                    .Select(t => new EvtOrderDto
                    {
                        Id = t.Id,
                        CreatedDate = t.CreatedDate,
                        ApproveDate = t.ApproveDate,
                        ContractCode = t.ContractCodeGen ?? t.ContractCode,
                        ContractCodeGen = t.ContractCodeGen,
                        ContractAddressId = t.ContractAddressId,
                        ContractAddressName = t.ContractAddress.ContactAddress,
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
                        Phone = t.Investor.Phone,
                        Email = t.Investor.Email,
                        AddressInvestor = t.Investor.InvestorContactAddresses.Where(ic => ic.Deleted == YesNo.NO && ic.IsDefault == YesNo.NO)
                                                                            .Select(ic => ic.ContactAddress).FirstOrDefault(),
                        Fullname = t.InvestorIdentification.Fullname,
                        IdNo = t.InvestorIdentification.IdNo,
                        EventName = t.EventDetail.Event.Name,
                        Organizator = t.EventDetail.Event.Organizator,
                        Types = t.EventDetail.Event.EventTypes.Select(e => e.Type),
                        Location = t.EventDetail.Event.Location,
                        Province = t.EventDetail.Event.Province.Name,
                        Address = t.EventDetail.Event.Address,
                        DepartmentId = t.DepartmentId,
                        StartDate = t.EventDetail.StartDate,
                        EndDate = t.EventDetail.EndDate,
                        SaleInfo = new EvtOrderSaleDto
                        {
                            SaleName = t.Sale.Investor.Name,
                            SalePhone = t.Sale.Investor.Phone,
                            ReferralCode = t.Sale.Investor.ReferralCodeSelf,
                            SaleEmail = t.Sale.Investor.Email,
                            ManagerDepartmentName = t.Department.DepartmentName,
                            DepartmentName = (from departmentSale in _dbContext.DepartmentSales 
                                                join department in _dbContext.Departments on departmentSale.DepartmentId equals department.DepartmentId
                                                where departmentSale.SaleId == t.ReferralSaleId && departmentSale.TradingProviderId == tradingProviderId
                                              select department.DepartmentName).FirstOrDefault(),
                        },
                        OrderDetails = t.OrderDetails.Select(ed => new EvtOrderDetailDto
                        {
                            Id = ed.Id,
                            OrderId = ed.OrderId,
                            TicketId = ed.TicketId,
                            Price = ed.Price,
                            Quantity = ed.Quantity,
                            Name = ed.Ticket.Name,
                            Depscription = ed.Ticket.Description,
                            CurrentTickets = ed.Ticket.Quantity - CalcCurrentTickets(_dbContext, ed.TicketId),
                        }),
                    })
                    .FirstOrDefault();
            return query;
        }

        /// <summary>
        /// Danh sách lịch sử theo bảng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<EvtHistoryUpdateDto> FindAllByTable(FilterEvtHistoryUpdateDto input)
        {
            _logger.LogInformation($"{nameof(FindAllByTable)}: input = {JsonSerializer.Serialize(input)}");
            var resultPaging = new PagingResult<EvtHistoryUpdateDto>();

            var history = _evtHistoryUpdateEFRepository.EntityNoTracking.Where(g => (input.UpdateTable == null || g.UpdateTable == input.UpdateTable) 
                            && (input.RealTableId == null || input.RealTableId == g.RealTableId))
                            .Select(o => new EvtHistoryUpdateDto
                            {
                                Id = o.Id,
                                Action = o.Action,
                                FieldName = o.FieldName,
                                OldValue = o.OldValue,
                                NewValue = o.NewValue,
                                RealTableId = o.RealTableId,
                                UpdateTable = o.UpdateTable,
                                Summary = o.Summary,
                                CreatedBy = o.CreatedBy,
                                CreatedDate = o.CreatedDate
                            });
            resultPaging.TotalItems = history.Count();
            if (input.PageSize != -1)
            {
                history = history.Skip(input.Skip).Take(input.PageSize);
            }
            resultPaging.Items = history;
            return resultPaging;
        }

        /// <summary>
        /// Thông tin vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<EvtOrderTicketInfo> GetOrderTicketInfoById(FilterEvtOrderTicketInfoDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(GetOrderTicketInfoById)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");
            var order = _dbContext.EvtOrders.FirstOrDefault(e => e.Id == input.OrderId && e.Deleted == YesNo.NO && e.TradingProviderId == tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);
            var result = new PagingResult<EvtOrderTicketInfo>();
            var query = _dbContext.EvtOrderTicketDetails
                            .Include(o => o.OrderDetail)
                            .Include(o => o.Ticket)
                            .Where(o => o.OrderDetail.OrderId == input.OrderId
                                && o.OrderDetail.Order.Deleted == YesNo.NO)
                            .Select(o => new EvtOrderTicketInfo
                            {
                                Id = o.Id,
                                TicketId = o.TicketId,
                                OrderDetailId = o.OrderDetailId,
                                Name = o.Ticket.Name,
                                TicketCode = o.TicketCode,
                                CheckIn = o.CheckIn,
                                CheckOut = o.CheckOut,
                                Status = o.Status,
                                TicketFilledUrl = o.TicketFilledUrl,
                            });

            result.TotalItems = query.Count();
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = query;
            return result;
        }

        /// <summary>
        /// danh sach giao nhan hoa don
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<EvtOrderDto> FindAllDeliveryInvoice(FilterEvtOrderDeliveryInvoiceDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindAllDeliveryInvoice)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            var query = _dbContext.EvtOrders.Include(e => e.EventDetail).ThenInclude(ed => ed.Event).ThenInclude(e => e.EventTypes)
                                            .Include(e => e.OrderDetails)
                                            .Include(e => e.Investor)
                                            .Include(e => e.InvestorIdentification)
                                            .Where(t => t.Deleted == YesNo.NO
                                                && t.EventDetail.Event.TradingProviderId == tradingProviderId
                                                && (input.EventId == null || t.EventDetail.EventId == input.EventId)
                                                && (input.Phone == null || t.Investor.Phone.ToLower().Contains(input.Phone.ToLower()))
                                                && (input.Orderer == null ||
                                                    (t.Source == SourceOrder.ONLINE && input.Orderer == SourceOrderFE.KHACH_HANG) ||
                                                    (t.Source == SourceOrder.OFFLINE && t.ReferralSaleId == null
                                                        && input.Orderer == SourceOrderFE.QUAN_TRI_VIEN) ||
                                                    (input.Orderer == SourceOrderFE.SALE))
                                                && (input.EventName == null || t.EventDetail.Event.Name.ToLower().Contains(input.EventName.ToLower()))
                                                && t.IsRequestReceiveRecipt == true
                                                && (t.Status == EvtOrderStatus.CHO_XU_LY || t.Status == EvtOrderStatus.HOP_LE)
                                                && (input.EventStatus == null || t.EventDetail.Event.Status == input.EventStatus)
                                                && (input.Keyword == null
                                                    || t.ContractCode.ToLower().Contains(input.Keyword.ToLower())
                                                    || t.ContractCodeGen.ToLower().Contains(input.Keyword.ToLower())
                                                    || t.Investor.Phone.ToLower().Contains(input.Keyword.ToLower())
                                                    || t.InvestorIdentification.Fullname.ToLower().Contains(input.Keyword.ToLower())
                                                    )
                                                && ((input.EventType == null || input.EventType.Count == 0) || t.EventDetail.Event.EventTypes.Any(et => input.EventType.Contains(et.Type)))
                                                && ((input.DeliveryInvoiceStatus == null || input.DeliveryInvoiceStatus.Count == 0) || input.DeliveryInvoiceStatus.Contains(t.DeliveryInvoiceStatus ?? 0))
                                                && (input.ProcessingInvoiceDate == null || (t.PendingInvoiceDate.Value.Date == input.ProcessingInvoiceDate.Value.Date
                                                    && t.DeliveryInvoiceDate.Value.Date == input.ProcessingInvoiceDate.Value.Date && t.FinishedDate.Value.Date == input.ProcessingInvoiceDate.Value.Date))
                                                )
                                            .Select(t => new EvtOrderDto
                                            {
                                                Id = t.Id,
                                                EventDetailId = t.EventDetailId,
                                                DeliveryInvoiceStatus = t.DeliveryInvoiceStatus,
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
                                                ContractCodeGen = t.ContractCodeGen,
                                                EventName = t.EventDetail.Event.Name,
                                                PendingInvoiceDate = t.PendingInvoiceDate,
                                                DeliveryInvoiceDate = t.DeliveryDate,
                                                PendingInvoiceDateModifiedBy = t.PendingInvoiceDateModifiedBy,
                                                FinishedInvoiceDateModifiedBy = t.FinishedInvoiceDateModifiedBy,
                                                FinishedInvoiceDate = t.FinishedInvoiceDate,
                                                OrderDetails = _mapper.Map<IEnumerable<EvtOrderDetailDto>>(t.OrderDetails)
                                            });




            var result = new PagingResult<EvtOrderDto>();

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
        /// đổi trạng thái giao nhận vé hoặc hóa đơn
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="deliveryStatus"></param>
        /// <see cref="EvtOrderDeliveryTypes"/>
        /// <param name="type"></param>
        public void ChangeDeliveryStatus(int orderId, int deliveryStatus, int type = EvtOrderDeliveryTypes.TICKET)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeDeliveryStatus)}: orderId = {orderId}, status = {deliveryStatus}, username = {username}");
            var orderFind = _dbContext.EvtOrders.FirstOrDefault(o => o.Id == orderId && o.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);
           
            if (type == EvtOrderDeliveryTypes.TICKET)
            {
                orderFind.DeliveryStatus = deliveryStatus;
                if (orderFind.DeliveryStatus == EventDeliveryStatus.DELIVERY)
                {
                    orderFind.DeliveryDate = DateTime.Now;
                    orderFind.DeliveryDateModifiedBy = username;
                }
                else if (orderFind.DeliveryStatus == EventDeliveryStatus.COMPLETE)
                {
                    orderFind.FinishedDate = DateTime.Now;
                    orderFind.FinishedDateModifiedBy = username;
                }

            } else if(type == EvtOrderDeliveryTypes.INVOICE)
            {
                orderFind.DeliveryInvoiceStatus = deliveryStatus;
                if (orderFind.DeliveryInvoiceStatus == EventDeliveryStatus.DELIVERY)
                {
                    orderFind.DeliveryInvoiceDate = DateTime.Now;
                    orderFind.DeliveryInvoiceDateModifiedBy = username;
                }
                else if (orderFind.DeliveryInvoiceStatus == EventDeliveryStatus.COMPLETE)
                {
                    orderFind.FinishedInvoiceDate = DateTime.Now;
                    orderFind.FinishedInvoiceDateModifiedBy = username;
                }
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Danh sách hợp đồng giao nhận vé sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<EvtOrderDto> FindAllDelivery(FilterEvtOrderDeliveryTicketDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindAllDelivery)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            var query = _dbContext.EvtOrders.Include(e => e.EventDetail).ThenInclude(ed => ed.Event).ThenInclude(e => e.EventTypes)
                                            .Include(e => e.OrderDetails)
                                            .Include(e => e.Investor)
                                            .Include(e => e.InvestorIdentification)
                                            .Where(t => t.Deleted == YesNo.NO
                                                && t.EventDetail.Event.TradingProviderId == tradingProviderId
                                                && (input.EventId == null || t.EventDetail.EventId == input.EventId)
                                                && (input.Phone == null || t.Investor.Phone.ToLower().Contains(input.Phone.ToLower()))
                                                && (input.Orderer == null ||
                                                    (t.Source == SourceOrder.ONLINE && input.Orderer == SourceOrderFE.KHACH_HANG) ||
                                                    (t.Source == SourceOrder.OFFLINE && t.ReferralSaleId == null
                                                        && input.Orderer == SourceOrderFE.QUAN_TRI_VIEN) ||
                                                    (input.Orderer == SourceOrderFE.SALE))
                                                && t.IsReceiveHardTicket == true
                                                && (t.Status == EvtOrderStatus.CHO_XU_LY || t.Status == EvtOrderStatus.HOP_LE)
                                                && (input.EventName == null || t.EventDetail.Event.Name.ToLower().Contains(input.EventName.ToLower()))
                                                && (input.EventStatus == null || t.EventDetail.Event.Status == input.EventStatus)
                                                && ((input.DeliveryStatus == null || input.DeliveryStatus.Count == 0) || input.DeliveryStatus.Contains(t.DeliveryStatus ?? 0))
                                                && ((input.EventType == null || input.EventType.Count == 0) || t.EventDetail.Event.EventTypes.Any(et => input.EventType.Contains(et.Type)))
                                                && (input.ProcessingDate == null || (t.PendingDate.Value.Date == input.ProcessingDate.Value.Date
                                                    && t.DeliveryDate.Value.Date == input.ProcessingDate.Value.Date && t.FinishedDate.Value.Date == input.ProcessingDate.Value.Date))
                                                && (input.Keyword == null 
                                                    || t.ContractCode.ToLower().Contains(input.Keyword.ToLower()) 
                                                    || t.ContractCodeGen.ToLower().Contains(input.Keyword.ToLower())
                                                    || t.Investor.Phone.ToLower().Contains(input.Keyword.ToLower())
                                                    || t.InvestorIdentification.Fullname.ToLower().Contains(input.Keyword.ToLower())
                                                    )
                                                )
                                            .Select(t => new EvtOrderDto
                                            {
                                                Id = t.Id,
                                                DeliveryStatus = t.DeliveryStatus,
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
                                                ContractCodeGen = t.ContractCodeGen,
                                                EventName = t.EventDetail.Event.Name,
                                                PendingDate = t.PendingDate,
                                                DeliveryDate = t.DeliveryDate,
                                                FinishedDate = t.FinishedDate,
                                                PendingDateModifiedBy = t.PendingDateModifiedBy,
                                                FinishedDateModifiedBy = t.FinishedDateModifiedBy,
                                                OrderDetails = _mapper.Map<IEnumerable<EvtOrderDetailDto>>(t.OrderDetails),
                                                TicketFilledUrls = t.OrderDetails.Where(e => e.OrderTicketDetails.All(e => e.TicketFilledUrl != null)).SelectMany(e => e.OrderTicketDetails.Select(e => e.TicketFilledUrl))
                                            });

            var result = new PagingResult<EvtOrderDto>();

            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = query;
            return result;
        }

        public async Task ReceiveHardTicket(int orderId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ReceiveHardTicket)}: orderId = {orderId}, username = {username}");
            var orderFind = _dbContext.EvtOrders.FirstOrDefault(o => o.Id == orderId && o.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);
            if (orderFind.ContractAddressId == null)
            {
                _evtOrderEFRepository.ThrowException(ErrorCode.EvtOrderContractAddressNotNull);
            }
            if (_dbContext.EvtEventDetails.Include(e => e.Event).Any(e => e.Id == orderFind.EventDetailId && e.Deleted == YesNo.NO
                                                                     && !e.Event.CanExportTicket && e.Event.Deleted == YesNo.NO))
            {
                _evtOrderEFRepository.ThrowException(ErrorCode.EvtOrderReceiveHardTicketNotFound);
            }
            if (orderFind.DeliveryStatus == null)
            {
                orderFind.IsReceiveHardTicket = true;
                orderFind.DeliveryStatus = EventDeliveryStatus.WAITING;
                orderFind.PendingDate = DateTime.Now;
                orderFind.PendingDateModifiedBy = username;
            }
            _dbContext.SaveChanges();
            await _eventNotificationServices.SendAdminNotifyReceiveHardTicket(orderFind.Id);
        }

        /// <summary>
        /// yeu cau nhan hoa don
        /// </summary>
        /// <param name="orderId"></param>
        public async Task InvoiceRequest(int orderId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(InvoiceRequest)}: orderId = {orderId}, username = {username}");
            var orderFind = _dbContext.EvtOrders.FirstOrDefault(o => o.Id == orderId && o.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);
            if (orderFind.ContractAddressId == null)
            {
                _evtOrderEFRepository.ThrowException(ErrorCode.EvtOrderContractAddressNotNull);
            }
            if (_dbContext.EvtEventDetails.Include(e => e.Event).Any(e => e.Id == orderFind.EventDetailId && e.Deleted == YesNo.NO 
                                                                        && !e.Event.CanExportRequestRecipt && e.Event.Deleted == YesNo.NO))
            {
                _evtOrderEFRepository.ThrowException(ErrorCode.EvtOrderRequestReceiveReciptNotFound);
            }
            if (orderFind.DeliveryInvoiceStatus == null)
            {
                orderFind.IsRequestReceiveRecipt = true;
                orderFind.DeliveryInvoiceStatus = EventDeliveryStatus.WAITING;
                orderFind.PendingInvoiceDate = DateTime.Now;
                orderFind.PendingInvoiceDateModifiedBy = username;
            }
            _dbContext.SaveChanges();
            await _eventNotificationServices.SendAdminNotifyReceiveRecipt(orderFind.Id);
        }
    }
}
