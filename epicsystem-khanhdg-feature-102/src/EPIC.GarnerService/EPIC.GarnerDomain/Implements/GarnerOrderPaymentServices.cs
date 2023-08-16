using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerOrderPayment;
using EPIC.GarnerRepositories;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerOrderPaymentServices : IGarnerOrderPaymentServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GarnerOrderPaymentServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly GarnerOrderPaymentEFRepository _garnerOrderPaymentEFRepository;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly GarnerNotificationServices _garnerNotificationServices;
        private readonly GarnerDistributionEFRepository _garnerDistributionEFRepository;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly IGarnerOrderContractFileServices _garnerOrderContractFileServices;

        public GarnerOrderPaymentServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<GarnerOrderPaymentServices> logger,
            IHttpContextAccessor httpContextAccessor,
            GarnerNotificationServices garnerNotificationServices,
            IGarnerOrderContractFileServices garnerOrderContractFileServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _garnerOrderPaymentEFRepository = new GarnerOrderPaymentEFRepository(dbContext, logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
            _garnerNotificationServices = garnerNotificationServices;
            _garnerDistributionEFRepository = new GarnerDistributionEFRepository(dbContext, logger);
            _garnerProductEFRepository = new GarnerProductEFRepository(dbContext, logger);
            _garnerOrderContractFileServices = garnerOrderContractFileServices;
        }

        /// <summary>
        /// Thêm mới thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public GarnerOrderPaymentDto Add(CreateGarnerOrderPaymentDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(input)}, username={username}, tradingProviderId = {tradingProviderId}");

            var transaction = _dbContext.Database.BeginTransaction();
            var insert = AddOrderPaymentCommon(input);
            _dbContext.SaveChanges();
            transaction.Commit();
            var result = _mapper.Map<GarnerOrderPaymentDto>(insert);
            return result;
        }

        /// <summary>
        /// Thêm OrderPayment (dùng không transaction)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public GarnerOrderPayment AddOrderPaymentCommon(CreateGarnerOrderPaymentDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderQuery = _garnerOrderEFRepository.FindById(input.OrderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
            // Kiểm tra có phải lần thanh toán đầu tiên
            var orderPaymentFirstQuery = _garnerOrderPaymentEFRepository.Entity.FirstOrDefault(o => o.OrderId == input.OrderId && o.TranType == TranTypes.THU && o.PaymentType == PaymentTypes.CHUYEN_KHOAN
                                            && o.TranClassify == TranClassifies.THANH_TOAN && o.Status != OrderPaymentStatus.HUY_THANH_TOAN && o.Deleted == YesNo.NO);

            if (orderPaymentFirstQuery != null && input.PaymentType == PaymentTypes.CHUYEN_KHOAN && input.TranClassify == TranClassifies.THANH_TOAN)
            {
                // Nếu ngân hàng khác với tài khoản ngân hàng đã tạo thanh toán trước đó
                if (input.TradingBankAccId != orderPaymentFirstQuery.TradingBankAccId)
                {
                    _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderPaymentTradingBankAcc);
                }
                orderQuery.TradingBankAccId = input.TradingBankAccId;
            }
            var insert = _garnerOrderPaymentEFRepository.Add(_mapper.Map<GarnerOrderPayment>(input), username, tradingProviderId);
            _dbContext.SaveChanges();
            return insert;
        }

        /// <summary>
        /// Cập nhật thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public GarnerOrderPayment Update(UpdateGarnerOrderPaymentDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(input)}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderPayment = _garnerOrderPaymentEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.Deleted == YesNo.NO);
            if (orderPayment == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderPaymentNotFound);
            }
            var updateOrderPayment = _garnerOrderPaymentEFRepository.Update(_mapper.Map<GarnerOrderPayment>(input), username, tradingProviderId);
            _dbContext.SaveChanges();
            return updateOrderPayment;
        }

        /// <summary>
        /// Xóa thanh toán
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="FaultException"></exception>
        public void Delete(int id)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: Id: {id}");

            //Lấy thông tin đối tác
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var orderPayment = _garnerOrderPaymentEFRepository.Entity.FirstOrDefault(x => x.Id == id);
            if (orderPayment == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderPaymentNotFound);
            }
            orderPayment.Deleted = YesNo.YES;
            orderPayment.ModifiedBy = username;
            orderPayment.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm thanh toán theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GarnerOrderPaymentDto FindById(int id)
        {
            var result = new GarnerOrderPaymentDto();
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: Id: {id}");
            var orderPayment = _garnerOrderPaymentEFRepository.Entity.FirstOrDefault(x => x.Id == id && x.Deleted == YesNo.NO);
            if (orderPayment == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderPaymentNotFound);
            }
            result = _mapper.Map<GarnerOrderPaymentDto>(orderPayment);
            var tradingBankAccount = _businessCustomerEFRepository.FindBankById(orderPayment.TradingBankAccId ?? 0);
            if (tradingBankAccount == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CoreBusinessCustomerBankOfTradingNotFound);
            }
            result.TradingBankAccount = tradingBankAccount;
            return result;
        }

        /// <summary>
        /// Danh sách thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<GarnerOrderPaymentDto> FindAll(FilterOrderPaymentDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input: {input}");
            var result = new PagingResult<GarnerOrderPaymentDto>();
            var resultItems = new List<GarnerOrderPaymentDto>();
            var orderPayment = _garnerOrderPaymentEFRepository.FindAll(input);
            foreach (var item in orderPayment.Items)
            {
                var orderPaymentItem = new GarnerOrderPaymentDto();
                orderPaymentItem = _mapper.Map<GarnerOrderPaymentDto>(item);
                var tradingBankAccount = _businessCustomerEFRepository.FindBankById(item.TradingBankAccId ?? 0);
                if (tradingBankAccount != null)
                {
                    orderPaymentItem.TradingBankAccount = tradingBankAccount;
                }
                resultItems.Add(orderPaymentItem);
            }
            result.Items = resultItems;
            result.TotalItems = orderPayment.TotalItems;
            return result;
        }

        /// <summary>
        /// Duyệt thanh toán, hủy thanh toán
        /// duyệt thanh toán nếu đủ tiền thì update trạng thái order thành chờ duyệt hợp đồng, nếu k đủ tiền thì về chờ thanh toán
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task ApprovePayment(long id, int status)
        {
            _logger.LogInformation($"{nameof(ApprovePayment)}: Id: {id}; Status: {status}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderPayment = _garnerOrderPaymentEFRepository.FindById(id, tradingProviderId).ThrowIfNull<GarnerOrderPayment>(_dbContext, ErrorCode.GarnerOrderPaymentNotFound);

            if (orderPayment.TranDate > DateTime.Now)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderPaymentApproveDateFuture);
            }

            // Lấy thanh toán được tạo mới nhất
            var orderPaymentNearest = _garnerOrderPaymentEFRepository.GetFirstPayment(orderPayment.OrderId);

            var order = _garnerOrderEFRepository.FindById(orderPayment.OrderId).ThrowIfNull<GarnerOrder>(_dbContext, ErrorCode.GarnerOrderNotFound);

            if (order.Status == OrderStatus.KHOI_TAO || order.Status == OrderStatus.CHO_KY_HOP_DONG
                || order.Status == OrderStatus.CHO_THANH_TOAN || order.Status == OrderStatus.CHO_DUYET_HOP_DONG || order.Status == OrderStatus.DANG_DAU_TU)
            {
                // Get TotalValue From BondOrder
                var totalValue = order.TotalValue;
                var totalPaymentValue = _garnerOrderPaymentEFRepository.Entity.Where(x => x.OrderId == order.Id && x.Status == OrderPaymentStatus.DA_THANH_TOAN
                                        && x.TranType == TranTypes.THU && x.Deleted == YesNo.NO).Sum(op => op.PaymentAmount);

                // duyệt, nếu số tiền đủ -> chuyển trạng thái order sang chờ ký nếu không đủ hoặc thừa chuyển về chờ thanh toán
                if (status == OrderPaymentStatus.DA_THANH_TOAN)
                {
                    orderPayment.Status = OrderPaymentStatus.DA_THANH_TOAN;
                    orderPayment.ApproveBy = username;
                    orderPayment.ApproveDate = DateTime.Now;
                    // So sánh giá trị thanh toán, nếu đủ thì thêm vào ngày thanh toán đủ, không thì null
                    if ((totalPaymentValue + orderPayment.PaymentAmount) >= totalValue)
                    {
                        order.Status = OrderStatus.CHO_DUYET_HOP_DONG;
                        order.PaymentFullDate = orderPaymentNearest?.TranDate;
                        // order.InvestDate = 
                        await _garnerNotificationServices.SendNotifyGarnerApprovePayment(id);
                    }
                    else
                    {
                        order.Status = OrderStatus.CHO_THANH_TOAN;
                        order.PaymentFullDate = null;
                        order.InvestDate = null;
                    }
                }
                else if (status == OrderPaymentStatus.HUY_THANH_TOAN)
                {
                    // So sánh giá trị thanh toán, nếu đủ thì thêm vào ngày thanh toán đủ, không thì null
                    if ((totalPaymentValue - orderPayment.PaymentAmount) >= totalValue)
                    {
                        // Đổi trạng thái của sổ lệnh
                        order.Status = OrderStatus.CHO_DUYET_HOP_DONG;
                        order.PaymentFullDate = orderPaymentNearest?.TranDate;
                    }
                    else
                    {
                        // Đổi trạng thái của sổ lệnh
                        order.Status = OrderStatus.CHO_THANH_TOAN;
                        order.PaymentFullDate = null;
                        order.InvestDate = null;
                    }

                    orderPayment.Status = OrderPaymentStatus.HUY_THANH_TOAN;
                    orderPayment.CancelBy = username;
                    orderPayment.CancelDate = DateTime.Now;
                }

                //Get product
                var product = _garnerProductEFRepository.FindById(order?.ProductId ?? 0);

                //Get Distribution
                var distribution = _garnerDistributionEFRepository.FindById(order.DistributionId);

                //đếm tiền các lệnh đã đầu tư hoặc đang chờ duyệt hợp đồng
                var sumMoneyOrder = _garnerOrderEFRepository.SumValue(order?.DistributionId ?? 0, null);

                var check = product?.InvTotalInvestment;
                var hanMucDauTu = (product?.InvTotalInvestment == null || product?.InvTotalInvestment == 0) ? ((product?.CpsQuantity ?? 0) * (product?.CpsParValue ?? 0)) : product?.InvTotalInvestment;
                if (sumMoneyOrder >= hanMucDauTu)
                {
                    distribution.IsShowApp = YesNo.NO;
                }
                _dbContext.SaveChanges();
                //cập nhật lại hợp đồng
                await _garnerOrderContractFileServices.UpdateContractFile(order.Id);
            }
        }
    }
}
