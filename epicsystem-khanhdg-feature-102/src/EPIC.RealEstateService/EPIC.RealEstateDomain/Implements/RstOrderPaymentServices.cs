using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Notification.Services;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOrderPayment;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EPIC.CoreRepositoryExtensions;
using EPIC.Utils.DataUtils;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstOrderPaymentServices : IRstOrderPaymentServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstOrderPaymentServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RealEstateNotificationServices _rstNotificationServices;
        private readonly IRstSignalRBroadcastServices _rstSignalRBroadcastServices;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly RstOrderPaymentEFRepository _rstOrderPaymentEFRepository;
        private readonly RstOrderEFRepository _rstOrderEFRepository;
        private readonly RstOpenSellBankEFRepository _rstOpenSellBankEFRepository;
        private readonly RstDistributionBankEFRepository _rstDistributionBankEFRepository;
        private readonly RstOpenSellDetailEFRepository _rstOpenSellDetailEFRepository;
        private readonly RstProductItemEFRepository _rstProductItemEFRepository;
        private readonly RstOpenSellEFRepository _rstOpenSellEFRepository;
        private readonly RstHistoryUpdateEFRepository _rstHistoryUpdateEFRepository;

        public RstOrderPaymentServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstOrderPaymentServices> logger,
            IHttpContextAccessor httpContextAccessor,
            RealEstateNotificationServices rstNotificationServices,
            IRstSignalRBroadcastServices rstSignalRBroadcastServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _rstNotificationServices = rstNotificationServices;
            _rstSignalRBroadcastServices = rstSignalRBroadcastServices;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _rstOrderPaymentEFRepository = new RstOrderPaymentEFRepository(dbContext, logger);
            _rstOrderEFRepository = new RstOrderEFRepository(dbContext, logger);
            _rstOpenSellBankEFRepository = new RstOpenSellBankEFRepository(dbContext, logger);
            _rstDistributionBankEFRepository = new RstDistributionBankEFRepository(dbContext, logger);
            _rstOpenSellDetailEFRepository = new RstOpenSellDetailEFRepository(dbContext, logger);
            _rstProductItemEFRepository = new RstProductItemEFRepository(dbContext, logger);
            _rstOpenSellEFRepository = new RstOpenSellEFRepository(dbContext, logger);
            _rstHistoryUpdateEFRepository = new RstHistoryUpdateEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstOrderPayment Add(CreateRstOrderPaymentDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");
            //Kiểm tra nếu là thanh toán bằng chuyển khoản thì phải bắt buộc nhập ngân hàng
            if (input.PaymentType == PaymentTypes.CHUYEN_KHOAN && (input.TradingBankAccountId == null && input.PartnerBankAccountId == null))
            {
                _rstOrderPaymentEFRepository.ThrowException(ErrorCode.RstOrderPaymentBankIsNotEmpty);
            }

            // Kiểm tra ngân hàng thanh toán
            CheckBankPayment(input.OrderId, tradingProviderId, input.TradingBankAccountId, input.PartnerBankAccountId);

            // Lịch sử thêm thanh toán
            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.OrderId, null, null, null,
                    RstHistoryUpdateTables.RST_ORDER_PAYMENT, ActionTypes.THEM_MOI, "Thêm thanh toán ", DateTime.Now), username);

            // Thêm thanh toán
            var insert = _rstOrderPaymentEFRepository.Add(_mapper.Map<RstOrderPayment>(input), username, tradingProviderId);
            _dbContext.SaveChanges();
            return insert;
        }

        /// <summary>
        /// Cập nhật thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstOrderPayment Update(UpdateRstOrderPaymentDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");
            var orderPayment = _rstOrderPaymentEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.Deleted == YesNo.NO)
                .ThrowIfNull<RstOrderPayment>(_dbContext, ErrorCode.RstOrderPaymentNotFound);
            CheckBankPayment(input.OrderId, tradingProviderId, input.TradingBankAccountId, input.PartnerBankAccountId);

            // Lịch sử chỉnh sửa thanh toán
            if(input.TranDate != orderPayment.TranDate)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(orderPayment.OrderId, orderPayment.TranDate?.Date.ToString(), input.TranDate?.Date.ToString(), RstFieldName.UPDATE_ORDER_PAYMNET_TRAN_DATE,
                        RstHistoryUpdateTables.RST_ORDER_PAYMENT, ActionTypes.CAP_NHAT, "Ngày giao dịch", DateTime.Now), username);
            }

            if(input.PaymentType != orderPayment.PaymentType)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(orderPayment.OrderId, RstPaymentType.PaymentType(orderPayment.PaymentType), RstPaymentType.PaymentType(input.PaymentType), RstFieldName.UPDATE_ORDER_PAYMNET_TYPE,
                        RstHistoryUpdateTables.RST_ORDER_PAYMENT, ActionTypes.CAP_NHAT, "Loại thanh toán", DateTime.Now), username);
            }

            if (input.PaymentAmount != orderPayment.PaymentAmount)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(orderPayment.OrderId, orderPayment.PaymentAmount + "", input.PaymentAmount + "", RstFieldName.UPDATE_ORDER_PAYMNET_AMOUNT,
                        RstHistoryUpdateTables.RST_ORDER_PAYMENT, ActionTypes.CAP_NHAT, "Số tiền", DateTime.Now), username);
            }

            if (input.Description != orderPayment.Description)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(orderPayment.OrderId, orderPayment.Description, input.Description, RstFieldName.UPDATE_ORDER_PAYMNET_DESCRIPTION,
                        RstHistoryUpdateTables.RST_ORDER_PAYMENT, ActionTypes.CAP_NHAT, "Mô tả", DateTime.Now), username);
            }

            var updateOrderPayment = _rstOrderPaymentEFRepository.Update(_mapper.Map<RstOrderPayment>(input), username, tradingProviderId);
            _dbContext.SaveChanges();
            return updateOrderPayment;
        }

        /// <summary>
        /// Phê duyệt hoặc hủy duyệt thanh toán
        /// </summary>
        public async Task ApproveOrCancel(int orderPaymentId, int status)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ApproveOrCancel)}: orderPaymentId = {orderPaymentId}, username = {username}, tradingProviderId = {tradingProviderId}");

            var transaction = _dbContext.Database.BeginTransaction();

            var orderPayment = _rstOrderPaymentEFRepository.FindById(orderPaymentId, tradingProviderId)
                .ThrowIfNull<RstOrderPayment>(_dbContext, ErrorCode.RstOrderPaymentNotFound);
            var orderQuery = _rstOrderEFRepository.FindById(orderPayment.OrderId)
                .ThrowIfNull(_dbContext, ErrorCode.RstOrderNotFound);
            var productItem = _rstProductItemEFRepository.FindById(orderQuery.ProductItemId)
                .ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
            var openSellDetail = _rstOpenSellDetailEFRepository.FindById(orderQuery.OpenSellDetailId)
                .ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            var openSell = _rstOpenSellEFRepository.FindById(openSellDetail.OpenSellId)
                .ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            var cifCode = _dbContext.CifCodes.FirstOrDefault(o => o.CifCode == orderQuery.CifCode);
            var investorFind = _dbContext.Investors.FirstOrDefault(o => o.InvestorId == cifCode.InvestorId);
            var phone = investorFind.Phone;
            var paymentAmount = orderPayment.PaymentAmount;

            // Lấy giá cọc, giá lock căn của hợp đồng
            var productItemPrice = _rstProductItemEFRepository.ProductItemPriceByPolicy(productItem.Price ?? 0, orderQuery.DistributionPolicyId);
            // Tổng số tiền đã được duyệt
            var sumOrderPaymentApprove = _rstOrderPaymentEFRepository.Entity.Where(p => p.OrderId == orderQuery.Id && p.Deleted == YesNo.NO && p.TranClassify == TranClassifies.THANH_TOAN
                                                && p.TranType == TranTypes.THU && p.Status == OrderPaymentStatus.DA_THANH_TOAN).Sum(p => p.PaymentAmount);

            // Hợp đồng đang bị phong tỏa thì không thể thao tác thêm
            if (orderQuery.Status == RstOrderStatus.PHONG_TOA)
            {
                _rstOrderPaymentEFRepository.ThrowException(ErrorCode.RstOrderStatusBlockadeCanNotOperation);
            }
            int openSellDetailStatusInit = openSellDetail.Status;
            var isLockPrice = false;
            if (orderPayment.Status == OrderPaymentStatus.NHAP && status == OrderPaymentStatus.DA_THANH_TOAN)
            {
                // Kiểm tra và cập nhật trạng thái hợp đồng nếu đủ số tiền cọc
                if (sumOrderPaymentApprove + orderPayment.PaymentAmount >= orderQuery.DepositMoney)
                {
                    orderQuery.Status = RstOrderStatus.CHO_DUYET_HOP_DONG_COC;
                    isLockPrice = true;
                }
                else
                {
                    orderQuery.Status = RstOrderStatus.CHO_THANH_TOAN_COC;
                }

                orderQuery.StatusMax = (orderQuery.Status > orderQuery.StatusMax) ? orderQuery.Status : orderQuery.StatusMax;
                // Căn hộ chưa được khóa căn, nếu hợp đồng có đủ hoặc thừa số tiền khóa căn thì cho Khóa căn
                if ((productItem.Status == RstProductItemStatus.KHOI_TAO || productItem.Status == RstProductItemStatus.GIU_CHO)
                    && sumOrderPaymentApprove + orderPayment.PaymentAmount >= productItemPrice.LockPrice)
                {
                    productItem.Status = RstProductItemStatus.KHOA_CAN;
                    openSellDetail.Status = RstProductItemStatus.KHOA_CAN;
                    isLockPrice = true;
                    //_rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(productItem.Id, "Giữ chỗ", "Khoá căn", "Trạng thái", RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, $"{RstHistoryUpdateSummary.LOCK} cho KH: {phone}", DateTime.Now, RstHistoryTypes.KhoaCan), username);
                    //_rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(openSellDetail.Id, "Giữ chỗ", "Khoá căn", "Trạng thái", RstHistoryUpdateTables.RST_OPEN_SELL_DETAIL, ActionTypes.CAP_NHAT, $"{RstHistoryUpdateSummary.LOCK} cho KH: {phone}", DateTime.Now, RstHistoryTypes.KhoaCan), username);
                }
                // Trường hợp căn đã khóa thì check xem khóa căn có phải là do hợp đồng hay không
                else if (productItem.Status == RstProductItemStatus.KHOA_CAN && (openSellDetail.Status != RstProductItemStatus.KHOA_CAN
                    || (openSellDetail.Status == RstProductItemStatus.KHOA_CAN && sumOrderPaymentApprove < productItemPrice.LockPrice)))
                {
                    _rstOrderPaymentEFRepository.ThrowException(ErrorCode.RstOrderPaymentCanNotApproveCuzProductItemLockNotOfOrder);
                }
                orderPayment.ApproveBy = username;
                orderPayment.ApproveDate = DateTime.Now;
            }
            else if (orderPayment.Status != OrderPaymentStatus.HUY_THANH_TOAN && status == OrderPaymentStatus.HUY_THANH_TOAN)
            {
                // Tính tổng số tiền xem đã đủ số tiền cọc chưa
                if (orderPayment.Status == OrderPaymentStatus.DA_THANH_TOAN && sumOrderPaymentApprove - orderPayment.PaymentAmount >= orderQuery.DepositMoney)
                {
                    orderQuery.Status = RstOrderStatus.CHO_DUYET_HOP_DONG_COC;
                    //isFullPayment = true;
                }
                else
                {
                    orderQuery.Status = RstOrderStatus.CHO_THANH_TOAN_COC;
                }

                // Hợp đồng chưa từng đến trạng thái đã cọc thì được phép update lại trạng thái
                // Căn hộ chưa được khóa căn, nếu hợp đồng có đủ hoặc thừa số tiền giữ chỗ thì cho Khóa căn
                if (orderPayment.Status == OrderPaymentStatus.DA_THANH_TOAN && orderQuery.StatusMax < RstOrderStatus.DA_COC 
                    && (productItem.Status == RstProductItemStatus.KHOA_CAN && openSellDetail.Status == RstProductItemStatus.KHOA_CAN)
                    && sumOrderPaymentApprove >= productItemPrice.LockPrice && sumOrderPaymentApprove - orderPayment.PaymentAmount < productItemPrice.LockPrice)
                {
                    if (orderQuery.ExpTimeDeposit != null && orderQuery.ExpTimeDeposit > DateTime.Now)
                    {
                        productItem.Status = RstProductItemStatus.GIU_CHO;
                        openSellDetail.Status = RstProductItemStatus.GIU_CHO;
                        //_rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(productItem.Id, null, null, null, RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, $"{RstHistoryUpdateSummary.HOLD} - KH: {phone}", DateTime.Now, RstHistoryTypes.GiuCho), username);
                        //_rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(openSellDetail.Id, null, null, null, RstHistoryUpdateTables.RST_OPEN_SELL_DETAIL, ActionTypes.CAP_NHAT, $"{RstHistoryUpdateSummary.HOLD} - KH: {phone}", DateTime.Now, RstHistoryTypes.GiuCho), username);
                    }
                    else
                    {
                        productItem.Status = RstProductItemStatus.KHOI_TAO;
                        openSellDetail.Status = RstProductItemStatus.KHOI_TAO;
                        _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(productItem.Id, null, null, null, RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.INITIALIZE, DateTime.Now, RstHistoryTypes.KhoiTao), username);
                        _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(openSellDetail.Id, null, null, null, RstHistoryUpdateTables.RST_OPEN_SELL_DETAIL, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.INITIALIZE, DateTime.Now, RstHistoryTypes.KhoiTao), username);
                    }
                }
                orderPayment.CancelBy = username;
                orderPayment.CancelDate = DateTime.Now;
            }

            var oldValue = RstOrderPaymentStatus.PaymentStatus(orderPayment.Status);
            var newValue = RstOrderPaymentStatus.PaymentStatus((orderPayment.Status == OrderPaymentStatus.NHAP) ? OrderPaymentStatus.DA_THANH_TOAN : OrderPaymentStatus.HUY_THANH_TOAN);

            string oldValueProductItem = (isLockPrice && productItem.Status == RstProductItemStatus.KHOA_CAN) ? "Khoá căn" : "Giữ chỗ";
            string newValueProductItem = (isLockPrice) ? "Khoá căn" : "Giữ chỗ";

            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(orderPayment.OrderId, oldValue, newValue, null,
                        RstHistoryUpdateTables.RST_ORDER_PAYMENT, ActionTypes.CAP_NHAT, (orderPayment.Status == OrderPaymentStatus.NHAP) ? $"Phê duyệt thanh toán: {NumberToText.ConvertNumberIS((double)paymentAmount)}" : $"Hủy thanh toán", DateTime.Now), username);
            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(productItem.Id, oldValueProductItem, newValueProductItem, "Trạng thái", 
                        RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, (orderPayment.Status == OrderPaymentStatus.NHAP) ? $"KH: {phone} thanh toán: {NumberToText.ConvertNumberIS((double)paymentAmount)}" : $"KH: {phone} hủy thanh toán", DateTime.Now, (orderPayment.Status == OrderPaymentStatus.NHAP) ? RstHistoryTypes.ThanhToan : null), username);
            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(openSellDetail.Id, oldValueProductItem, newValueProductItem, "Trạng thái",
                        RstHistoryUpdateTables.RST_OPEN_SELL_DETAIL, ActionTypes.CAP_NHAT, (orderPayment.Status == OrderPaymentStatus.NHAP) ? $"KH: {phone} thanh toán: {NumberToText.ConvertNumberIS((double)paymentAmount)}" : $"KH: {phone} hủy thanh toán", DateTime.Now), username);
            // Cập nhật trạng thái của thanh toán 
            orderPayment.Status = status;
            
            _dbContext.SaveChanges();
            transaction.Commit();
            // Duyệt/Hủy thanh toán thì đếm số căn hộ phát sinh thanh toán
            if (orderQuery.ExpTimeDeposit > DateTime.Now && productItem.Status == RstProductItemStatus.GIU_CHO)
            {
                await _rstSignalRBroadcastServices.BroadcatProductItemHasPaymentOrder(orderQuery.ProductItemId, openSell.Id);
            }    
            // Trường hợp thanh toán có thay đổi trạng thái sản phẩm của mở bán
            if (openSellDetailStatusInit != openSellDetail.Status)
            {
                await _rstSignalRBroadcastServices.BroadcastOpenSellDetailChangeStatus(openSellDetail.Id);
            }    
            // Gửi thông thông báo chuyển tiền thành công
            if (status == OrderPaymentStatus.DA_THANH_TOAN)
            {
                await _rstNotificationServices.SendNotifyRstApprovePayment(orderPaymentId);
            }
        }

        /// <summary>
        /// Xoá thanh toán
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Delete)}: id = {id}");

            //Lấy thông tin đối tác
            var orderPayment = _rstOrderPaymentEFRepository.Entity.FirstOrDefault(x => x.Id == id)
                .ThrowIfNull<RstOrderPayment>(_dbContext, ErrorCode.RstOrderPaymentNotFound);
            // Hợp đồng đang ở trạng thái đã thanh toán thì ko xóa được, hủy duyệt để xóa
            if (orderPayment.Status == OrderPaymentStatus.DA_THANH_TOAN)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.RstOrderPaymentCanNotDelete);
            }

            // Lịch sử xóa thanh toán
            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(orderPayment.OrderId, null, null, null,
                    RstHistoryUpdateTables.RST_ORDER_PAYMENT, ActionTypes.XOA, "Xóa thanh toán ", DateTime.Now), username);

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
        public RstOrderPaymentDto FindById(int id)
        {
            var result = new RstOrderPaymentDto();
            _logger.LogInformation($"{nameof(FindById)}: id = {id}");

            var orderPayment = _rstOrderPaymentEFRepository.Entity.FirstOrDefault(x => x.Id == id && x.Deleted == YesNo.NO)
                .ThrowIfNull<RstOrderPayment>(_dbContext, ErrorCode.RstOrderPaymentNotFound);
            result = _mapper.Map<RstOrderPaymentDto>(orderPayment);
            var tradingBankAccount = _businessCustomerEFRepository.FindBankById(orderPayment.TradingBankAccountId ?? 0);
            if (tradingBankAccount != null)
            {
                result.BankAccount = tradingBankAccount;
            }
            var partnerBankAccount = _businessCustomerEFRepository.FindBankById(orderPayment.PartnerBankAccountId ?? 0);
            if (partnerBankAccount != null)
            {
                result.BankAccount = partnerBankAccount;
            }
            return result;
        }

        /// <summary>
        /// Tìm danh sách thanh toán phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<RstOrderPaymentDto> FindAll(FilterRstOrderPaymentDto input)
        {
            int? tradingProviderId = null;
            int? partnerId = null;
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            if (usertype == UserTypes.TRADING_PROVIDER || usertype == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (usertype == UserTypes.PARTNER || usertype == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                _dbContext.CheckTradingRelationshipPartner(partnerId, input.TradingProviderIds);
            }
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            var result = new PagingResult<RstOrderPaymentDto>();
            var resultItems = new List<RstOrderPaymentDto>();
            var orderPayment = _rstOrderPaymentEFRepository.FindAll(input, tradingProviderId);
            foreach (var item in orderPayment.Items)
            {
                var orderPaymentItem = new RstOrderPaymentDto();
                orderPaymentItem = _mapper.Map<RstOrderPaymentDto>(item);
                var tradingBankAccount = _businessCustomerEFRepository.FindBankById(item.TradingBankAccountId ?? 0);
                if (tradingBankAccount != null)
                {
                    orderPaymentItem.BankAccount = tradingBankAccount;
                }
                var partnerBankAccount = _businessCustomerEFRepository.FindBankById(item.PartnerBankAccountId ?? 0);
                if (partnerBankAccount != null)
                {
                    orderPaymentItem.BankAccount = partnerBankAccount;
                }
                resultItems.Add(orderPaymentItem);
            }
            result.Items = resultItems;
            result.TotalItems = orderPayment.TotalItems;
            return result;
        }

        /// <summary>
        /// Kiểm tra ngân hàng thanh toán của hợp đồng
        /// Nếu đã có thanh toán trước đó thì tài khoản thanh toán lần sau phải giống với lần trước đó
        /// </summary>
        public void CheckBankPayment(int orderId, int tradingProviderId, int? tradingBankAccountId, int? partnerBankAccountId)
        {
            var orderQuery = _rstOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.RstOrderNotFound);

            // Hợp đồng đang bị phong tỏa thì không thể thao tác thêm
            if (orderQuery.Status == RstOrderStatus.PHONG_TOA)
            {
                _rstOrderPaymentEFRepository.ThrowException(ErrorCode.RstOrderStatusBlockadeCanNotOperation);
            }

            // Kiểm tra có phải lần thanh toán đầu tiên chưa
            var orderPaymentFirstQuery = _rstOrderPaymentEFRepository.Entity.FirstOrDefault(o => o.OrderId == orderId && o.TranType == TranTypes.THU
                                        && o.TranClassify == TranClassifies.THANH_TOAN && o.PaymentType == PaymentTypes.CHUYEN_KHOAN
                                        && o.Status != OrderPaymentStatus.HUY_THANH_TOAN && o.Deleted == YesNo.NO);
            // Nếu đã có thanh toán kiểm tra xem ngân hàng nhận tiền có giống lần đầu không
            if (orderPaymentFirstQuery != null)
            {
                if (tradingBankAccountId != orderPaymentFirstQuery.TradingBankAccountId)
                {
                    _defErrorEFRepository.ThrowException(ErrorCode.RstOrderPaymentTradingBankAcc);
                }
                if (partnerBankAccountId != orderPaymentFirstQuery.PartnerBankAccountId)
                {
                    _defErrorEFRepository.ThrowException(ErrorCode.RstOrderPaymentTradingBankAcc);
                }
            }
            // Nếu thanh toán lần đầu kiểm tra xem ngân hàng xem hợp lệ không
            else
            {
                var projectItemQuery = _dbContext.RstProductItems.FirstOrDefault(r => r.Id == orderQuery.ProductItemId && r.Deleted == YesNo.NO)
                    .ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound); 
                var openSellDetailQuery = _dbContext.RstOpenSellDetails.FirstOrDefault(r => r.Id == orderQuery.OpenSellDetailId && r.Deleted == YesNo.NO)
                    .ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);

                // Thanh toán lần đầu tiên nếu hết hạn đặt cọc thì căn hộ và sản phẩm của mở bán phải ở khởi tạo mới cho tạo thanh toán
                if (projectItemQuery.Status != RstProductItemStatus.KHOI_TAO && openSellDetailQuery.Status != RstProductItemStatus.KHOI_TAO 
                    && (orderQuery.ExpTimeDeposit == null || (orderQuery.ExpTimeDeposit != null && orderQuery.ExpTimeDeposit < DateTime.Now)))
                {
                    _rstOrderPaymentEFRepository.ThrowException(ErrorCode.RstOrderPaymentCanNotInsertCuzProductItemStatusInvalid);
                }    

                if (tradingBankAccountId != null)
                {
                    _rstOpenSellBankEFRepository.CheckTradingBankAccount(tradingProviderId, tradingBankAccountId ?? 0);
                }
                if (partnerBankAccountId != null)
                {
                    _rstDistributionBankEFRepository.CheckPartnerBankAccount(projectItemQuery.ProjectId, partnerBankAccountId ?? 0);
                }
                orderQuery.TradingBankAccountId = tradingBankAccountId;
                orderQuery.PartnerBankAccountId = partnerBankAccountId;
            }
        }
    }
}
