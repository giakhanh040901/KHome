using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.CoreRepositoryExtensions;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.InterestPayment;
using EPIC.InvestRepositories;
using EPIC.InvestSharedDomain.Interfaces;
using EPIC.MSB.ConstVariables;
using EPIC.MSB.Dto.PayMoney;
using EPIC.MSB.Services;
using EPIC.Notification.Services;
using EPIC.PaymentEntities.DataEntities;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using EPIC.PaymentEntities.Dto.MsbRequestPaymentDetail;
using EPIC.PaymentRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Payment;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace EPIC.InvestDomain.Implements
{
    public class InterestPaymentServices : IInterestPaymentServices
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly MsbPayMoneyServices _msbPayMoneyServices;
        private readonly InvestInterestPaymentRepository _interestPaymentRepository;
        private readonly InvestOrderRepository _orderRepository;
        private readonly InvestOrderPaymentRepository _orderPaymentRepository;
        private readonly InvestPolicyRepository _policyRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly InvRenewalsRequestRepository _renewalsRequestRepository;
        private readonly DistributionRepository _distributionRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly InvestInterestPaymentEFRepository _interestPaymentEFRepository;
        private readonly MsbRequestPaymentEFRepository _msbRequestPaymentEFRepository;
        private readonly MsbRequestPaymentDetailEFRepository _msbRequestPaymentDetailEFRepository;
        private readonly BusinessCustomerBankEFRepository _businessCustomerBankEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly InvestorBankAccountEFRepository _investorBankAccountEFRepository;
        private readonly BankEFRepository _bankEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly InvestOrderEFRepository _invOrderEFRepository;
        private readonly InvestOrderPaymentEFRepository _invOrderPaymentEFRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IInvestSharedServices _investSharedServices;
        private readonly OrderContractFileRepository _orderContractFileRepository;
        private readonly IContractTemplateServices _contractTemplateServices;
        private readonly IContractDataServices _contractDataServices;
        private readonly IInvestOrderContractFileServices _investOrderContractFileServices;
        private readonly NotificationServices _sendEmailServices;
        private readonly InvestNotificationServices _investSendEmailServices;
        private readonly IMapper _mapper;
        private readonly IInvestContractCodeServices _investContractCodeServices;
        public InterestPaymentServices(ILogger<InterestPaymentServices> logger,
            IWebHostEnvironment env,
            EpicSchemaDbContext dbContext,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IInvestSharedServices investSharedServices,
            IContractTemplateServices contractTemplateServices,
            IContractDataServices contractDataServices,
            IInvestOrderContractFileServices investOrderContractFileServices,
            NotificationServices sendEmailServices,
            InvestNotificationServices investSendEmailServices,
            MsbPayMoneyServices msbPayMoneyServices,
            IMapper mapper,
            IInvestContractCodeServices investContractCodeServices)
        {
            _env = env;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _dbContext = dbContext;
            _msbPayMoneyServices = msbPayMoneyServices;
            _interestPaymentRepository = new InvestInterestPaymentRepository(_connectionString, _logger);
            _orderRepository = new InvestOrderRepository(_connectionString, _logger);
            _orderPaymentRepository = new InvestOrderPaymentRepository(_connectionString, _logger);
            _policyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _renewalsRequestRepository = new InvRenewalsRequestRepository(_connectionString, _logger);
            _distributionRepository = new DistributionRepository(_connectionString, _logger);
            _projectRepository = new ProjectRepository(_connectionString, _logger);
            _interestPaymentEFRepository = new InvestInterestPaymentEFRepository(_dbContext, _logger);
            _msbRequestPaymentEFRepository = new MsbRequestPaymentEFRepository(dbContext, logger);
            _msbRequestPaymentDetailEFRepository = new MsbRequestPaymentDetailEFRepository(dbContext, logger);
            _businessCustomerBankEFRepository = new BusinessCustomerBankEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _investorBankAccountEFRepository = new InvestorBankAccountEFRepository(dbContext, logger);
            _bankEFRepository = new BankEFRepository(dbContext, logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _invOrderEFRepository = new InvestOrderEFRepository(dbContext, logger);
            _invOrderPaymentEFRepository = new InvestOrderPaymentEFRepository(dbContext, logger);
            _httpContext = httpContext;
            _investSharedServices = investSharedServices;
            _orderContractFileRepository = new OrderContractFileRepository(_connectionString, _logger);
            _contractTemplateServices = contractTemplateServices;
            _contractDataServices = contractDataServices;
            _investOrderContractFileServices = investOrderContractFileServices;
            _sendEmailServices = sendEmailServices;
            _investSendEmailServices = investSendEmailServices;
            _mapper = mapper;
            _investContractCodeServices = investContractCodeServices;
        }

        public List<InvestInterestPaymentDto> InterestPaymentAdd(InterestPaymentCreateListDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new List<InvestInterestPaymentDto>();
            var transaction = _dbContext.Database.BeginTransaction();

            foreach (var item in input.InterestPayments)
            {
                if (item.PayDate != null && item.PayDate.Value.Date > DateTime.Now.Date)
                {
                    throw new FaultException(new FaultReason($"Ngày chi trả đang lớn hơn ngày hiện tại"), new FaultCode(((int)ErrorCode.InvestInterestPaymentPayDateLargerNowDate).ToString()), "");
                }
                var orderFind = _invOrderEFRepository.FindById(item.OrderId, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);
                var insertInterestPayment = _interestPaymentEFRepository.Add(new InvestInterestPayment
                {
                    TradingProviderId = tradingProviderId,
                    OrderId = item.OrderId,
                    PeriodIndex = item.PeriodIndex,
                    CifCode = item.CifCode,
                    AmountMoney = item.AmountMoney,
                    Profit = item.AmountMoney,
                    Tax = item.Tax,
                    TotalValueInvestment = item.TotalValueInvestment,
                    TotalValueCurrent = orderFind.TotalValue,
                    PolicyDetailId = item.PolicyDetailId,
                    PayDate = item.PayDate,
                    IsLastPeriod = item.IsLastPeriod,
                    CreatedBy = username
                });
                result.Add(_mapper.Map<InvestInterestPaymentDto>(insertInterestPayment));
            }
            _dbContext.SaveChanges();
            transaction.Commit();
            return result;
        }

        public PagingResult<DanhSachChiTraDto> FindAll(InterestPaymentFilterDto input)
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
            // Nếu không phải Parner thì tradingProviderIds = null
            input.TradingProviderIds = (partnerId != null) ? input.TradingProviderIds : null;
            var interestPaymentQuery = _interestPaymentEFRepository.FindAllInterestPayment(input, tradingProviderId);
            var result = new PagingResult<DanhSachChiTraDto>();
            var items = new List<DanhSachChiTraDto>();
            foreach (var paymentItem in interestPaymentQuery.Items)
            {
                var orderFind = _orderRepository.FindById(paymentItem.OrderId, tradingProviderId);
                if (orderFind == null)
                {
                    _logger.LogError($"PagingResult<DanhSachChiTraDto> {nameof(FindAll)}: Không tìm thấy thông tin hợp đồng: orderId = {paymentItem.OrderId}");
                    continue;
                }

                //Lấy thông tin chính sách
                var policyFind = _policyRepository.FindPolicyById(orderFind.PolicyId);
                if (policyFind == null)
                {
                    _logger.LogError($"PagingResult<DanhSachChiTraDto> {nameof(FindAll)}: Không tìm thấy thông tin kỳ hạn: PolicyId = {orderFind.PolicyId}");
                    continue;
                }

                paymentItem.GenContractCode = _invOrderEFRepository.GetContractCodeGen(orderFind.Id);
                paymentItem.PolicyCalculateType = policyFind.CalculateType;

                var cifCodeFind = _cifCodeRepository.GetByCifCode(orderFind.CifCode);
                if (cifCodeFind != null && cifCodeFind.InvestorId != null)
                {
                    paymentItem.InvestorBank = _investorBankAccountEFRepository.GetBankById(orderFind.InvestorBankAccId ?? 0);
                }
                else if (cifCodeFind != null && cifCodeFind.BusinessCustomerId != null)
                {
                    paymentItem.BusinessCustomerBank = _businessCustomerEFRepository.FindBankById(orderFind.InvestorBankAccId ?? 0);
                }

                // Đổ ra thông tin nếu có tái tục
                // Nếu có tái tục thì sẽ lấy thông tin tái tục từ Id được lưu sau khi đã tái tục đầu tư thành hợp đồng mới
                if (paymentItem.IsLastPeriod == YesNo.YES && (InterestPaymentStatus.NHOM_CHUA_CHI_TRA.Contains(paymentItem.InterestPaymentStatus ?? 0)))
                {
                    // Kiểm tra xem có yêu cầu tái tục hay không/ Lấy yêu cầu tái tục mới nhất
                    var renewalsRequestFind = _renewalsRequestRepository.GetByOrderId(paymentItem.OrderId);
                    if (renewalsRequestFind != null)
                    {
                        var renewarsPolicyDetail = _policyRepository.FindPolicyDetailById(renewalsRequestFind.RenewalsPolicyDetailId);
                        if (renewarsPolicyDetail != null)
                        {
                            paymentItem.RenewalsRequest = new();
                            paymentItem.RenewalsRequest.Id = renewalsRequestFind.Id;
                            paymentItem.RenewalsRequest.OrderId = renewalsRequestFind.OrderId;
                            paymentItem.RenewalsRequest.SettlementMethod = renewalsRequestFind.SettlementMethod;
                            paymentItem.RenewalsRequest.RenewalsPolicyDetailId = renewalsRequestFind.RenewalsPolicyDetailId;
                            paymentItem.RenewalsRequest.PolicyDetail = _mapper.Map<PolicyDetailDto>(renewarsPolicyDetail);
                            var renewarsPolicy = _policyRepository.FindPolicyById(renewarsPolicyDetail.PolicyId);
                            if (renewarsPolicy != null)
                            {
                                paymentItem.RenewalsRequest.Policy = _mapper.Map<PolicyDto>(renewarsPolicy);
                            }
                        }

                        // Tổng số tiền đầu tư sau khi tái tục hợp đồng mới
                        decimal initTotalValueOrderNew = orderFind.TotalValue;

                        // Tái tục gốc và lợi tức
                        if (renewalsRequestFind.SettlementMethod == SettlementMethod.TAI_TUC_GOC_VA_LOI_NHUAN)
                        {
                            initTotalValueOrderNew = initTotalValueOrderNew + paymentItem.AmountMoney;
                        }
                        // Hạn mức tối đa của Phân phối
                        decimal totalValueOrderOfDistribution = _orderRepository.MaxTotalInvestment(orderFind.TradingProviderId, renewarsPolicyDetail.DistributionId);
                        if ((totalValueOrderOfDistribution + orderFind.TotalValue) - initTotalValueOrderNew < 0)
                        {
                            // Vượt hạn mức không thể tái tục
                            paymentItem.CanRenewalOrder = YesNo.NO;
                        }
                    }
                }
                // Nếu có tái tục thì sẽ lấy thông tin tái tục từ Id được lưu sau khi đã tái tục đầu tư thành hợp đồng mới
                if (paymentItem.IsLastPeriod == YesNo.YES && (InterestPaymentStatus.NHOM_DA_CHI_TRA.Contains(paymentItem.InterestPaymentStatus ?? 0)))
                {
                    var renewarsPolicyDetail = _policyRepository.FindPolicyDetailById(orderFind.RenewalsPolicyDetailId ?? 0);
                    if (renewarsPolicyDetail != null)
                    {
                        paymentItem.RenewalsRequest = new();
                        paymentItem.RenewalsRequest.OrderId = orderFind.Id;
                        paymentItem.RenewalsRequest.SettlementMethod = orderFind.SettlementMethod ?? 0;
                        paymentItem.RenewalsRequest.RenewalsPolicyDetailId = orderFind.RenewalsPolicyDetailId;
                        paymentItem.RenewalsRequest.PolicyDetail = _mapper.Map<PolicyDetailDto>(renewarsPolicyDetail);
                        var renewarsPolicy = _policyRepository.FindPolicyById(renewarsPolicyDetail.PolicyId);
                        if (renewarsPolicy != null)
                        {
                            paymentItem.RenewalsRequest.Policy = _mapper.Map<PolicyDto>(renewarsPolicy);
                        }
                    }
                }
                items.Add((DanhSachChiTraDto)paymentItem);
            }
            result.TotalItems = interestPaymentQuery.TotalItems;
            result.Items = items;
            return result;
        }

        public InvestInterestPayment FindById(int id)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = _interestPaymentRepository.FindById(id, tradingProviderId);
            if (result == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy danh sách chi trả"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            return result;
        }

        /// <summary>
        /// hủy yêu cầu tái tục 
        /// </summary>
        /// <param name="interestPaymentId"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public void CancelRenewalRequest(List<int> interestPaymentId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                foreach (var id in interestPaymentId)
                {
                    var interestPayment = _interestPaymentRepository.FindById(id, tradingProviderId);

                    // tìm order theo interestPayment
                    var order = _orderRepository.FindById(interestPayment.OrderId);
                    if (order == null)
                    {
                        throw new FaultException(new FaultReason($"Không tìm thấy hợp đồng"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                    }

                    // tìm investRenewalsRequest theo orderId
                    var investRenewalsRequest = _renewalsRequestRepository.GetListByOrderId(order.Id).ToList();

                    // đổi sang trạng thái đã hủy 
                    foreach (var item in investRenewalsRequest)
                    {
                        _renewalsRequestRepository.CancelRequest(item.Id, null);
                    }
                }
                transaction.Complete();
            }
            _interestPaymentRepository.CloseConnection();
        }

        /// <summary>
        /// Chi trả cuối kỳ (có tái tục)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        [Obsolete("Bỏ")]
        public async Task ApprovePaymentLastPeriod(List<int> ids)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var approveIp = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            var result = new InvestInterestPaymentApproveRenewalDto();
            //Sử dụng transaction để thực hiện vòng for lập chi trả khi 1 trong số chi trả có sổ lệnh không hoạt động trả ra exception
            //toàn bộ chi truyền vào ko đc thực hiện
            // Lấy thông tin sổ lệnh, tính đáo hạn, NextWorkDay đưa vào repo để cho dùng được transaction
            var transaction = _interestPaymentRepository.BeginTransaction();
            foreach (var interestPaymentIdItem in ids)
            {
                //Nếu tái tục sau chi trả thì sẽ dùng để đầu tư tiếp
                var ngayDauTuSauTaiTuc = DateTime.Now.Date;

                var interestPaymentFind = _interestPaymentRepository.FindById(interestPaymentIdItem, tradingProviderId, false);
                if (interestPaymentFind == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông chi trả"), new FaultCode(((int)ErrorCode.InvestInterestPaymentNotFound).ToString()), "");
                }
                if (interestPaymentFind.PayDate != null && interestPaymentFind.PayDate.Value.Date > DateTime.Now.Date)
                {
                    throw new FaultException(new FaultReason($"Ngày chi trả đang lớn hơn ngày hiện tại"), new FaultCode(((int)ErrorCode.InvestInterestPaymentPayDateLargerNowDate).ToString()), "");
                }
                var orderFind = _interestPaymentRepository.FindOrderById(interestPaymentFind.OrderId, tradingProviderId, false);
                if (orderFind == null || orderFind.InvestDate == null)
                {
                    throw new FaultException(new FaultReason($"Chi trả tái tục cuối kỳ: Hợp đồng không trong nằm trạng thái đang đầu tư"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                }

                var renewalsRequestFind = _renewalsRequestRepository.GetByOrderId(interestPaymentFind.OrderId, null, false);
                if (renewalsRequestFind == null)
                {
                    throw new FaultException(new FaultReason($"Chi trả tái tục cuối kỳ: Hợp đồng chưa có yêu cầu tái tục"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                }
                else
                {
                    var policyDetailFind = _policyRepository.FindPolicyDetailById(renewalsRequestFind.RenewalsPolicyDetailId, null, false);
                    if (policyDetailFind == null)
                    {
                        throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn sau tái tục"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
                    }
                }

                //Nếu sổ lệnh đang active // Tính ngày đáo hạn của hợp đồng để tái tục mới đầu tư
                if (orderFind != null && orderFind.InvestDate != null && orderFind.Status == OrderStatus.DANG_DAU_TU)
                {
                    //Tìm kiếm kỳ hạn để tìm ngày đáo hạn của order
                    var policyDetailFind = _policyRepository.FindPolicyDetailById(orderFind.PolicyDetailId, null, false);
                    if (policyDetailFind == null)
                    {
                        throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
                    }
                    DateTime ngayDauTu = orderFind.InvestDate.Value.Date;

                    //Tính ngày đáo hạn
                    ngayDauTuSauTaiTuc = _interestPaymentRepository.CalculateDueDate(policyDetailFind, ngayDauTu);
                }

                result = _interestPaymentRepository.InterestPaymentLastPeriodOrder(interestPaymentIdItem, tradingProviderId, ngayDauTuSauTaiTuc, approveIp, username, false);
                // Nếu tái tục gốc nhận lợi nhuận thì sẽ sinh thêm thông 
                if (renewalsRequestFind.SettlementMethod == SettlementMethod.NHAN_LOI_NHUAN_VA_TAI_TUC_GOC)
                {
                    _orderPaymentRepository.PaymentPayAdd(new InvestEntities.DataEntities.OrderPayment
                    {
                        TradingProviderId = tradingProviderId,
                        OrderId = result.OrderId ?? 0,
                        TranDate = DateTime.Now,
                        TranType = TranTypes.CHI,
                        TranClassify = TranClassifies.THANH_TOAN,
                        PaymentType = PaymentTypes.CHUYEN_KHOAN,
                        Status = OrderPaymentStatus.DA_THANH_TOAN,
                        PaymentAmnount = result.AmountMoney,
                        Description = $"CTT {orderFind.ContractCode}",
                        CreatedBy = username,
                        ApproveDate = DateTime.Now,
                        ApproveBy = username,
                    }, false);
                }
                transaction.Commit();
            }
        }

        /// <summary>
        /// Duyệt chi trả 
        /// </summary>
        /// <exception cref="FaultException"></exception>
        [Obsolete("Bỏ")]
        public async Task ApproveInterestPayment(List<int> ids)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var approveIp = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            var result = new InvestInterestPayment();
            //Sử dụng transaction để thực hiện vòng for lập chi trả khi 1 trong số chi trả có sổ lệnh không hoạt động trả ra exception
            //toàn bộ chi truyền vào ko đc thực hiện
            // Lấy thông tin sổ lệnh, tính đáo hạn, NextWorkDay đưa vào repo để cho dùng được transaction
            //using (var transaction = new TransactionScope())
            var transaction = _interestPaymentRepository.BeginTransaction();
            {
                //Nếu tái tục sau chi trả thì sẽ dùng để đầu tư tiếp
                var ngayDauTuSauTaiTuc = DateTime.Now.Date;
                foreach (var id in ids)
                {
                    var interestPaymentFind = _interestPaymentRepository.FindById(id, tradingProviderId, false);
                    if (interestPaymentFind == null)
                    {
                        throw new FaultException(new FaultReason($"Không tìm thấy thông chi trả"), new FaultCode(((int)ErrorCode.InvestInterestPaymentNotFound).ToString()), "");
                    }
                    if (interestPaymentFind.PayDate != null && interestPaymentFind.PayDate.Value.Date > DateTime.Now.Date)
                    {
                        throw new FaultException(new FaultReason($"Ngày chi trả đang lớn hơn ngày hiện tại"), new FaultCode(((int)ErrorCode.InvestInterestPaymentPayDateLargerNowDate).ToString()), "");
                    }
                    var orderFind = _interestPaymentRepository.FindOrderById(interestPaymentFind.OrderId, tradingProviderId, false);

                    //Nếu sổ lệnh đang active
                    if (orderFind != null && orderFind.InvestDate != null && orderFind.Status == OrderStatus.DANG_DAU_TU)
                    {
                        //Tìm kiếm kỳ hạn để tìm ngày đáo hạn của order
                        var policyDetailFind = _policyRepository.FindPolicyDetailById(orderFind.PolicyDetailId, null, false);
                        if (policyDetailFind == null)
                        {
                            throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
                        }
                        DateTime ngayDauTu = orderFind.InvestDate.Value.Date;

                        //Tính ngày đáo hạn
                        ngayDauTuSauTaiTuc = _interestPaymentRepository.CalculateDueDate(policyDetailFind, ngayDauTu);
                    }
                    //Tiến hành chi trả (tái tục nếu có)
                    result = _interestPaymentRepository.ApproveInterestPayment(id, tradingProviderId, ngayDauTuSauTaiTuc, approveIp, username, false);
                    _orderPaymentRepository.PaymentPayAdd(new InvestEntities.DataEntities.OrderPayment
                    {
                        TradingProviderId = tradingProviderId,
                        OrderId = result.OrderId,
                        TranDate = DateTime.Now,
                        TranType = TranTypes.CHI,
                        TranClassify = TranClassifies.THANH_TOAN,
                        PaymentType = PaymentTypes.TIEN_MAT,
                        Status = OrderPaymentStatus.DA_THANH_TOAN,
                        PaymentAmnount = result.AmountMoney,
                        Description = $"CLN {orderFind.ContractCode}",
                        CreatedBy = username,
                        ApproveDate = DateTime.Now,
                        ApproveBy = username,
                    }, false);
                    await _investSendEmailServices.SendEmailInvestInterestPaymentSuccess(result.Id);
                }
                transaction.Commit();
            }
            _interestPaymentRepository.CloseConnection();
        }

        /// <summary>
        /// Chuẩn bị để chi tiền
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<MsbRequestPaymentWithErrorDto> PrepareApproveRequestInterestPayment(PrepareApproveRequestInterestPaymentDto input)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(PrepareApproveRequestInterestPayment)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            var prefixAccount = _dbContext.TradingMSBPrefixAccounts.FirstOrDefault(e => e.TradingBankAccountId == input.TradingBankAccId && e.Deleted == YesNo.NO)
                    .ThrowIfNull(_dbContext, ErrorCode.CoreTradingMSBPrefixAccountNotConfigured);
            List<int> distributionIds = new();
            var prepareResult = new MsbRequestPaymentWithErrorDto()
            {
                Id = (long)_msbRequestPaymentEFRepository.NextKey(),
                PrefixAccount = prefixAccount.PrefixMsb,
                Details = new(),
            };
            foreach (long interestPaymentId in input.InterestPaymentIds)
            {
                // Kiểm tra xem đã có yêu cầu chi tiền trước đó mà không thành công không
                var checkRequestPayment = _dbContext.MsbRequestPaymentDetail.Any(p => p.DataType == RequestPaymentDataTypes.EP_INV_INTEREST_PAYMENT && p.ReferId == interestPaymentId
                                            && p.Exception == null && p.Status == RequestPaymentStatus.KHOI_TAO);
                if (checkRequestPayment)
                {
                    _interestPaymentEFRepository.ThrowException(ErrorCode.PaymentMsbNotificationExistRequestNotSuccess, interestPaymentId);
                }

                // Tìm kiếm chi trả
                var interestPayment = _interestPaymentEFRepository.FindById(interestPaymentId, tradingProviderId)
                    .ThrowIfNull(_dbContext, ErrorCode.InvestInterestPaymentNotFound, interestPaymentId);

                if (interestPayment.Status != InterestPaymentStatus.DA_LAP_CHUA_CHI_TRA && interestPayment.Status != InterestPaymentStatus.CHO_PHAN_HOI)
                {
                    _interestPaymentEFRepository.ThrowException(ErrorCode.InvesInterestPaymentNotRequest);
                }

                // Tìm kiếm thông tin hợp đồng
                var orderFind = _invOrderEFRepository
                    .FindById(interestPayment.OrderId, tradingProviderId)
                    .ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound, interestPayment.OrderId);

                if (orderFind.MethodInterest != InvestMethodInterests.DoPayment)
                {
                    _interestPaymentEFRepository.ThrowException($"Danh sách chi trả có chứa lệnh \"{orderFind.ContractCode}\" không chi tiền");
                }

                var cifCodeFind = _cifCodeEFRepository.FindByCifCode(orderFind.CifCode)
                    .ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound, orderFind.CifCode);

                var prepareResultDetail = new MsbRequestDetailWithErrorDto
                {
                    Id = (long)_msbRequestPaymentDetailEFRepository.NextKey(),
                    DataType = RequestPaymentDataTypes.EP_INV_INTEREST_PAYMENT,
                    ReferId = interestPaymentId,
                    AmountMoney = interestPayment.AmountMoney + interestPayment.TotalValueInvestment, //cộng tiền thực nhận của chi trả
                };

                distributionIds.Add(orderFind.DistributionId);

                if (cifCodeFind.InvestorId != null) //khách cá nhân
                {
                    var investorBankAcc = _investorBankAccountEFRepository
                        .FindById(orderFind.InvestorBankAccId ?? 0)
                        .ThrowIfNull(_dbContext, ErrorCode.InvestorBankAccNotFound, orderFind.InvestorBankAccId);

                    var bankFind = _bankEFRepository.FindById(investorBankAcc.BankId)
                        .ThrowIfNull(_dbContext, ErrorCode.CoreBankNotFound, investorBankAcc.BankId);

                    prepareResultDetail.BankAccount = investorBankAcc.BankAccount;
                    prepareResultDetail.OwnerAccount = investorBankAcc.OwnerAccount;
                    prepareResultDetail.Bin = bankFind.Bin;
                    prepareResultDetail.BankId = bankFind.BankId;
                    prepareResultDetail.BankCode = bankFind.BankCode;
                    prepareResultDetail.BankName = bankFind.BankName;
                }
                else //khách doanh nghiệp
                {
                    var businessBankAcc = _businessCustomerBankEFRepository
                       .FindById(orderFind.InvestorBankAccId ?? 0)
                       .ThrowIfNull(_dbContext, ErrorCode.CoreBusinessCustomerBankNotFound, orderFind.InvestorBankAccId);

                    var bankFind = _bankEFRepository.FindById(businessBankAcc.BankId)
                        .ThrowIfNull(_dbContext, ErrorCode.CoreBankNotFound, businessBankAcc.BankId);

                    prepareResultDetail.BankAccount = businessBankAcc.BankAccNo;
                    prepareResultDetail.OwnerAccount = businessBankAcc.BankAccName;
                    prepareResultDetail.Bin = bankFind.Bin;
                    prepareResultDetail.BankId = bankFind.BankId;
                    prepareResultDetail.BankCode = bankFind.BankCode;
                    prepareResultDetail.BankName = bankFind.BankName;
                }
                if (string.IsNullOrWhiteSpace(prepareResultDetail.Bin))
                {
                    prepareResultDetail.ErrorMessage = $"Chưa cấu hình BIN cho ngân hàng {prepareResultDetail.BankCode}";
                }
                prepareResult.Details.Add(prepareResultDetail);
            }

            //Check xem các yêu cầu chi tiền (interestPayment) có cùng 1 sản phẩm phân phối (distribution) hay không 
            if (distributionIds.GroupBy(e => e).Count() > 1)
            {
                _interestPaymentEFRepository.ThrowException(ErrorCode.InvestRequestIsNotTheSameDistribution);
            }
            //lọc ra những bank có bin khác null
            var detailRequest = prepareResult.Details.Where(d => d.Bin != null)
                .Select(d => new MsbRequestPayMoneyItemDto
                {
                    DetailId = d.Id,
                    BankAccount = d.BankAccount,
                    OwnerAccount = d.OwnerAccount,
                    AmountMoney = d.AmountMoney,
                    Note = $"{PaymentNotes.THANH_TOAN}{d.Id}",
                    ReceiveBankBin = d.Bin
                })
                .ToList();

            var request = new MsbRequestPayMoneyDto()
            {
                TId = prefixAccount.TId,
                MId = prefixAccount.MId,
                RequestId = prepareResult.Id,
                PrefixAccount = prepareResult.PrefixAccount,
                Details = detailRequest,
                AccessCode = prefixAccount.AccessCode
            };
            if (detailRequest.Count() > 0)
            {
                ResultRequestPayDto resultRequest = new();
                try
                {
                    //kết nối bank
                    resultRequest = await _msbPayMoneyServices.RequestPayMoney(request);
                }
                catch (Exception ex)
                {
                    var requestPaymentInsert = _msbRequestPaymentEFRepository.Add(new MsbRequestPayment
                    {
                        Id = prepareResult.Id,
                        RequestType = RequestPaymentTypes.CHI_TRA_LOI_TUC,
                        ProductType = ProductTypes.INVEST,
                        TradingProdiverId = tradingProviderId,
                        CreatedBy = username,
                    });
                    foreach (var detailItem in prepareResult.Details.Where(d => d.Bin != null))
                    {
                        _msbRequestPaymentDetailEFRepository.Add(new MsbRequestPaymentDetail
                        {
                            Id = detailItem.Id,
                            RequestId = prepareResult.Id,
                            DataType = detailItem.DataType,
                            ReferId = detailItem.ReferId,
                            Status = RequestPaymentStatus.FAILED,
                            BankId = detailItem.BankId,
                            OwnerAccount = detailItem.OwnerAccount,
                            OwnerAccountNo = detailItem.BankAccount,
                            Note = $"{PaymentNotes.THANH_TOAN}{detailItem.Id}",
                            Bin = detailItem.Bin,
                            Exception = ex.Message,
                            AmountMoney = detailItem.AmountMoney
                        });
                    }
                    _dbContext.SaveChanges();
                    throw;
                }

                foreach (var err in resultRequest.ErrorDetails)
                {
                    var detail = prepareResult.Details.FirstOrDefault(d => d.Id == err.DetailId)
                        .ThrowIfNull(ErrorCode.BadRequest, $"Yêu cầu chi hộ bị thay đổi id khi nhận kết quả từ bank, transId | detailId = {err.DetailId}");
                    detail.ErrorMessage = err.ErrorMessage; //gán lỗi của bank trả về
                }
                prepareResult.IsSuccess = resultRequest.ErrorDetails.Count == 0 && prepareResult.Details.Count(d => d.Bin == null) == 0;
                prepareResult.IsSubmitOtp = resultRequest.IsSubmitOtp;
            }
            return prepareResult;
        }

        /// <summary>
        /// Duyệt/Hủy chi tiền cho Hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isRenewals"> Có tái tục hay không true/false</param>
        /// <returns></returns>
        public async Task ApproveInterestPaymentOrder(ApproveInterestPaymentRenewalsOrderDto input, bool isRenewals)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            _logger.LogInformation($"{nameof(InterestPaymentServices)} -> {nameof(ApproveInterestPaymentOrder)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            List<InvestInterestPayment> interestPayments = new();
            MsbRequestPayment requestPayment = new MsbRequestPayment();
            List<MsbRequestPaymentDetail> listRequestPaymentDetails = new();

            // Kiểm tra thông tin các yêu cầu chi trả
            foreach (var interestPaymentId in input.InterestPaymentIds)
            {
                var interestPayment = _interestPaymentEFRepository.FindById(interestPaymentId, tradingProviderId)
                                         .ThrowIfNull(_dbContext, ErrorCode.InvestInterestPaymentNotFound);

                var orderFind = _invOrderEFRepository.EntityNoTracking
                    .Select(o => new { o.Id, o.MethodInterest, o.ContractCode })
                    .FirstOrDefault(o => o.Id == interestPayment.OrderId);

                if (input.Status == InterestPaymentStatus.DA_DUYET_CHI_TIEN && orderFind?.MethodInterest != InvestMethodInterests.DoPayment)
                {
                    _interestPaymentEFRepository.ThrowException($"Danh sách chi trả có chứa lệnh \"{orderFind.ContractCode}\" không chi tiền");
                }

                // Nếu khác trạng thái khởi tạo hoặc chờ phản hồi
                if (interestPayment.Status != InterestPaymentStatus.DA_LAP_CHUA_CHI_TRA && interestPayment.Status != InterestPaymentStatus.CHO_PHAN_HOI)
                {
                    _interestPaymentEFRepository.ThrowException(ErrorCode.InvesInterestPaymentNotRequest);
                }
                // Yêu cầu chi có giống lúc chuẩn bị hay không
                if (input.Prepare != null && !input.Prepare.Details.Select(p => p.ReferId).Contains(interestPaymentId))
                {
                    _interestPaymentEFRepository.ThrowException(ErrorCode.InvestInterestPaymentIdNotInPrepareDetail);
                }
                interestPayments.Add(interestPayment);
            }

            // Đã lập chưa chi trả
            if (input.Status == InterestPaymentStatus.DA_DUYET_KHONG_CHI_TIEN)
            {
                var transaction = _dbContext.Database.BeginTransaction();
                // Có tái tục
                if (isRenewals)
                {
                    await RenewalInterestPayment(interestPayments, tradingProviderId, input.TradingBankAccId, input.Status, username, input.ApproveNote);
                }
                else
                {
                    InterestPayment(interestPayments, tradingProviderId, input.TradingBankAccId, input.Status, username, input.ApproveNote);
                }
                _dbContext.SaveChanges();
                transaction.Commit();
                // Duyệt không đi tiền thì gửi thông báo//Gửi thông báo khi thành công các lệnh. Duyệt chi tiền từ bank thì đợi notipayment
                foreach (var interestPayment in interestPayments)
                {
                    // Chi có tái tục
                    if (isRenewals)
                    {
                        var orderNew = _invOrderEFRepository.EntityNoTracking.FirstOrDefault(o => o.RenewalsReferId == interestPayment.OrderId && o.Deleted == YesNo.NO);
                        if (orderNew == null)
                        {
                            _logger.LogError($"{nameof(ApproveInterestPaymentOrder)}: Không tìm thấy thông tin hợp đồng mới sau khi tái tục thành công");
                        }
                        else
                        {
                            try
                            {
                                // Ký điện tử các file hợp đồng khi hợp đồng tái tục thành công
                                _investOrderContractFileServices.UpdateContractFileSignPdf(orderNew.Id);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.Message);
                            }
                        }
                        await _investSendEmailServices.SendEmailInvestRenewalsSuccess(interestPayment.Id);
                    }
                    else
                    {
                        // Chi bình thường
                        if (interestPayment.IsLastPeriod == YesNo.NO)
                        {
                            await _investSendEmailServices.SendEmailInvestInterestPaymentSuccess(interestPayment.Id);
                        }
                        // Chi có tất toán
                        else if (interestPayment.IsLastPeriod == YesNo.YES)
                        {
                            await _investSendEmailServices.SendEmailInvestInterestPaymentSettlementSuccess(interestPayment.Id);
                        }
                    }
                }
            }

            // Hủy yêu cầu chi tiền
            else if (input.Status == InterestPaymentStatus.HUY_DUYET)
            {
                foreach (var interestPayment in interestPayments)
                {
                    interestPayment.Status = input.Status;
                    interestPayment.ModifiedBy = username;
                    interestPayment.ModifiedDate = DateTime.Now;
                }
                _dbContext.SaveChanges();
            }

            // Chi tiền tự động
            if (input.Status == InterestPaymentStatus.DA_DUYET_CHI_TIEN)
            {
                var transaction = _dbContext.Database.BeginTransaction();
                // Chuyển sang trạng thái tạm khi duyệt đi tiền
                // Notifi thành công thì chuyển sang trạng thái duyệt đi tiền
                var waitResponseStatus = InterestPaymentStatus.CHO_PHAN_HOI;

                // Có tái tục
                if (isRenewals)
                {
                    await RenewalInterestPayment(interestPayments, tradingProviderId, input.TradingBankAccId, waitResponseStatus, username, null, MsbBankStatus.INIT);
                }
                else
                {
                    InterestPayment(interestPayments, tradingProviderId, input.TradingBankAccId, waitResponseStatus, username, null, MsbBankStatus.INIT);
                }

                var prefixAccount = _dbContext.TradingMSBPrefixAccounts.FirstOrDefault(e => e.TradingBankAccountId == input.TradingBankAccId && e.Deleted == YesNo.NO)
                   .ThrowIfNull(_dbContext, ErrorCode.CoreTradingMSBPrefixAccountNotConfigured);

                //lấy thông tin để lưu vào lô chi hộ
                requestPayment.Id = input.Prepare.Id;
                requestPayment.TradingProdiverId = tradingProviderId;
                requestPayment.ProductType = ProductTypes.INVEST;
                requestPayment.RequestType = RequestPaymentTypes.CHI_TRA_LOI_TUC;
                requestPayment.CreatedDate = DateTime.Now;
                requestPayment.CreatedBy = username;

                _msbRequestPaymentEFRepository.Add(requestPayment);
                foreach (var requestDetail in input.Prepare.Details)
                {
                    var requestPaymentDetail = new MsbRequestPaymentDetail()
                    {
                        Id = requestDetail.Id,
                        RequestId = requestPayment.Id,
                        DataType = RequestPaymentDataTypes.EP_INV_INTEREST_PAYMENT,
                        ReferId = requestDetail.ReferId,
                        Status = RequestPaymentStatus.KHOI_TAO,
                        BankId = requestDetail.BankId,
                        Bin = requestDetail.Bin,
                        Note = $"{PaymentNotes.THANH_TOAN}{requestDetail.Id}",
                        OwnerAccount = requestDetail.OwnerAccount,
                        OwnerAccountNo = requestDetail.BankAccount,
                        TradingBankAccId = input.TradingBankAccId ?? 0,
                        AmountMoney = requestDetail.AmountMoney
                    };
                    _msbRequestPaymentDetailEFRepository.Add(requestPaymentDetail);
                    listRequestPaymentDetails.Add(requestPaymentDetail);
                }
                _dbContext.SaveChanges();

                string exceptionMessage;
                try
                {
                    await _msbPayMoneyServices.TransferProcessOtp(new ProcessRequestPayDto
                    {
                        RequestId = requestPayment.Id,
                        Otp = input.Otp,
                        TId = prefixAccount.TId,
                        MId = prefixAccount.MId,
                        AccessCode = prefixAccount.AccessCode
                    });
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    exceptionMessage = ex.Message;
                    transaction.Rollback();

                    foreach (var detailItem in listRequestPaymentDetails)
                    {
                        detailItem.Status = RequestPaymentStatus.FAILED;
                        detailItem.Exception = ex.Message;
                    }
                    _msbRequestPaymentEFRepository.Entity.Add(requestPayment);
                    _msbRequestPaymentDetailEFRepository.Entity.AddRange(listRequestPaymentDetails);
                    _dbContext.SaveChanges();
                    throw;
                }
            }
        }

        /// <summary>
        /// Chi trả có tái tục vốn
        /// </summary>
        public async Task RenewalInterestPayment(List<InvestInterestPayment> interestPayments, int tradingProviderId, int? tradingBankAccId, int interestPaymentStatus, string username, int? approveNote, int? statusBank = null)
        {
            _logger.LogInformation($"{nameof(InterestPaymentServices)} -> {nameof(RenewalInterestPayment)}: interestPayments = {JsonSerializer.Serialize(interestPayments)}, tradingProviderId = {tradingProviderId}, tradingBankAccId = {tradingBankAccId}, interestPaymentStatus = {interestPaymentStatus} username = {username}");

            var approveIp = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            foreach (var interestPaymentFind in interestPayments)
            {
                if (interestPaymentFind.PayDate != null && interestPaymentFind.PayDate.Value.Date > DateTime.Now.Date)
                {
                    throw new FaultException(new FaultReason($"Ngày chi trả đang lớn hơn ngày hiện tại"), new FaultCode(((int)ErrorCode.InvestInterestPaymentPayDateLargerNowDate).ToString()), "");
                }
                if (statusBank == null)
                {
                    interestPaymentFind.StatusBank = MsbBankStatus.INIT;
                }
                var orderFind = _invOrderEFRepository.Entity.FirstOrDefault(o => o.Id == interestPaymentFind.OrderId && o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO)
                    .ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);

                DateTime investDateNew = DateTime.Now;
                //Nếu sổ lệnh đang active
                if (orderFind != null && orderFind.InvestDate != null && orderFind.Status == OrderStatus.DANG_DAU_TU)
                {
                    //Tìm kiếm kỳ hạn để tìm ngày đáo hạn của order
                    var policyDetailFind = _policyRepository.FindPolicyDetailById(orderFind.PolicyDetailId, null, false);
                    if (policyDetailFind == null)
                    {
                        throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
                    }
                    var distribution = _distributionRepository.FindById(orderFind.DistributionId, tradingProviderId)
                        .ThrowIfNull(_dbContext, ErrorCode.InvestDistributionNotFound);
                    DateTime ngayDauTu = orderFind.InvestDate.Value.Date;

                    //Tính ngày đáo hạn
                    investDateNew = orderFind.DueDate ?? _orderRepository.CalculateDueDate(policyDetailFind, ngayDauTu, distribution.CloseCellDate);
                }

                PolicyDetail policyDetailNew = new();
                // Tìm yêu cầu tái tục
                var renewalsRequestFind = _renewalsRequestRepository.GetByOrderId(interestPaymentFind.OrderId);
                if (renewalsRequestFind == null)
                {
                    throw new FaultException(new FaultReason($"Chi trả tái tục cuối kỳ: Hợp đồng chưa có yêu cầu tái tục"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                }
                else
                {
                    // Kỳ hạn mới
                    policyDetailNew = _policyRepository.FindPolicyDetailById(renewalsRequestFind.RenewalsPolicyDetailId);
                    if (policyDetailNew == null)
                    {
                        throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn sau tái tục"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
                    }
                    if (policyDetailNew.Status != Utils.Status.ACTIVE)
                    {
                        throw new FaultException(new FaultReason($"Thông tin kỳ hạn không trong trạng thái hoạt động"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotActive).ToString()), "");
                    }
                }

                var approveInterestLastPeriod = _interestPaymentEFRepository.RenewalInterestLastPeriodOrder(interestPaymentFind, tradingProviderId, interestPaymentStatus, approveIp, username, approveNote);

                #region Kiểm tra thông tin cho hợp đồng mới
                // Kiểm tra thông tin phân phối mới
                var distributionNew = _distributionRepository.FindById(policyDetailNew.DistributionId, tradingProviderId);
                if (distributionNew == null)
                {
                    _interestPaymentEFRepository.ThrowException(ErrorCode.InvestDistributionNotFound);
                }
                // Phân phối có đóng đặt lệnh không
                if (distributionNew.IsClose == YesNo.YES)
                {
                    _interestPaymentEFRepository.ThrowException(ErrorCode.InvestDistributionIsCloseOrder);
                }

                // Kiểm tra chính sách mới
                var policyNew = _policyRepository.FindPolicyById(policyDetailNew.PolicyId, tradingProviderId);
                if (policyNew == null)
                {
                    _interestPaymentEFRepository.ThrowException(ErrorCode.InvestPolicyNotFound);
                }
                // Trạng thái chính sách
                if (policyNew.Status != Utils.Status.ACTIVE)
                {
                    _interestPaymentEFRepository.ThrowException(ErrorCode.InvestPolicyNotActive);
                }
                // Số tiền tái tục có lớn hơn số tiền đầu tư tối thiểu
                if (approveInterestLastPeriod.InitTotalValueOrderNew < policyNew.MinMoney)
                {
                    _interestPaymentEFRepository.ThrowException(ErrorCode.InvestTotalValueOrderMustBiggerMinMoneyPolicy);
                }

                // Kiểm tra sản phẩm mới
                var projectNew = _projectRepository.FindById(distributionNew.ProjectId);
                if (projectNew == null)
                {
                    _interestPaymentEFRepository.ThrowException(ErrorCode.InvestProjectNotFound);
                }

                // Nếu số tiền tái tục lớn hơn hạn mức đầu tư
                decimal totalValueOrderOfDistribution = _orderRepository.MaxTotalInvestment(tradingProviderId, distributionNew.Id);
                if ((totalValueOrderOfDistribution + orderFind.TotalValue) - approveInterestLastPeriod.InitTotalValueOrderNew < 0)
                {
                    _interestPaymentEFRepository.ThrowException(ErrorCode.InvestOrderTotalValueBiggerMaxMoney);
                }

                #endregion

                // Nếu không phải là kiểm tra (Duyệt chi tự động): Thêm hợp đồng mới + Thanh toán mới
                if (interestPaymentStatus == InterestPaymentStatus.DA_DUYET_KHONG_CHI_TIEN || interestPaymentStatus == InterestPaymentStatus.DA_DUYET_CHI_TIEN)
                {
                    var renewalsRequest = _dbContext.InvestRenewalsRequests.FirstOrDefault(r => r.Id == renewalsRequestFind.Id && r.SettlementMethod != SettlementMethod.TAT_TOAN_KHONG_TAI_TUC && r.Status != InvestRenewalsRequestStatus.DA_HUY);
                    renewalsRequest.Status = InvestRenewalsRequestStatus.DA_DUYET;
                    orderFind.TotalValue = 0;
                    orderFind.Status = OrderStatus.TAT_TOAN;
                    orderFind.SettlementDate = interestPaymentFind.PayDate;
                    orderFind.SettlementMethod = approveInterestLastPeriod.SettlementMethod;
                    orderFind.RenewalsPolicyDetailId = approveInterestLastPeriod.RenewalsPolicyDetailId;

                    // Tìm đến hợp đồng gốc nếu có
                    var orderOriginal = _invOrderEFRepository.EntityNoTracking.FirstOrDefault(r => r.Id == orderFind.RenewalsReferIdOriginal && r.Deleted == YesNo.NO);

                    // Id của hợp đồng mới
                    long orderNewId = (long)_invOrderEFRepository.NextKey();

                    // Mã hợp đồng cũ + RO + lần tái tục
                    var newContractCode = (orderOriginal?.ContractCode ?? orderFind.ContractCode) + $"RO{(orderFind.RenewalIndex ?? 0) + 1}";

                    var orderNew = _invOrderEFRepository.AddRenewalOrder(new InvOrder
                    {
                        Id = orderNewId,
                        TradingProviderId = orderFind.TradingProviderId,
                        CifCode = orderFind.CifCode,
                        ContractCode = newContractCode,
                        DepartmentId = orderFind.DepartmentId,
                        ProjectId = projectNew.Id,
                        DistributionId = distributionNew.Id,
                        PolicyId = policyDetailNew.PolicyId,
                        PolicyDetailId = policyDetailNew.Id,
                        TotalValue = approveInterestLastPeriod.InitTotalValueOrderNew,
                        BuyDate = investDateNew,
                        IsInterest = orderFind.IsInterest,
                        SaleReferralCode = orderFind.SaleReferralCode,
                        Source = orderFind.Source,
                        BusinessCustomerBankAccId = orderFind.BusinessCustomerBankAccId,
                        InvestorBankAccId = orderFind.InvestorBankAccId,
                        CreatedBy = username,
                        InvestDate = investDateNew,
                        InvestorIdenId = orderFind.InvestorIdenId,
                        ContractAddressId = orderFind.ContractAddressId,
                        PaymentFullDate = investDateNew,
                        DeliveryStatus = (orderFind.Source == SourceOrder.OFFLINE) ? DeliveryStatus.WAITING : null,
                        DeliveryDate = (orderFind.Source == SourceOrder.OFFLINE) ? investDateNew : null,
                        ActiveDate = DateTime.Now,
                        ApproveBy = username,
                        ApproveDate = DateTime.Now,
                        SaleOrderId = orderFind.SaleOrderId,
                        DepartmentIdSub = orderFind.DepartmentIdSub,
                        SaleReferralCodeSub = orderFind.SaleReferralCodeSub,
                        InitTotalValue = approveInterestLastPeriod.InitTotalValueOrderNew,
                        RenewalsReferId = orderFind.Id,
                        RenewalIndex = (orderFind.RenewalIndex ?? 0) + 1,
                        RenewalsReferIdOriginal = (orderFind.RenewalsReferIdOriginal != null) ? orderFind.RenewalsReferIdOriginal : orderFind.Id,
                    });

                    List<string> listContractCode = new();
                    var contactTemplate = from contractTemp in _dbContext.InvestContractTemplates
                                          join contractTempTemp in _dbContext.InvestContractTemplateTemps on contractTemp.ContractTemplateTempId equals contractTempTemp.Id
                                          where contractTemp.PolicyId == policyNew.Id && contractTemp.TradingProviderId == tradingProviderId && contractTemp.Deleted == YesNo.NO
                                          && contractTempTemp.Deleted == YesNo.NO && contractTemp.Status == Status.ACTIVE && contractTempTemp.Status == Status.ACTIVE
                                          && (contractTempTemp.ContractSource == orderNew.Source || contractTempTemp.ContractSource == ContractSources.ALL)
                                          && ((approveInterestLastPeriod.SettlementMethod == SettlementMethod.NHAN_LOI_NHUAN_VA_TAI_TUC_GOC && (contractTempTemp.ContractType == ContractTypes.TAI_TUC_GOC || contractTemp.ContractType == ContractTypes.TAI_TUC_GOC))
                                             || (approveInterestLastPeriod.SettlementMethod == SettlementMethod.TAI_TUC_GOC_VA_LOI_NHUAN && (contractTempTemp.ContractType == ContractTypes.TAI_TUC_GOC_VA_LOI_NHUAN || contractTemp.ContractType == ContractTypes.TAI_TUC_GOC_VA_LOI_NHUAN)))
                                          select contractTemp;
                    foreach (var contract in contactTemplate)
                    {
                        var contractCode = _investContractCodeServices.GetContractCode(orderNew, policyNew, contract.ConfigContractId);
                        listContractCode.Add(contractCode);
                    }

                    // Nội dung thanh toán khi hợp đồng có tái tục
                    string paymentNote = "";

                    if (policyNew.RenewalsType == InvestRenewalsType.GIU_HOP_DONG_CU)
                    {
                        var orderRenewalContractFile = _dbContext.InvestOrderContractFile.FirstOrDefault(o => o.OrderId == orderNew.RenewalsReferId && o.ContractCodeGen != null && o.Deleted == YesNo.NO);
                        paymentNote = PaymentNotes.THANH_TOAN + orderRenewalContractFile?.ContractCodeGen;

                    }
                    else if (policyNew.RenewalsType == InvestRenewalsType.TAO_HOP_DONG_MOI)
                    {
                        if (listContractCode.Distinct().Count() == 1)
                        {
                            paymentNote = PaymentNotes.THANH_TOAN + listContractCode.First();
                        }
                        else
                        {
                            paymentNote = PaymentNotes.THANH_TOAN + orderNew.ContractCode;
                        }
                    }
                    _invOrderPaymentEFRepository.Add(new InvestEntities.DataEntities.OrderPayment
                    {
                        TradingProviderId = tradingProviderId,
                        OrderId = orderNewId,
                        TranDate = investDateNew,
                        TranType = TranTypes.THU,
                        TranClassify = TranClassifies.TAI_TUC_HOP_DONG,
                        PaymentType = PaymentTypes.CHUYEN_KHOAN,
                        PaymentAmnount = orderNew.TotalValue,
                        Description = paymentNote,
                        Status = OrderPaymentStatus.DA_THANH_TOAN,
                        CreatedBy = username,
                        CreatedDate = DateTime.Now,
                        ApproveBy = username,
                        ApproveDate = DateTime.Now,
                        TradingBankAccId = tradingBankAccId ?? orderFind.BusinessCustomerBankAccId ?? 0,
                    });

                    _logger.LogInformation($"{nameof(InterestPaymentServices)} -> {nameof(RenewalInterestPayment)}: orderNew: {JsonSerializer.Serialize(orderNew)}");

                    _dbContext.SaveChanges();
                    //số lần tái tục
                    var renewalsTimes = _renewalsRequestRepository.GetListByOrderId(renewalsRequestFind.OrderId).Where(r => r.Status == InvestRenewalsRequestStatus.DA_DUYET).Count();
                    //Get base data
                    var data = _contractDataServices.GetDataContractFile(orderNew, orderNew.TradingProviderId, true);
                    //Get data cho hợp đồng rút
                    data.AddRange(_contractDataServices.GetDataRenewalsContractFile(policyDetailNew));
                    //sinh hợp đồng tái tục
                    await _investOrderContractFileServices.CreateContractFileRenewals(orderNew, renewalsRequestFind.Id, renewalsTimes, data, approveInterestLastPeriod.SettlementMethod);
                    if (policyNew.RenewalsType == InvestRenewalsType.TAO_HOP_DONG_MOI)
                    {
                        //Sinh hợp đồng mới
                        await _investOrderContractFileServices.UpdateContractFile(orderNew, data, ContractTypes.DAT_LENH);
                        _dbContext.SaveChanges();
                    }
                    if (approveInterestLastPeriod.SettlementMethod == SettlementMethod.NHAN_LOI_NHUAN_VA_TAI_TUC_GOC)
                    {
                        _invOrderPaymentEFRepository.Add(new InvestEntities.DataEntities.OrderPayment
                        {
                            TradingProviderId = tradingProviderId,
                            OrderId = orderFind.Id,
                            TranDate = DateTime.Now,
                            TranType = TranTypes.CHI,
                            TranClassify = TranClassifies.CHI_TRA_LOI_TUC,
                            PaymentType = PaymentTypes.CHUYEN_KHOAN,
                            PaymentAmnount = approveInterestLastPeriod.InterestPaymentMoney,
                            Description = PaymentNotes.CHI_LOI_NHUAN + orderFind.ContractCode,
                            Status = OrderPaymentStatus.DA_THANH_TOAN,
                            CreatedBy = username,
                            CreatedDate = DateTime.Now,
                            ApproveBy = username,
                            ApproveDate = DateTime.Now,
                            TradingBankAccId = tradingBankAccId ?? orderFind.BusinessCustomerBankAccId ?? 0,
                        });
                    }
                }
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Chi trả từng kỳ
        /// </summary>
        public void InterestPayment(List<InvestInterestPayment> interestPayments, int tradingProviderId, int? tradingBankAccId, int interestPaymentStatus, string username, int? approveNote, int? statusBank = null)
        {
            var approveIp = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            foreach (var interestPaymentFind in interestPayments)
            {

                if (interestPaymentFind.PayDate != null && interestPaymentFind.PayDate.Value.Date > DateTime.Now.Date)
                {
                    throw new FaultException(new FaultReason($"Ngày chi trả đang lớn hơn ngày hiện tại"), new FaultCode(((int)ErrorCode.InvestInterestPaymentPayDateLargerNowDate).ToString()), "");
                }

                var approveInterestPayment = _interestPaymentEFRepository.ApproveInterestPayment(interestPaymentFind, tradingProviderId, interestPaymentStatus, approveIp, username, approveNote);
                if (statusBank == null)
                {
                    approveInterestPayment.StatusBank = MsbBankStatus.INIT;
                }
                _dbContext.SaveChanges();

                //Duyệt không đi tiền thì gửi thông báo// Duyệt chi tiền từ bank thì đợi notipayment
                if (interestPaymentStatus == InterestPaymentStatus.DA_DUYET_KHONG_CHI_TIEN || interestPaymentStatus == InterestPaymentStatus.DA_DUYET_CHI_TIEN)
                {
                    var orderFind = _invOrderEFRepository.Entity.FirstOrDefault(o => o.Id == interestPaymentFind.OrderId && o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO)
                        .ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);

                    var genContractCode = _invOrderEFRepository.GetContractCodeGen(orderFind.Id);
                    _invOrderPaymentEFRepository.Add(new InvestEntities.DataEntities.OrderPayment
                    {
                        TradingProviderId = tradingProviderId,
                        OrderId = orderFind.Id,
                        TranDate = DateTime.Now,
                        TranType = TranTypes.CHI,
                        TranClassify = TranClassifies.CHI_TRA_LOI_TUC,
                        PaymentType = PaymentTypes.CHUYEN_KHOAN,
                        PaymentAmnount = approveInterestPayment.AmountMoney,
                        Description = PaymentNotes.CHI_LOI_NHUAN + (genContractCode ?? orderFind.ContractCode),
                        Status = OrderPaymentStatus.DA_THANH_TOAN,
                        CreatedBy = username,
                        CreatedDate = DateTime.Now,
                        ApproveBy = username,
                        ApproveDate = DateTime.Now,
                        TradingBankAccId = tradingBankAccId ?? orderFind.BusinessCustomerBankAccId ?? 0,
                    });

                    if (approveInterestPayment.IsLastPeriod == YesNo.YES)
                    {
                        orderFind.TotalValue = 0;
                        orderFind.Status = OrderStatus.TAT_TOAN;
                        orderFind.SettlementDate = approveInterestPayment.PayDate;
                    }
                    _dbContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Gửi lại thông báo chi trả thành công
        /// </summary>
        /// <param name="interestPaymentId"></param>
        /// <returns></returns>
        public async Task ResendNotifyInvestInterestPaymentSuccess(int interestPaymentId)
        {
            _logger.LogInformation($"{nameof(ResendNotifyInvestInterestPaymentSuccess)}: interestPayment = {interestPaymentId}");

            await _investSendEmailServices.SendEmailInvestInterestPaymentSuccess(interestPaymentId);
        }

        /// <summary>
        /// Gửi lại thông báo TÁI TỤC thành công
        /// </summary>
        /// <param name="interestPaymentId"></param>
        /// <returns></returns>
        public async Task ResendNotifyInvestRenewalsSuccess(int interestPaymentId)
        {
            _logger.LogInformation($"{nameof(ResendNotifyInvestRenewalsSuccess)}: interestPayment = {interestPaymentId}");

            var interestPayment = _interestPaymentRepository.FindById(interestPaymentId);
            if (interestPayment != null)
            {
                var order = _orderRepository.FindById(interestPayment.OrderId);
                if (order != null && order.RenewalsPolicyDetailId == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn sau tái tục"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
                }
                else if (order != null && order.RenewalsPolicyDetailId != null)
                {
                    await _investSendEmailServices.SendEmailInvestRenewalsSuccess(interestPaymentId);
                }
            }
        }

    }
}
