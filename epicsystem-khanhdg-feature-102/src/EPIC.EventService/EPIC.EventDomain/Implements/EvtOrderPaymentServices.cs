using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.EventDomain.Interfaces;
using EPIC.EventRepositories;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.EventEntites.Entites;
using System.Text.Json;
using EPIC.EventEntites.Dto.EvtOrderPayment;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Utils.Linq;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.GarnerEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.EventEntites.Dto.EvtEventMedia;
using MySqlX.XDevAPI.Common;
using Microsoft.EntityFrameworkCore;
using EPIC.Notification.Services;
using Microsoft.AspNetCore.Mvc;

namespace EPIC.EventDomain.Implements
{
    public class EvtOrderPaymentServices : IEvtOrderPaymentServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EvtOrderPaymentServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly EvtOrderPaymentEFRepository _evtOrderPaymentEFRepository;
        private readonly EvtOrderEFRepository _evtOrderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly EvtHistoryUpdateEFRepository _evtHistoryUpdateEFRepository;
        private readonly EventNotificationServices _eventNotificationServices;
        private readonly IEvtSignalRBroadcastService _evtSignalRBroadcastService;

        public EvtOrderPaymentServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<EvtOrderPaymentServices> logger,
            IHttpContextAccessor httpContextAccessor,
            EventNotificationServices eventNotificationServices,
            IEvtSignalRBroadcastService evtSignalRBroadcastService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _evtOrderPaymentEFRepository = new EvtOrderPaymentEFRepository(dbContext, logger);
            _evtOrderEFRepository = new EvtOrderEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _evtHistoryUpdateEFRepository = new EvtHistoryUpdateEFRepository(dbContext, logger);
            _eventNotificationServices = eventNotificationServices;
            _evtSignalRBroadcastService = evtSignalRBroadcastService;
        }

        /// <summary>
        /// Thêm thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public EvtOrderPaymentDto Add(CreateEvtOrderPaymentDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");
            
            // Thêm thanh toán
            var insert = new EvtOrderPayment
            {
                Id = (int)_evtOrderPaymentEFRepository.NextKey(),
                TradingProviderId = tradingProviderId,
                OrderId = input.OrderId,
                TradingBankAccountId = input.TradingBankAccountId,
                TranDate = input.TranDate,
                TranClassify = input.TranClassify,
                PaymentType = input.PaymentType,
                PaymentAmount = input.PaymentAmount,
                Description = input.Description
            };
            var result = _dbContext.EvtOrderPayments.Add(insert);
            _evtHistoryUpdateEFRepository.Entity.Add(new EvtHistoryUpdate
            {
                Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                RealTableId = insert.Id,
                OldValue = null,
                NewValue = "Khởi tạo",
                FieldName = "Trạng thái",
                UpdateTable = EvtHistoryUpdateTables.EVT_ORDER_PAYMENT,
                Action = ActionTypes.THEM_MOI,
                Summary = "Thêm thanh toán"
            });
            _dbContext.SaveChanges();
            return _mapper.Map<EvtOrderPaymentDto>(result.Entity);
        }

        /// <summary>
        /// Cập nhật thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public EvtOrderPaymentDto Update(UpdateEvtOrderPaymentDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");
            var orderPayment = _evtOrderPaymentEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtOrderPaymentNotFound);

            if (orderPayment.Status != OrderPaymentStatus.NHAP)
            {
                _evtOrderPaymentEFRepository.ThrowException(ErrorCode.EvtOrderPaymentNotUpdate);
            }

            orderPayment.TranDate = input.TranDate;
            orderPayment.TranClassify = input.TranClassify;
            orderPayment.PaymentType = input.PaymentType;
            orderPayment.PaymentAmount = input.PaymentAmount;
            orderPayment.Description = input.Description;
            _evtHistoryUpdateEFRepository.Entity.Add(new EvtHistoryUpdate
            {
                Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                RealTableId = orderPayment.Id,
                UpdateTable = EvtHistoryUpdateTables.EVT_ORDER_PAYMENT,
                Action = ActionTypes.CAP_NHAT,
                Summary = "Cập nhật thanh toán"
            });
            _dbContext.SaveChanges();
            return _mapper.Map<EvtOrderPaymentDto>(orderPayment);
        }

        /// <summary>
        /// Xoá thanh toán
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Delete)}: id = {id}, tradingProviderId = {tradingProviderId}");

            var orderPayment = _evtOrderPaymentEFRepository.Entity.FirstOrDefault(x => x.Id == id && x.TradingProviderId == tradingProviderId && x.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtOrderPaymentNotFound);
            // Hợp đồng đang ở trạng thái đã thanh toán thì ko xóa được, hủy duyệt để xóa
            if (orderPayment.Status == OrderPaymentStatus.DA_THANH_TOAN)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtOrderPaymentCanNotDelete);
            }

            orderPayment.Deleted = YesNo.YES;

            _evtHistoryUpdateEFRepository.Entity.Add(new EvtHistoryUpdate
            {
                Id = (int)_evtHistoryUpdateEFRepository.NextKey(),
                RealTableId = orderPayment.Id,
                UpdateTable = EvtHistoryUpdateTables.EVT_ORDER_PAYMENT,
                Action = ActionTypes.XOA,
                Summary = "Xoá thanh toán"
            });
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm thanh toán theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EvtOrderPaymentDto FindById(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindById)}: id = {id}, tradingProviderId = {tradingProviderId}");

            var orderPayment = _evtOrderPaymentEFRepository.Entity.Select(o => new EvtOrderPaymentDto
            {
                Id = o.Id,
                TradingProviderId = o.TradingProviderId,
                OrderId = o.OrderId,
                OrderNo = o.OrderNo,
                TradingBankAccountId = o.TradingBankAccountId,
                TranDate = o.TranDate,
                TranClassify = o.TranClassify,
                PaymentType = o.PaymentType,
                PaymentAmount = o.PaymentAmount,
                Description = o.Description,
                Status = o.Status,
                ApproveDate = o.ApproveDate,
                ApproveBy = o.ApproveBy,
                CancelDate = o.CancelDate,
                CancelBy = o.CancelBy,
                CreatedDate = o.CreatedDate,
                CreatedBy = o.CreatedBy,
                Deleted = o.Deleted,
                BankAccount = _dbContext.BusinessCustomerBanks
                               .Where(b => b.BusinessCustomerBankAccId == o.TradingBankAccountId && b.Deleted == YesNo.NO)
                               .Select(b => new BusinessCustomerBankDto
                               {
                                   BusinessCustomerBankAccId = b.BusinessCustomerBankAccId,
                                   BusinessCustomerId = b.BusinessCustomerId,
                                   BankAccName = b.BankAccName,
                                   BankAccNo = b.BankAccNo,
                                   BankCode = b.CoreBank.BankCode,
                                   Logo = b.CoreBank.Logo,
                                   BankId = b.BankId,
                                   BankName = b.CoreBank.BankName,
                                   FullBankName = b.CoreBank.FullBankName,
                                   Status = b.Status,
                                   IsDefault = b.IsDefault
                               }).FirstOrDefault()
            }).FirstOrDefault(x => x.Id == id && x.TradingProviderId == tradingProviderId && x.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtOrderPaymentNotFound);
            return orderPayment;
        }

        /// <summary>
        /// Tìm danh sách thanh toán phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<EvtOrderPaymentDto> FindAll(FilterEvtOrderPaymentDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            var result = new PagingResult<EvtOrderPaymentDto>();
            var resultItems = new List<EvtOrderPaymentDto>();
            var orderPaymentQuery = _evtOrderPaymentEFRepository.EntityNoTracking.Where(op => op.OrderId == input.OrderId && op.Deleted == YesNo.NO
                                            && op.TradingProviderId == tradingProviderId
                                            && (input.Status == null || input.Status == op.Status))
                                    .Select(o => new EvtOrderPaymentDto
                                    {
                                        Id = o.Id,
                                        TradingProviderId = o.TradingProviderId,
                                        OrderId = o.OrderId,
                                        OrderNo = o.OrderNo,
                                        TradingBankAccountId = o.TradingBankAccountId,
                                        TranDate = o.TranDate,
                                        TranClassify = o.TranClassify,
                                        PaymentType = o.PaymentType,
                                        PaymentAmount = o.PaymentAmount,
                                        Description = o.Description,
                                        Status = o.Status,
                                        ApproveDate = o.ApproveDate,
                                        ApproveBy = o.ApproveBy,
                                        CancelDate = o.CancelDate,
                                        CancelBy = o.CancelBy,
                                        CreatedDate = o.CreatedDate,
                                        CreatedBy = o.CreatedBy,
                                        Deleted = o.Deleted,
                                        BankAccount = _dbContext.BusinessCustomerBanks
                                                       .Where(b => b.BusinessCustomerBankAccId == o.TradingBankAccountId && b.Deleted == YesNo.NO)
                                                       .Select(b => new BusinessCustomerBankDto
                                                       {
                                                           BusinessCustomerBankAccId = b.BusinessCustomerBankAccId,
                                                           BusinessCustomerId = b.BusinessCustomerId,
                                                           BankAccName = b.BankAccName,
                                                           BankAccNo = b.BankAccNo,
                                                           BankCode = b.CoreBank.BankCode,
                                                           Logo = b.CoreBank.Logo,
                                                           BankId = b.BankId,
                                                           BankName = b.CoreBank.BankName,
                                                           FullBankName = b.CoreBank.FullBankName,
                                                           Status = b.Status,
                                                           IsDefault = b.IsDefault
                                                       }).FirstOrDefault()
                                    });
            result.TotalItems = orderPaymentQuery.Count();
            orderPaymentQuery = orderPaymentQuery.OrderDynamic(input.Sort);

            if (input.PageSize != -1)
            {
                orderPaymentQuery = orderPaymentQuery.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = orderPaymentQuery;
            return result;
        }

        /// <summary>
        /// Duyệt thanh toán, hủy thanh toán
        /// duyệt thanh toán nếu đủ tiền thì update trạng thái order thành chờ xử lý, nếu k đủ tiền thì về chờ thanh toán
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task ApprovePayment(int id, int status)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ApprovePayment)}: Id: {id}; Status: {status}");

            var orderPayment = _evtOrderPaymentEFRepository.Entity.FirstOrDefault(d => d.Id == id && d.TradingProviderId == tradingProviderId && d.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtOrderPaymentNotFound);

            if (orderPayment.TranDate > DateTime.Now)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtOrderPaymentApproveDateFuture);
            }

            var order = _evtOrderEFRepository.Entity.FirstOrDefault(o => o.Id == orderPayment.OrderId && o.Deleted == YesNo.NO && o.TradingProviderId == tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);

            if (order.Status == EvtOrderStatus.KHOI_TAO || order.Status == EvtOrderStatus.CHO_THANH_TOAN
                || order.Status == EvtOrderStatus.CHO_XU_LY || order.Status == EvtOrderStatus.HOP_LE)
            {
                var totalMoney = _dbContext.EvtOrderDetails
                    .Where(od => od.OrderId == orderPayment.OrderId)
                    .Sum(od => od.Quantity * od.Price);
                var totalPaymentValue = _evtOrderPaymentEFRepository.Entity.Where(x => x.OrderId == order.Id && x.Status == OrderPaymentStatus.DA_THANH_TOAN
                                        && x.Deleted == YesNo.NO).Sum(op => op.PaymentAmount);

                // duyệt, nếu số tiền đủ -> chuyển trạng thái order sang chờ xử lý, nếu không đủ hoặc thừa chuyển về chờ thanh toán
                if (status == OrderPaymentStatus.DA_THANH_TOAN)
                {
                    orderPayment.Status = OrderPaymentStatus.DA_THANH_TOAN;
                    orderPayment.ApproveBy = username;
                    orderPayment.ApproveDate = DateTime.Now;
                    var totalPaymentWithCurrentPayment = totalPaymentValue + orderPayment.PaymentAmount;
                    bool hasEnoughPayment = totalPaymentWithCurrentPayment >= totalMoney;

                    if (hasEnoughPayment)
                    {
                        if (order.IsReceiveHardTicket)
                        {
                            order.PendingDate = DateTime.Now;
                            order.PendingDateModifiedBy = username;
                            order.DeliveryStatus = EventDeliveryStatus.WAITING;
                        }

                        if (order.IsRequestReceiveRecipt)
                        {
                            order.DeliveryInvoiceStatus = EventDeliveryStatus.WAITING;
                            order.PendingInvoiceDate = DateTime.Now;
                            order.PendingInvoiceDateModifiedBy = username;
                        }
                    }

                    order.Status = hasEnoughPayment ? EvtOrderStatus.CHO_XU_LY : EvtOrderStatus.CHO_THANH_TOAN;
                    order.ExpiredTime = null;

                    await _eventNotificationServices.SendAdminNotifyApprovePaymentOrder(id);
                    await _eventNotificationServices.SendNotifyApprovePaymentOrder(id);
                }
                else if (status == OrderPaymentStatus.HUY_THANH_TOAN)
                {
                    if ((totalPaymentValue - orderPayment.PaymentAmount) >= totalMoney)
                    {
                        // Đổi trạng thái của sổ lệnh
                        order.Status = EvtOrderStatus.CHO_XU_LY;
                    }
                    else
                    {
                        // Đổi trạng thái của sổ lệnh
                        order.Status = EvtOrderStatus.CHO_THANH_TOAN;
                    }

                    orderPayment.Status = OrderPaymentStatus.HUY_THANH_TOAN;
                    orderPayment.CancelBy = username;
                    orderPayment.CancelDate = DateTime.Now;
                }
                _dbContext.SaveChanges();
                await _evtSignalRBroadcastService.BroadcastOrderExpiredTime(order.Id);
            }
        }

        public IEnumerable<BusinessCustomerBankDto> FindListBank(int orderId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindListBank)}: tradingProviderId = {tradingProviderId}");
            var eventId = _dbContext.EvtOrders.Include(o => o.EventDetail).ThenInclude(o => o.Event).Where(o => o.Id == orderId && o.EventDetail.Deleted == YesNo.NO && o.EventDetail.Event.Deleted == YesNo.NO).Select(o => o.EventDetail.Event.Id).FirstOrDefault();
            var result = from bankAccount in _dbContext.EvtEventBankAccounts
                        join businessCustomerBank in _dbContext.BusinessCustomerBanks on bankAccount.BusinessCustomerBankAccId equals businessCustomerBank.BusinessCustomerBankAccId
                        where bankAccount.EventId == eventId && bankAccount.Deleted == YesNo.NO && businessCustomerBank.Deleted == YesNo.NO
                        select new BusinessCustomerBankDto
                        {
                            BusinessCustomerBankAccId = businessCustomerBank.BusinessCustomerBankAccId,
                            BusinessCustomerId = businessCustomerBank.BusinessCustomerId,
                            BankAccName = businessCustomerBank.BankAccName,
                            BankAccNo = businessCustomerBank.BankAccNo,
                            BankCode = businessCustomerBank.CoreBank.BankCode,
                            Logo = businessCustomerBank.CoreBank.Logo,
                            BankId = businessCustomerBank.BankId,
                            BankName = businessCustomerBank.CoreBank.BankName,
                            FullBankName = businessCustomerBank.CoreBank.FullBankName,
                            Status = businessCustomerBank.Status,
                            IsDefault = businessCustomerBank.IsDefault
                        };
            return result;
        }
    }
}
