using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.RocketChat;
using EPIC.Entities.Dto.User;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerRepositories;
using EPIC.GarnerSharedDomain.Interfaces;
using EPIC.IdentityRepositories;
using EPIC.Notification.Dto.DataEmail;
using EPIC.Notification.Dto.GarnerNotification;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Notification;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.SharedApiService;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.Notification.Services
{
    /// <summary>
    /// Thông báo garner
    /// </summary>
    public class GarnerNotificationServices
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        //Garner Repo
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly GarnerDistributionEFRepository _garnerDistributionEFRepository;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly GarnerPolicyEFRepository _garnerPolicyEFRepository;
        private readonly GarnerPolicyDetailEFRepository _garnerPolicyDetailEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly GarnerOrderContractFileEFRepository _garnerOrderContractFileEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly UsersEFRepository _usersEFRepository;
        private readonly GarnerOrderPaymentEFRepository _garnerOrderPaymentEFRepository;
        private readonly GarnerWithdrawalEFRepository _garnerWithdrawalEFRepository;
        private readonly GarnerInterestPaymentEFRepository _garnerInterestPaymentEFRepository;
        private readonly GarnerContractTemplateEFRepository _contractTemplateEFRepository;
        private readonly GarnerConfigContractCodeEFRepository _garnerConfigContractCodeEFRepository;
        private readonly IGarnerContractCodeServices _garnerContractCodeService;
        private readonly DefErrorEFRepository _defErrorEFRepository;

        //Common
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SharedNotificationApiUtils _sharedEmailApiUtils;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;

        public GarnerNotificationServices(EpicSchemaDbContext dbContext,
            ILogger<GarnerNotificationServices> logger,
            IMapper mapper,
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor,
            SharedNotificationApiUtils sharedEmailApiUtils,
            IGarnerContractCodeServices garnerContractCodeServices,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _env = env;

            //Garner
            _garnerOrderEFRepository = new GarnerOrderEFRepository(_dbContext, _logger);
            _cifCodeEFRepository = new CifCodeEFRepository(_dbContext, _logger);
            _garnerDistributionEFRepository = new GarnerDistributionEFRepository(_dbContext, _logger);
            _garnerProductEFRepository = new GarnerProductEFRepository(_dbContext, _logger);
            _garnerPolicyEFRepository = new GarnerPolicyEFRepository(_dbContext, _logger);
            _garnerPolicyDetailEFRepository = new GarnerPolicyDetailEFRepository(_dbContext, _logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(_dbContext, _logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(_dbContext, _logger);
            _garnerOrderContractFileEFRepository = new GarnerOrderContractFileEFRepository(_dbContext, _logger);
            _investorEFRepository = new InvestorEFRepository(_dbContext, _logger);
            _usersEFRepository = new UsersEFRepository(_dbContext, _logger);
            _garnerOrderPaymentEFRepository = new GarnerOrderPaymentEFRepository(_dbContext, _logger);
            _garnerWithdrawalEFRepository = new GarnerWithdrawalEFRepository(dbContext, _logger);
            _garnerInterestPaymentEFRepository = new GarnerInterestPaymentEFRepository(dbContext, logger);
            _contractTemplateEFRepository = new GarnerContractTemplateEFRepository(dbContext, _logger);
            _garnerConfigContractCodeEFRepository = new GarnerConfigContractCodeEFRepository(dbContext, _logger);
            _garnerContractCodeService = garnerContractCodeServices;
            _defErrorEFRepository = new DefErrorEFRepository(_dbContext);

            //Common
            _httpContextAccessor = httpContextAccessor;
            _sharedEmailApiUtils = sharedEmailApiUtils;
            _configuration = configuration;
            _baseUrl = _configuration["SharedApi:BaseUrl"];
        }

        public DataEmail GetDataInvest(CifCodes cifCode, GarnerOrder order)
        {
            var result = new DataEmail();
            if (cifCode.InvestorId != null)
            {
                result.Investor = _investorEFRepository.FindById(cifCode.InvestorId ?? 0);
                if (result.Investor == null)
                {
                    _logger.LogError($"{nameof(GetDataInvest)}: Không tìm thấy khách hàng khi gửi email/ sms/ push app. investorId: {cifCode.InvestorId}");
                    return new();
                }
                result.InvestorIdentification = _investorEFRepository.GetIdentificationById(order.InvestorIdenId ?? 0);
                if (result.InvestorIdentification == null)
                {
                    _logger.LogError($"{nameof(GetDataInvest)}: Không tìm thấy thông tin giấy tờ khách hàng khi gửi email/ sms/ push app. investorIdentificationId: {order.InvestorIdenId}");
                    return new();
                }

                result.OtherParams = new ParamsChooseTemplate()
                {
                    TradingProviderId = order.TradingProviderId
                };
                var user = _usersEFRepository.FindByInvestorId(result.Investor.InvestorId);
                if (user != null)
                {
                    result.Users = _mapper.Map<UserDto>(user);
                    result.FcmTokens = _usersEFRepository.GetFcmTokenByUserId(result.Users.UserId);
                }
            }
            else
            {
                result.BusinessCustomer = _mapper.Map<BusinessCustomer>(_businessCustomerEFRepository.FindById(cifCode.BusinessCustomerId ?? 0));
                result.OtherParams = new ParamsChooseTemplate
                {
                    TradingProviderId = order.TradingProviderId
                };
            }
            return result;
        }

        public DataEmail GetDataInvest(CifCodes cifCode, int? tradingProviderId)
        {
            var result = new DataEmail();
            if (cifCode.InvestorId != null)
            {
                result.Investor = _investorEFRepository.FindById(cifCode.InvestorId ?? 0);
                if (result.Investor == null)
                {
                    _logger.LogError($"{nameof(GetDataInvest)}: Không tìm thấy khách hàng khi gửi email/ sms/ push app. investorId: {cifCode.InvestorId}");
                    return new();
                }
                result.InvestorIdentification = _investorEFRepository.GetDefaultIdentification(cifCode.InvestorId ?? 0);
                if (result.InvestorIdentification == null)
                {
                    _logger.LogError($"{nameof(GetDataInvest)}: Không tìm thấy thông tin giấy tờ khách hàng khi gửi email/ sms/ push app. InvestorId: {cifCode.InvestorId}");
                    return new();
                }

                result.OtherParams = new ParamsChooseTemplate()
                {
                    TradingProviderId = tradingProviderId
                };
                var user = _usersEFRepository.FindByInvestorId(result.Investor.InvestorId);
                if (user != null)
                {
                    result.Users = _mapper.Map<UserDto>(user);
                    result.FcmTokens = _usersEFRepository.GetFcmTokenByUserId(result.Users.UserId);
                }
            }
            else
            {
                result.BusinessCustomer = _mapper.Map<BusinessCustomer>(_businessCustomerEFRepository.FindById(cifCode.BusinessCustomerId ?? 0));
                result.OtherParams = new ParamsChooseTemplate
                {
                    TradingProviderId = tradingProviderId
                };
            }
            return result;
        }

        /// <summary>
        /// Thông báo tiền thành công khi duyệt lệnh chuyển tiền
        /// </summary>
        /// <param name="orderPaymentId"></param>
        /// <returns></returns>
        public async Task SendNotifyGarnerApprovePayment(long orderPaymentId)
        {
            _logger.LogInformation($"{nameof(SendNotifyGarnerApprovePayment)}: orderPaymentId: {orderPaymentId}");
            var orderPayment = _garnerOrderPaymentEFRepository.FindById(orderPaymentId);
            if (orderPayment == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerApprovePayment)}: Không tìm thấy thông tin thanh toán khi thông báo chuyển tiền thành công. orderPaymentId: {orderPaymentId}");
                return;
            }
            var order = _garnerOrderEFRepository.FindById(orderPayment.OrderId);
            if (order == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerApprovePayment)}: Không tìm thấy sổ lệnh khi thông báo chuyển tiền thành công. orderId: {orderPayment.OrderId}");
                return;
            }
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerApprovePayment)}: Không tìm thấy mã khách hàng khi thông báo chuyển tiền thành công. cifCode: {order.CifCode}");
                return;
            }

            var policy = _garnerPolicyEFRepository.FindById(order.PolicyId);
            if (policy == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerApprovePayment)}: Không tìm thấy thông tin chính sách Garner. PolicyId: {order.PolicyId}, orderId = {order.Id}");
                return;
            }

            var profitPolicyDetailByPolicyId = _garnerPolicyDetailEFRepository.GetAllPolicyDetailByPolicyId(order.PolicyId);
            decimal profit = 0;
            if (profitPolicyDetailByPolicyId.Count() > 0)
            {
                profit = profitPolicyDetailByPolicyId.Max(e => e.Profit);
            }
            else
            {
                _logger.LogError($"{nameof(SendNotifyGarnerApprovePayment)}: Không tìm thấy thông tin lợi nhuận kỳ hạn Garner. PolicyId: {order.PolicyId}, orderId = {order.Id}");
                return;
            }

            var tradingProvider = _tradingProviderEFRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerApprovePayment)}: Không tim thấy đại lý khi gửi email thông báo chuyển tiền thành công Garner. tradingProviderId = {order.TradingProviderId}, orderPaymentId = {orderPaymentId}");
                return;
            }

            var businessCustomer = _businessCustomerEFRepository.FindById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerApprovePayment)}: Không tim thấy business customer là đại lý khi gửi email thông báo chuyển tiền thành công invest. tradingProviderId = {order.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }
            var product = _garnerProductEFRepository.FindById(order.ProductId);
            if (product == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerApprovePayment)}: Không tìm thấy thông tin dự án Garner. projectId: {order.ProductId}, orderId = {order.Id}");
                return;
            }

            var data = GetDataInvest(cifCode, order);
            string fullName = null;

            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification?.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer?.Name;
            }

            var orderContractFiles = _dbContext.GarnerOrderContractFiles.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
            string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : order.ContractCode;

            var content = new GarnerPaymentSuccessContent()
            {
                CustomerName = fullName,
                PolicyName = policy?.Name,
                PaymentAmount = NumberToText.ConvertNumberIS((double)orderPayment?.PaymentAmount),
                ContractCode = contractCode,
                TradingProviderName = businessCustomer?.Name,
                TranDate = orderPayment?.TranDate?.ToString("dd/MM/yyyy HH:mm:ss"),
                TranNote = $"{PaymentNotes.THANH_TOAN}{contractCode ?? order?.ContractCode}",
            };
            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.GARNER_CHUYEN_TIEN_THANH_CONG
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.GARNER_CHUYEN_TIEN_THANH_CONG, receiver, data.OtherParams);
        }

        /// <summary>
        /// Gửi thông báo khi tích lũy thành công
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task SendNotifyGarnerOrderActive(long orderId)
        {
            var order = _garnerOrderEFRepository.FindById(orderId);
            if (order == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerOrderActive)}: Không tìm thấy sổ lệnh orderId: {orderId}");
                return;
            }

            if (order.Status != OrderStatus.DANG_DAU_TU)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerOrderActive)}: Lệnh không ở trạng thái đang đầu tư. orderId: {orderId}");
                return;
            }

            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerOrderActive)}: Không tìm thấy mã khách hàng. cifCode: {order.CifCode}");
                return;
            }

            var distribution = _garnerDistributionEFRepository.FindById(order.DistributionId);
            if (distribution == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerOrderActive)}: Không tìm thấy thông tin phân phối Garner. distributionId: {order.DistributionId}, orderId = {orderId}");
                return;
            }

            var product = _garnerProductEFRepository.FindById(order.ProductId);
            if (product == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerOrderActive)}: Không tìm thấy thông tin sản phẩm Garner. ProductId: {order.ProductId}, orderId = {orderId}");
                return;
            }

            var policy = _garnerPolicyEFRepository.FindById(order.PolicyId);
            if (policy == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerOrderActive)}: Không tìm thấy thông tin chính sách Garner. PolicyId: {order.PolicyId}, orderId = {orderId}");
                return;
            }

            var profit = _garnerPolicyDetailEFRepository.GetAllPolicyDetailByPolicyId(order.PolicyId)?.Max(e => e.Profit);
            if (profit == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerOrderActive)}: Không tìm thấy thông tin lợi nhuận kỳ hạn Garner. PolicyId: {order.PolicyId}, orderId = {orderId}");
                return;
            }

            var tradingProvider = _tradingProviderEFRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerOrderActive)}: Không tìm thấy đại lý. tradingProviderId = {order.TradingProviderId}, orderId = {orderId}");
                return;
            }

            var businessCustomer = _businessCustomerEFRepository.FindById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerOrderActive)}: Không tìm thấy business customer là đại lý. tradingProviderId = {order.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }

            //danh sách file hợp đồng
            var contracts = _garnerOrderContractFileEFRepository.FindAll(orderId, order.TradingProviderId);
            if (order.Source == SourceOrder.OFFLINE && order.SaleOrderId == null)
            {
                contracts = new();
            }

            DataEmail data = GetDataInvest(cifCode, order);
            string fullName = null;

            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification?.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer?.Name;
            }

            var orderContractFiles = _dbContext.GarnerOrderContractFiles.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
            string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : order.ContractCode;

            var content = new GarnerOrderSuccessContent()
            {
                ProductCode = product?.Code,
                ProductName = product?.Name,
                CustomerName = fullName,
                TotalValue = NumberToText.ConvertNumberIS((double?)order?.TotalValue),
                ContractCode = contractCode,
                TranDate = order?.BuyDate.ToString("dd/MM/yyyy"),
                PolicyName = policy?.Name,
                Profit = NumberToText.ConvertNumberIS((double?)profit ?? 0) + "%",
                TradingProviderName = businessCustomer?.Name,
            };
            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.GARNER_TICH_LUY_THANH_CONG
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            var attachments = contracts.Select(c => (new Uri(new Uri(_baseUrl), c.FileSignatureUrl)).AbsoluteUri).ToList();
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.GARNER_TICH_LUY_THANH_CONG, receiver, data.OtherParams, attachments);
        }

        /// <summary>
        /// Gửi thông báo khi rút tiền thành công
        /// </summary>
        /// <returns></returns>
        public async Task SendNotifyGarnerOrderWithdraw(long withdrawalId)
        {
            var withdrawal = _garnerWithdrawalEFRepository.FindById(withdrawalId);
            if (withdrawal == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerOrderWithdraw)}: Không tìm thấy yêu cầu rút tiền khi gửi email thông báo rút tiền thành công Garner. orderId: {withdrawalId}");
                return;
            }

            // Số tiền đang còn tích lũy theo CifCode nhà đầu tư của chính sách này
            var amountRemainInPolicy = _garnerOrderEFRepository.Entity.Where(e => e.PolicyId == withdrawal.PolicyId && e.CifCode == withdrawal.CifCode && e.Status == OrderStatus.DANG_DAU_TU && e.Deleted == YesNo.NO).Select(o => o.TotalValue).Sum();

            var cifCode = _cifCodeEFRepository.FindByCifCode(withdrawal.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerOrderWithdraw)}: Không tìm thấy mã khách hàng khi gửi email thông báo rút tiền thành công Garner. cifCode: {withdrawal.CifCode}");
                return;
            }

            var distribution = _garnerDistributionEFRepository.FindById(withdrawal.DistributionId);
            if (distribution == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerOrderWithdraw)}: Không tìm thấy thông tin phân phối Garner. distributionId: {withdrawal.DistributionId}, withdrawalId = {withdrawalId}");
                return;
            }

            var policy = _garnerPolicyEFRepository.FindById(withdrawal.PolicyId);
            if (policy == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerOrderWithdraw)}: Không tìm thấy thông tin chính sách Garner. PolicyId: {withdrawal.PolicyId}, withdrawalId = {withdrawalId}");
                return;
            }

            var tradingProvider = _tradingProviderEFRepository.FindById(withdrawal.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerOrderWithdraw)}: Không tìm thấy đại lý khi gửi email thông báo rút tiền thành công Garner. tradingProviderId = {withdrawal.TradingProviderId}, withdrawalId = {withdrawalId}");
                return;
            }

            var businessCustomer = _businessCustomerEFRepository.FindById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerOrderWithdraw)}: Không tìm thấy business customer là đại lý khi gửi email thông báo rút tiền thành công Garner. tradingProviderId = {tradingProvider.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }

            //danh sách file hợp đồng

            DataEmail data = GetDataInvest(cifCode, withdrawal.TradingProviderId);
            string fullName = null;
            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification?.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer?.Name;
            }

            if (withdrawal.Status == WithdrawalStatus.DUYET_DI_TIEN || withdrawal.Status == WithdrawalStatus.DUYET_KHONG_DI_TIEN)
            {
                var content = new GarnerWithdrawSuccessContent()
                {
                    PolicyName = policy?.Name,
                    CustomerName = fullName,
                    TradingProviderName = businessCustomer?.Name,
                    WithdrawAmount = NumberToText.ConvertNumberIS((double?)withdrawal?.AmountMoney),
                    AmountRemain = NumberToText.ConvertNumberIS((double?)amountRemainInPolicy ?? 0),
                    TranDate = withdrawal.WithdrawalDate.ToString("dd/MM/yyyy"),
                    TranNote = $"RUT {withdrawalId}",
                };
                var receiver = new Receiver
                {
                    Phone = data.Investor?.Phone,
                    Email = new EmailNotifi
                    {
                        Address = data.Investor?.Email,
                        Title = TitleEmail.GARNER_RUT_TIEN_THANH_CONG
                    },
                    UserId = data.Users?.UserId.ToString(),
                    FcmTokens = data.FcmTokens
                };
                await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.GARNER_RUT_TIEN_THANH_CONG, receiver, data.OtherParams);
            }
        }

        /// <summary>
        /// Chi trả lợi nhận thành công
        /// </summary>
        public async Task SendNotifyGarnerInterestPayment(long interestPaymentId)
        {
            var interestPayment = _garnerInterestPaymentEFRepository.FindById(interestPaymentId);
            if (interestPayment == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerInterestPayment)}: Không tìm thấy yêu cầu rút tiền khi gửi email thông báo Chi trả lợi nhuận thành công Garner. orderId: {interestPaymentId}");
                return;
            }

            var cifCode = _cifCodeEFRepository.FindByCifCode(interestPayment.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerInterestPayment)}: Không tìm thấy mã khách hàng khi gửi email thông báo Chi trả lợi nhuận thành công Garner. cifCode: {interestPayment.CifCode}");
                return;
            }

            var policy = _garnerPolicyEFRepository.FindById(interestPayment.PolicyId);
            if (policy == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerInterestPayment)}: Không tìm thấy thông tin chính sách Garner. PolicyId: {interestPayment.PolicyId}, withdrawalId = {interestPaymentId}");
                return;
            }

            var distribution = _garnerDistributionEFRepository.FindById(policy.DistributionId);
            if (distribution == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerInterestPayment)}: Không tìm thấy thông tin phân phối Garner. distributionId: {policy.DistributionId}, withdrawalId = {interestPaymentId}");
                return;
            }

            var tradingProvider = _tradingProviderEFRepository.FindById(interestPayment.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerInterestPayment)}: Không tìm thấy đại lý khi gửi email thông báo Chi trả lợi nhuận thành công Garner. tradingProviderId = {interestPayment.TradingProviderId}, withdrawalId = {interestPaymentId}");
                return;
            }

            var businessCustomer = _businessCustomerEFRepository.FindById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"{nameof(SendNotifyGarnerInterestPayment)}: Không tìm thấy business customer là đại lý khi gửi email thông báo Chi trả lợi nhuận thành công Garner. tradingProviderId = {tradingProvider.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }

            DataEmail data = GetDataInvest(cifCode, interestPayment.TradingProviderId);
            string fullName = null;
            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification?.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer?.Name;
            }

            if (interestPayment.Status == WithdrawalStatus.DUYET_DI_TIEN || interestPayment.Status == WithdrawalStatus.DUYET_KHONG_DI_TIEN)
            {
                var content = new GarnerInterestPaymentSuccessContentDto()
                {
                    PolicyName = policy?.Name,
                    CustomerName = fullName,
                    TradingProviderName = businessCustomer?.Name,
                    AmountMoney = NumberToText.ConvertNumberIS((double?)interestPayment?.AmountMoney),
                    PayDate = interestPayment.PayDate.ToString("dd/MM/yyyy"),
                };
                var receiver = new Receiver
                {
                    Phone = data.Investor?.Phone,
                    Email = new EmailNotifi
                    {
                        Address = data.Investor?.Email,
                        Title = TitleEmail.GARNER_CHI_TRA_THANH_CONG
                    },
                    UserId = data.Users?.UserId.ToString(),
                    FcmTokens = data.FcmTokens
                };
                await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.GARNER_CHI_TRA_THANH_CONG, receiver, data.OtherParams);
            }
        }

        public async Task SendNotifyQRContractDelivery(string deliveryCode, int investorId)
        {
            _logger.LogInformation($"{nameof(SendNotifyQRContractDelivery)}: deliveryCode = {deliveryCode},investorId = {investorId} ");

            var orderFind = (from order in _dbContext.GarnerOrders
                             join cifcode in _dbContext.CifCodes on order.CifCode equals cifcode.CifCode
                             where order.DeliveryCode == deliveryCode && investorId == cifcode.InvestorId && order.Status == OrderStatus.DANG_DAU_TU
                             select order).FirstOrDefault();
            if (orderFind == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.GarnerOrderNotFound);
                return;
            }
            var tradingProvider = _tradingProviderEFRepository.FindById(orderFind.TradingProviderId);
            if (tradingProvider == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.TradingProviderNotFound);
                return;
            }

            var product = _garnerProductEFRepository.FindById(orderFind.ProductId);
            if (product == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.GarnerProductNotFound);
                return;
            }

            var policy = _garnerPolicyEFRepository.FindById(orderFind.PolicyId);
            if (policy == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.GarnerPolicyNotFound);
                return;
            }

            var cifCode = _cifCodeEFRepository.FindByCifCode(orderFind.CifCode);
            if (cifCode == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.CoreCifCodeNotFound);
                return;
            }

            var data = GetDataInvest(cifCode, orderFind);
            string fullName = null;

            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification?.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer?.Name;
            }

            var orderContractFiles = _dbContext.GarnerOrderContractFiles.Where(e => e.OrderId == orderFind.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
            string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : orderFind.ContractCode;

            var content = new GarnerOrderDeliverySuccessContentDto()
            {
                CustomerName = fullName,
                ContractCode = contractCode,
                ReceivedDate = orderFind?.ReceivedDate.Value.ToString("dd/MM/yyyy"),
                DeliveryCode = orderFind.DeliveryCode,
                ProductCode = product?.Code,
                ProductName = product?.Name,
                InitTotalValue = NumberToText.ConvertNumberIS((double?)orderFind.InitTotalValue),
                InvestDate = orderFind?.InvestDate.Value.ToString("dd/MM/yyyy"),
                PolicyName = policy?.Name,
            };

            var receiver = new Receiver
            {
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.GARNER_QR_CONTRACT_DELIVERY
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.GARNER_KHACH_QUET_QR_GIAO_NHAN_HD, receiver, data.OtherParams);
        }

        /// <summary>
        /// Thông báo quản trị khách vào tiền
        /// </summary>
        /// <param name="orderPaymentId"></param>
        /// <returns></returns>
        public async Task SendNotifyAdminCustomerPayment(long orderPaymentId)
        {
            _logger.LogInformation($"{nameof(SendNotifyAdminCustomerPayment)}: orderPaymentId = {orderPaymentId}");
            var paymentFind = _garnerOrderPaymentEFRepository.FindById(orderPaymentId);
            if (paymentFind == null)
            {
                _garnerWithdrawalEFRepository.LogError(ErrorCode.GarnerOrderPaymentNotFound);
                return;
            }
            var order = _garnerOrderEFRepository.FindById(paymentFind.OrderId);
            if (order == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.GarnerOrderNotFound);
                return;
            }
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.CoreCifCodeNotFound);
                return;
            }

            var policy = _garnerPolicyEFRepository.FindById(order.PolicyId);
            if (policy == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.GarnerPolicyNotFound);
                return;
            }

            var profitPolicyDetailByPolicyId = _garnerPolicyDetailEFRepository.GetAllPolicyDetailByPolicyId(order.PolicyId);
            decimal profit = 0;
            if (profitPolicyDetailByPolicyId.Count() > 0)
            {
                profit = profitPolicyDetailByPolicyId.Max(e => e.Profit);
            }
            else
            {
                _logger.LogError($"{nameof(SendNotifyAdminCustomerPayment)}: Không tìm thấy thông tin lợi nhuận kỳ hạn Garner. PolicyId: {order.PolicyId}, orderId = {order.Id}");
                return;
            }

            var tradingProvider = _tradingProviderEFRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.TradingProviderNotFound);
                return;
            }

            var businessCustomer = _businessCustomerEFRepository.FindById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.CoreBusinessCustomerNotFound);
                return;
            }
            var product = _garnerProductEFRepository.FindById(order.ProductId);
            if (product == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.GarnerProductNotFound);
                return;
            }

            var data = GetDataInvest(cifCode, order);
            string fullName = null;

            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification?.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer?.Name;
            }

            var orderContractFiles = _dbContext.GarnerOrderContractFiles.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
            string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : order.ContractCode;

            var content = new GarnerPaymentSuccessContent()
            {
                CustomerName = fullName,
                PolicyName = policy?.Name,
                PaymentAmount = NumberToText.ConvertNumberIS((double)paymentFind?.PaymentAmount),
                ContractCode = contractCode,
                TradingProviderName = businessCustomer?.Name,
                TranDate = paymentFind?.TranDate?.ToString("dd/MM/yyyy HH:mm:ss"),
                TranNote = $"{PaymentNotes.THANH_TOAN}{contractCode}",
            };
            var receiver = new Receiver
            {
                Email = new EmailNotifi
                {
                    Title = TitleEmail.GARNER_ADMIN_KHACH_VAO_TIEN
                },
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.GARNER_ADMIN_KHACH_VAO_TIEN, receiver,
                new ParamsChooseTemplate { TradingProviderId = paymentFind.TradingProviderId });
        }

        /// <summary>
        /// Thông báo quản trị về việc khách rút tiền
        /// </summary>
        /// <returns></returns>
        public async Task SendNotifyAdminCustomerWithdrawal(long withdrawalId)
        {
            _logger.LogInformation($"{nameof(SendNotifyAdminCustomerWithdrawal)}: withdrawalId = {withdrawalId}");
            var withdrawlFind = _garnerWithdrawalEFRepository.FindById(withdrawalId);
            if (withdrawlFind == null)
            {
                _garnerWithdrawalEFRepository.LogError(ErrorCode.GarnerWithdrawalNotFound);
                return;
            }

            // Số tiền đang còn tích lũy theo CifCode nhà đầu tư của chính sách này
            var amountRemainInPolicy = _garnerOrderEFRepository.Entity.Where(e => e.PolicyId == withdrawlFind.PolicyId && e.CifCode == withdrawlFind.CifCode && e.Status == OrderStatus.DANG_DAU_TU && e.Deleted == YesNo.NO).Select(o => o.TotalValue).Sum();

            var cifCode = _cifCodeEFRepository.FindByCifCode(withdrawlFind.CifCode);
            if (cifCode == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.CoreCifCodeNotFound);
                return;
            }

            var distribution = _garnerDistributionEFRepository.FindById(withdrawlFind.DistributionId);
            if (distribution == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.GarnerDistributionNotFound);
                return;
            }

            var policy = _garnerPolicyEFRepository.FindById(withdrawlFind.PolicyId);
            if (policy == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.GarnerPolicyNotFound);
                return;
            }

            var tradingProvider = _tradingProviderEFRepository.FindById(withdrawlFind.TradingProviderId);
            if (tradingProvider == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.GarnerPolicyDetailNotFound);
                return;
            }

            var businessCustomer = _businessCustomerEFRepository.FindById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.CoreBusinessCustomerNotFound);
                return;
            }

            //danh sách file hợp đồng

            DataEmail data = GetDataInvest(cifCode, withdrawlFind.TradingProviderId);
            string fullName = null;
            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification?.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer?.Name;
            }

            var content = new GarnerWithdrawSuccessContent()
            {
                PolicyName = policy?.Name,
                CustomerName = fullName,
                TradingProviderName = businessCustomer?.Name,
                WithdrawAmount = NumberToText.ConvertNumberIS((double?)withdrawlFind?.AmountMoney),
                AmountRemain = NumberToText.ConvertNumberIS((double?)amountRemainInPolicy ?? 0),
                TranDate = withdrawlFind.CreatedDate?.ToString("dd/MM/yyyy HH:mm:ss"),
                TranNote = $"RUT {withdrawalId}",
            };
            var receiver = new Receiver
            {
                Email = new EmailNotifi
                {
                    Title = TitleEmail.GARNER_ADMIN_KHACH_RUT_TIEN
                },
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.GARNER_ADMIN_KHACH_RUT_TIEN, receiver,
                new ParamsChooseTemplate { TradingProviderId = withdrawlFind.TradingProviderId });
        }
    }
}
