using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.User;
using EPIC.IdentityRepositories;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InvestShared;
using EPIC.InvestRepositories;
using EPIC.InvestSharedDomain.Interfaces;
using EPIC.Notification.Dto.EmailInvest;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Notification;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.SharedApiService;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.Notification.Services
{
    public class InvestNotificationServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        #region shared
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly string _baseUrl;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SharedNotificationApiUtils _sharedEmailApiUtils;
        private readonly IOptions<RecognitionApiConfiguration> _recognitionApiConfig;
        #endregion

        #region customer
        private readonly SysVarRepository _sysVarRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly ApproveRepository _approveRepository;
        private readonly UserRepository _userRepository;

        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly PartnerRepository _partnerRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly InvestorBankAccountRepository _investorBankAccountRepository;
        private readonly DepartmentRepository _departmentRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        #endregion
        private readonly ProjectRepository _projectRepository;
        private readonly DistributionRepository _distributionRepository;
        private readonly SaleRepository _saleRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly InvestOrderPaymentRepository _investOrderPaymentRepository;
        private readonly InvestOrderRepository _investOrderRepository;
        private readonly InvestPolicyRepository _investPolicyRepository;
        private readonly OrderContractFileRepository _investOrderContractFileRepository;
        private readonly WithdrawalRepository _withdrawalRepository;
        private readonly InvestContractTemplateEFRepository _investContractTemplateEFRepository;
        private readonly InvestInterestPaymentRepository _investInterestPaymentRepository;
        private readonly IInvestContractCodeServices _investContractCodeServices;
        private readonly InvestInterestPaymentEFRepository _interestPaymentEFRepository;
        private readonly InvestOrderEFRepository _investOrderEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly UsersEFRepository _usersEFRepository;
        private readonly IHttpContextAccessor _httpContext;


        public InvestNotificationServices(ILogger<InvestNotificationServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IOptions<RecognitionApiConfiguration> recognitionApiConfiguration,
            SharedNotificationApiUtils sharedEmailApiUtils,
            IInvestContractCodeServices investContractCodeServices,
            EpicSchemaDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _baseUrl = _configuration["SharedApi:BaseUrl"];
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _recognitionApiConfig = recognitionApiConfiguration;
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _investorBankAccountRepository = new InvestorBankAccountRepository(_connectionString, _logger);
            _approveRepository = new ApproveRepository(_connectionString, _logger);
            _userRepository = new UserRepository(_connectionString, _logger);
            _partnerRepository = new PartnerRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContextAccessor);
            _sysVarRepository = new SysVarRepository(_connectionString, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _sharedEmailApiUtils = sharedEmailApiUtils;
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _departmentRepository = new DepartmentRepository(_connectionString, _logger);
            _investOrderPaymentRepository = new InvestOrderPaymentRepository(_connectionString, _logger);
            _investOrderRepository = new InvestOrderRepository(_connectionString, _logger);
            _investPolicyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _distributionRepository = new DistributionRepository(_connectionString, _logger);
            _projectRepository = new ProjectRepository(_connectionString, _logger);
            _investOrderContractFileRepository = new OrderContractFileRepository(_connectionString, _logger);
            _withdrawalRepository = new WithdrawalRepository(_connectionString, _logger);
            _investContractTemplateEFRepository = new InvestContractTemplateEFRepository(_dbContext, _logger);
            _investInterestPaymentRepository = new InvestInterestPaymentRepository(_connectionString, _logger);
            _investContractCodeServices = investContractCodeServices;
            _interestPaymentEFRepository = new InvestInterestPaymentEFRepository(_dbContext, _logger);
            _investOrderEFRepository = new InvestOrderEFRepository(_dbContext, _logger);
            _defErrorEFRepository = new DefErrorEFRepository(_dbContext);
            _investorEFRepository = new InvestorEFRepository(_dbContext, _logger);
            _usersEFRepository = new UsersEFRepository(_dbContext, _logger);
        }

        #region Send Email/SMS/Push APP Đầu tư invest
        public EmailDataInvest GetDataInvest(CifCodes cifCode, InvOrder order)
        {
            var result = new EmailDataInvest();
            if (cifCode.InvestorId != null)
            {
                result.Investor = _investorRepository.FindById(cifCode.InvestorId ?? 0);
                if (result.Investor == null)
                {
                    _logger.LogError($"{nameof(GetDataInvest)}: Không tìm thấy khách hàng khi gửi email/ sms/ push app. investorId: {cifCode.InvestorId}");
                    return new();
                }
                result.InvestorIdentification = _managerInvestorRepository.GetIdentificationById(order.InvestorIdenId ?? 0);
                if (result.InvestorIdentification == null)
                {
                    _logger.LogError($"Không tìm thấy thông tin giấy tờ khách hàng khi gửi email/ sms/ push app. investorIdentificationId: {order.InvestorIdenId}");
                    return new();
                }

                result.OtherParams = new ParamsChooseTemplate()
                {
                    TradingProviderId = order.TradingProviderId
                };
                var user = _userRepository.FindByInvestorId(result.Investor.InvestorId);
                if (user != null)
                {
                    result.Users = _mapper.Map<UserDto>(user);
                    result.FcmTokens = _userRepository.GetFcmToken(result.Users.UserId);
                }
            }
            else
            {
                result.BusinessCustomer = _businessCustomerRepository.FindBusinessCustomerById(cifCode.BusinessCustomerId ?? 0);
                result.OtherParams = new ParamsChooseTemplate
                {
                    TradingProviderId = order.TradingProviderId
                };
            }
            return result;
        }

        /// <summary>
        /// Thông báo tiền thành công khi duyệt lệnh chuyển tiền
        /// </summary>
        /// <param name="orderPaymentId"></param>
        public async Task SendEmailInvestApprovePayment(long orderPaymentId)
        {
            var orderPayment = _investOrderPaymentRepository.FindById(orderPaymentId);
            if (orderPayment == null)
            {
                _logger.LogError($"{nameof(GetDataInvest)}: Không tìm thấy thông tin thanh toán. orderPaymentId: {orderPaymentId}");
                return;
            }

            var order = _investOrderRepository.FindById(orderPayment.OrderId);
            if (order == null)
            {
                _logger.LogError($"{nameof(GetDataInvest)}: Không tìm thấy sổ lệnh. orderId: {orderPayment.OrderId}");
                return;
            }
            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(GetDataInvest)}: Không tìm thấy mã khách hàng. cifCode: {order.CifCode}");
                return;
            }

            var policy = _investPolicyRepository.FindPolicyById(order.PolicyId);
            if (policy == null)
            {
                _logger.LogError($"{nameof(GetDataInvest)}: Không tìm thấy thông tin chính sách invest. PolicyId: {order.PolicyId}, orderId = {order.Id}");
                return;
            }

            var policyDetail = _investPolicyRepository.FindPolicyDetailById(order.PolicyDetailId);
            if (policyDetail == null)
            {
                _logger.LogError($"{nameof(GetDataInvest)}: Không tim thấy kỳ hạn. policyDetailId: {order.PolicyDetailId}, tradingProviderId: {order.TradingProviderId}");
                return;
            }

            var tradingProvider = _tradingProviderRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(GetDataInvest)}: Không tim thấy đại lý. tradingProviderId = {order.TradingProviderId}, orderPaymentId = {orderPaymentId}");
                return;
            }

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"{nameof(GetDataInvest)}: Không tim thấy business customer là đại lý. tradingProviderId = {order.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }

            var project = _projectRepository.FindById(order.ProjectId);
            if (project == null)
            {
                _logger.LogError($"{nameof(GetDataInvest)}: Không tìm thấy thông tin dự án invest. projectId: {order.ProjectId}, orderId = {order.Id}");
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

            var orderContractFiles = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
            string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : order.ContractCode;

            var content = new InvestPaymentSuccessContent()
            {
                InvCode = project?.InvCode,
                InvName = project?.InvName,
                CustomerName = fullName,
                PaymentAmount = NumberToText.ConvertNumberIS((double?)orderPayment?.PaymentAmnount),
                ContractCode = contractCode,
                TradingProviderName = businessCustomer?.Name,
                TranDate = orderPayment?.TranDate?.ToString("dd/MM/yyyy HH:mm:ss"),
                TranNote = $"{PaymentNotes.THANH_TOAN}{order?.ContractCode}",
            };
            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.CHUYEN_TIEN_THANH_CONG_INVEST
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };

            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.DAU_TU_CHUYEN_TIEN_THANH_CONG_INVEST, receiver, data.OtherParams);
            if (order.Status == InvestOrderStatus.DANG_DAU_TU)
            {
                //danh sách file hợp đồng
                var contracts = _investOrderContractFileRepository.FindAll(order.Id, order.TradingProviderId);
                if (order.Source == SourceOrder.OFFLINE && order.SaleOrderId == null)
                {
                    contracts = new();
                }
                var contentOrderActive = new InvestOrderSuccessContent()
                {
                    InvCode = project?.InvCode,
                    InvName = project?.InvName,
                    CustomerName = fullName,
                    TotalValue = NumberToText.ConvertNumberIS((double?)order?.TotalValue),
                    ContractCode = contractCode,
                    Tenor = $"{policyDetail?.PeriodQuantity} {ContractDataUtils.GetNameDateType(policyDetail?.PeriodType)}",
                    PaymentFullDate = order?.PaymentFullDate?.ToString("dd/MM/yyyy"),
                    PolicyName = policy?.Name,
                    Profit = NumberToText.ConvertNumberIS((double?)policyDetail?.Profit ?? 0) + "%/năm",
                    TradingProviderName = businessCustomer?.Name
                };
                var attachments = contracts.Select(c => new Uri(new Uri(_baseUrl), c.FileSignatureUrl).AbsoluteUri).ToList();
                await _sharedEmailApiUtils.SendEmailAsync(contentOrderActive, KeyTemplate.DAU_TU_THANH_CONG_INVEST, receiver, data.OtherParams, attachments);
            }
        }

        /// <summary>
        /// Send email thông báo đầu tư thành công
        /// </summary>
        /// <param name="orderId"></param>
        public async Task SendEmailInvestOrderActive(long orderId)
        {
            var order = _investOrderRepository.FindById(orderId, null);
            if (order == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderActive)}: Không tìm thấy sổ lệnh. orderId: {orderId}");
                return;
            }

            if (order.Status != InvestOrderStatus.DANG_DAU_TU)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderActive)}: Lệnh không ở trạng thái active. orderId: {orderId}");
                return;
            }

            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderActive)}: Không tìm thấy mã khách hàng. cifCode: {order.CifCode}");
                return;
            }

            var distribution = _distributionRepository.FindById(order.DistributionId);
            if (distribution == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderActive)}: Không tìm thấy thông tin phân phối invest. distributionId: {order.DistributionId}, orderId = {orderId}");
                return;
            }

            var project = _projectRepository.FindById(order.ProjectId);
            if (project == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderActive)}: Không tìm thấy thông tin dự án invest. projectId: {order.ProjectId}, orderId = {orderId}");
                return;
            }

            var policy = _investPolicyRepository.FindPolicyById(order.PolicyId);
            if (policy == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderActive)}: Không tìm thấy thông tin chính sách invest. PolicyId: {order.PolicyId}, orderId = {orderId}");
                return;
            }

            var policyDetail = _investPolicyRepository.FindPolicyDetailById(order.PolicyDetailId);
            if (policyDetail == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderActive)}: Không tìm thấy thông tin kỳ hạn invest. PolicyDetailId: {order.PolicyDetailId}, orderId = {orderId}");
                return;
            }

            var tradingProvider = _tradingProviderRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderActive)}: Không tìm thấy đại lý. tradingProviderId = {order.TradingProviderId}, orderId = {orderId}");
                return;
            }

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderActive)}: Không tìm thấy business customer là đại lý. tradingProviderId = {order.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }

            //danh sách file hợp đồng
            var contracts = _investOrderContractFileRepository.FindAll(orderId, order.TradingProviderId);
            if (order.Source == SourceOrder.OFFLINE && order.SaleOrderId == null)
            {
                contracts = new();
            }

            EmailDataInvest data = GetDataInvest(cifCode, order);
            string fullName = null;
            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification?.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer?.Name;
            }

            var orderContractFiles = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
            string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : order.ContractCode;

            var content = new InvestOrderSuccessContent()
            {
                InvCode = project?.InvCode,
                InvName = project?.InvName,
                CustomerName = fullName,
                TotalValue = NumberToText.ConvertNumberIS((double?)order?.TotalValue),
                ContractCode = contractCode,
                Tenor = $"{policyDetail?.PeriodQuantity} {ContractDataUtils.GetNameDateType(policyDetail?.PeriodType)}",
                PaymentFullDate = order?.PaymentFullDate?.ToString("dd/MM/yyyy"),
                PolicyName = policy?.Name,
                Profit = NumberToText.ConvertNumberIS((double?)policyDetail?.Profit ?? 0) + "%/năm",
                TradingProviderName = businessCustomer?.Name,
            };
            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.DAU_TU_THANH_CONG_INVEST
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            var attachments = contracts.Select(c => (new Uri(new Uri(_baseUrl), c.FileSignatureUrl)).AbsoluteUri).ToList();
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.DAU_TU_THANH_CONG_INVEST, receiver, data.OtherParams, attachments);
        }

        /// <summary>
        /// Send email thông báo đầu tư thành công
        /// </summary>
        /// <param name="orderId"></param>
        public async Task SendNotifySaleInvestOrderActive(long orderId, DateTime investDate)
        {
            var order = _investOrderRepository.FindById(orderId, null);
            if (order == null)
            {
                _logger.LogError($"{nameof(SendNotifySaleInvestOrderActive)}: Không tìm thấy sổ lệnh. orderId: {orderId}");
                return;
            }

            if (order.Status != InvestOrderStatus.DANG_DAU_TU)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderActive)}: Lệnh không ở trạng thái active. orderId: {orderId}");
                return;
            }

            //thông tin sale
            var saleInvestor = _dbContext.Investors.Where(s => s.ReferralCodeSelf == order.SaleReferralCode && s.Deleted == YesNo.NO && s.Status == Status.ACTIVE)
                .Select(investor => new
                {
                    investor.InvestorId,
                    investor.Phone,
                    investor.Email
                })
                .FirstOrDefault();
            if (saleInvestor == null)
            {
                _logger.LogError($"{nameof(SendNotifySaleInvestOrderActive)}: Không tìm thấy thông tin tư vấn viên có mã: {order.SaleReferralCode}");
                return;
            }

            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(SendNotifySaleInvestOrderActive)}: Không tìm thấy mã khách hàng. cifCode: {order.CifCode}");
                return;
            }

            var distribution = _distributionRepository.FindById(order.DistributionId);
            if (distribution == null)
            {
                _logger.LogError($"{nameof(SendNotifySaleInvestOrderActive)}: Không tìm thấy thông tin phân phối invest. distributionId: {order.DistributionId}, orderId = {orderId}");
                return;
            }

            var project = _projectRepository.FindById(order.ProjectId);
            if (project == null)
            {
                _logger.LogError($"{nameof(SendNotifySaleInvestOrderActive)}: Không tìm thấy thông tin dự án invest. projectId: {order.ProjectId}, orderId = {orderId}");
                return;
            }

            var policy = _investPolicyRepository.FindPolicyById(order.PolicyId);
            if (policy == null)
            {
                _logger.LogError($"{nameof(SendNotifySaleInvestOrderActive)}: Không tìm thấy thông tin chính sách invest. PolicyId: {order.PolicyId}, orderId = {orderId}");
                return;
            }

            var policyDetail = _investPolicyRepository.FindPolicyDetailById(order.PolicyDetailId);
            if (policyDetail == null)
            {
                _logger.LogError($"{nameof(SendNotifySaleInvestOrderActive)}: Không tìm thấy thông tin kỳ hạn invest. PolicyDetailId: {order.PolicyDetailId}, orderId = {orderId}");
                return;
            }

            var tradingProvider = _tradingProviderRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(SendNotifySaleInvestOrderActive)}: Không tìm thấy đại lý. tradingProviderId = {order.TradingProviderId}, orderId = {orderId}");
                return;
            }

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"{nameof(SendNotifySaleInvestOrderActive)}: Không tìm thấy business customer là đại lý. tradingProviderId = {order.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }

            //danh sách file hợp đồng
            var contracts = _investOrderContractFileRepository.FindAll(orderId, order.TradingProviderId);
            if (order.Source == SourceOrder.OFFLINE && order.SaleOrderId == null)
            {
                contracts = new();
            }

            EmailDataInvest data = GetDataInvest(cifCode, order);
            string fullName = null;
            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification?.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer?.Name;
            }

            var orderContractFiles = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
            string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : order.ContractCode;

            var content = new InvestSaleOrderActive()
            {
                SaleName = _investorEFRepository.GetDefaultIdentification(saleInvestor.InvestorId)?.Fullname,
                InvCode = project?.InvCode,
                InvName = project?.InvName,
                CustomerName = fullName,
                TotalValue = NumberToText.ConvertNumberIS((double?)order?.TotalValue),
                ContractCode = contractCode,
                Tenor = $"{policyDetail?.PeriodQuantity} {ContractDataUtils.GetNameDateType(policyDetail?.PeriodType)}",
                PaymentFullDate = order?.PaymentFullDate?.ToString("dd/MM/yyyy"),
                InvestDate = investDate.ToString("dd/MM/yyyy HH:mm"),
                PolicyName = policy?.Name,
                Profit = NumberToText.ConvertNumberIS((double?)policyDetail?.Profit ?? 0) + "%/năm",
                TradingProviderName = businessCustomer?.Name,
            };
            var user = _usersEFRepository.FindByInvestorId(saleInvestor.InvestorId);
            var receiver = new Receiver
            {
                Phone = saleInvestor?.Phone,
                Email = new EmailNotifi
                {
                    Address = saleInvestor?.Email,
                    Title = TitleEmail.INVEST_SALE_KHACH_DAU_TU_THANH_CONG
                },
                UserId = user?.UserId.ToString(),
                FcmTokens = user != null ? _usersEFRepository.GetFcmTokenByUserId(user.UserId) : null
            };
            var attachments = contracts.Select(c => (new Uri(new Uri(_baseUrl), c.FileSignatureUrl)).AbsoluteUri).ToList();
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.INVEST_SALE_KHACH_DAU_TU_THANH_CONG, receiver, data.OtherParams, attachments);
        }

        /// <summary>
        /// Gửi mail thông báo hợp đồng đầu tư sắp hết hạn
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task SendEmailInvestOrderSettlement(int orderId)
        {
            var order = _investOrderRepository.FindById(orderId, null);
            if (order == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderSettlement)}: Không tìm thấy sổ lệnh. orderId: {orderId}");
                return;
            }
            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderSettlement)}: Không tìm thấy mã khách hàng. cifCode: {order.CifCode}");
                return;
            }

            var distribution = _distributionRepository.FindById(order.DistributionId);
            if (distribution == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderSettlement)}: Không tìm thấy thông tin phân phối invest. distributionId: {order.DistributionId}, orderId = {orderId}");
                return;
            }

            var project = _projectRepository.FindById(order.ProjectId);
            if (project == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderSettlement)}: Không tìm thấy thông tin dự án invest. projectId: {order.ProjectId}, orderId = {orderId}");
                return;
            }

            var policy = _investPolicyRepository.FindPolicyById(order.PolicyId);
            if (policy == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderSettlement)}: Không tìm thấy thông tin chính sách invest. PolicyId: {order.PolicyId}, orderId = {orderId}");
                return;
            }

            var policyDetail = _investPolicyRepository.FindPolicyDetailById(order.PolicyDetailId);
            if (policyDetail == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderSettlement)}: Không tìm thấy thông tin kỳ hạn invest. PolicyDetailId: {order.PolicyDetailId}, orderId = {orderId}");
                return;
            }

            var tradingProvider = _tradingProviderRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderSettlement)}: Không tìm thấy đại lý. tradingProviderId = {order.TradingProviderId}, orderId = {orderId}");
                return;
            }

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestOrderSettlement)}: Không tìm thấy business customer là đại lý. tradingProviderId = {order.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }

            EmailDataInvest data = GetDataInvest(cifCode, order);
            string fullName = null;
            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification?.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer?.Name;
            }

            if (order.Status == InvestOrderStatus.DANG_DAU_TU)
            {
                var orderContractFiles = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
                string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : order.ContractCode;

                var content = new InvestOrderToExpireDto()
                {
                    InvCode = project?.InvCode,
                    InvName = project?.InvName,
                    CustomerName = fullName,
                    TotalValue = NumberToText.ConvertNumberIS((double?)order?.TotalValue),
                    ContractCode = contractCode,
                    Tenor = $"{policyDetail?.PeriodQuantity} {ContractDataUtils.GetNameDateType(policyDetail?.PeriodType)}",
                    PaymentFullDate = order?.PaymentFullDate?.ToString("dd/MM/yyyy"),
                    PolicyName = policy?.Name,
                    Profit = NumberToText.ConvertNumberIS((double?)policyDetail?.Profit ?? 0) + "%/năm",
                    TradingProviderName = businessCustomer?.Name,
                };
                var receiver = new Receiver
                {
                    Phone = data.Investor?.Phone,
                    Email = new EmailNotifi
                    {
                        Address = data.Investor?.Email,
                        Title = TitleEmail.INVEST_HOP_DONG_DEN_HAN
                    },
                    UserId = data.Users?.UserId.ToString(),
                    FcmTokens = data.FcmTokens
                };
                await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.INVEST_HOP_DONG_DEN_HAN, receiver, data.OtherParams);
            }
        }
        #endregion

        #region Tái tục, rút vốn
        /// <summary>
        /// Tái tục Invest thành công
        /// </summary>
        /// <returns></returns>
        public async Task SendEmailInvestRenewalsSuccess(int interestPaymentId)
        {
            var interestPayment = _investInterestPaymentRepository.FindById(interestPaymentId);
            if (interestPayment == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalsSuccess)}: Không tìm thấy thông tin chi trả {interestPaymentId}");
                return;
            }

            var orderOld = _investOrderRepository.FindById(interestPayment.OrderId);
            if (orderOld == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalsSuccess)}: Không tìm thấy thông tin hợp đồng khi gửi email thông báo tái tục thành công orderId = {interestPayment.OrderId}");
                return;
            }

            var policyOld = _investPolicyRepository.FindPolicyById(orderOld.PolicyId);
            if (policyOld == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalsSuccess)}: Không tìm thấy thông tin chính sách invest. PolicyId: {orderOld.PolicyId}, orderId = {orderOld.Id}");
                return;
            }

            var orderNew = _investOrderEFRepository.Entity.FirstOrDefault(o => o.RenewalsReferId == orderOld.Id && o.Deleted == YesNo.NO);
            if (orderNew == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalsSuccess)}: Không tìm thấy thông tin hợp đồng mới sau táu tục khi gửi email thông báo tái tục thành công orderId = {orderNew.Id}");
                return;
            }

            var cifCode = _cifCodeRepository.GetByCifCode(orderNew.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalsSuccess)}: Không tìm thấy mã khách hàng khi gửi email thông báo tái tục thành công invest. cifCode: {orderNew.CifCode}");
                return;
            }

            var distribution = _distributionRepository.FindById(orderNew.DistributionId);
            if (distribution == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalsSuccess)}: Không tìm thấy thông tin phân phối invest. distributionId: {orderNew.DistributionId}, orderId = {orderNew.Id}");
                return;
            }

            var project = _projectRepository.FindById(orderNew.ProjectId);
            if (project == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalsSuccess)}: Không tìm thấy thông tin dự án invest. projectId: {orderNew.ProjectId}, orderId = {orderNew.Id}");
                return;
            }

            var policy = _investPolicyRepository.FindPolicyById(orderNew.PolicyId);
            if (policy == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalsSuccess)}: Không tìm thấy thông tin chính sách invest. PolicyId: {orderNew.PolicyId}, orderId = {orderNew.Id}");
                return;
            }

            var policyDetail = _investPolicyRepository.FindPolicyDetailById(orderNew.PolicyDetailId);
            if (policyDetail == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalsSuccess)}: Không tìm thấy thông tin kỳ hạn invest. PolicyDetailId: {orderNew.PolicyDetailId}, orderId = {orderNew.Id}");
                return;
            }

            if (orderNew.InvestDate == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalsSuccess)}: Không tìm thấy thông tin ngày đầu tư: {orderNew.InvestDate}");
                return;
            }

            var calculateDueDate = _investOrderRepository.CalculateDueDate(policyDetail, orderNew.InvestDate.Value, distribution.CloseCellDate);

            var tradingProvider = _tradingProviderRepository.FindById(orderNew.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalsSuccess)}: Không tìm thấy đại lý. tradingProviderId = {orderNew.TradingProviderId}, orderId = {interestPayment.OrderId}");
                return;
            }

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalsSuccess)}: Không tìm thấy business customer là đại lý. tradingProviderId = {orderNew.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }

            EmailDataInvest data = GetDataInvest(cifCode, orderNew);
            string fullName = null;
            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer.Name;
            }

            if (orderNew.Status == InvestOrderStatus.DANG_DAU_TU || orderNew.Status == InvestOrderStatus.TAT_TOAN)
            {
                var orderContractFileOlds = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == orderOld.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
                string contractCodeOld = orderContractFileOlds.Count() == 1 ? orderContractFileOlds.First() : orderOld.ContractCode;

                var orderContractFileNews = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == orderNew.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
                string contractCodeNew = orderContractFileNews.Count() == 1 ? orderContractFileNews.First() : orderNew.ContractCode;

                var content = new InvestOrderRenewalsSuccessContent()
                {
                    InvCode = project?.InvCode,
                    InvName = project?.InvName,
                    CustomerName = fullName,
                    TotalValue = NumberToText.ConvertNumberIS((double?)orderNew?.TotalValue),
                    ContractCode = contractCodeOld,
                    ContractCodeNew = contractCodeNew,
                    Tenor = $"{policyDetail?.PeriodQuantity} {ContractDataUtils.GetNameDateType(policyDetail?.PeriodType)}",
                    PaymentFullDate = orderNew?.PaymentFullDate?.ToString("dd/MM/yyyy"),
                    InvestDate = orderNew?.InvestDate?.ToString("dd/MM/yyyy"),
                    DueDate = calculateDueDate.ToString("dd/MM/yyyy"),
                    PolicyName = policy?.Name,
                    Profit = NumberToText.ConvertNumberIS((double?)policyDetail?.Profit ?? 0) + "%/năm",
                    TradingProviderName = businessCustomer?.Name,
                };
                var receiver = new Receiver
                {
                    Phone = data.Investor?.Phone,
                    Email = new EmailNotifi
                    {
                        Address = data.Investor?.Email,
                        Title = TitleEmail.INVEST_TAI_TUC_INVEST
                    },
                    UserId = data.Users?.UserId.ToString(),
                    FcmTokens = data.FcmTokens
                };
                await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.INVEST_TAI_TUC_INVEST, receiver, data.OtherParams);
            }
        }

        /// <summary>
        /// Duyệt chi trả thành công
        /// </summary>
        /// <param name="interestPaymentId"></param>
        /// <returns></returns>
        public async Task SendEmailInvestInterestPaymentSuccess(int interestPaymentId)
        {
            var interestPayment = _investInterestPaymentRepository.FindById(interestPaymentId);
            if (interestPayment == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSuccess)}: Không tìm thấy thông tin chi trả {interestPaymentId}");
                return;
            }

            var order = _investOrderRepository.FindById(interestPayment.OrderId);
            if (order == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSuccess)}: Không tìm thấy thông tin hợp đồng khi gửi email thông báo chi trả thành công orderId = {interestPayment.OrderId}");
                return;
            }

            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSuccess)}: Không tìm thấy mã khách hàng khi gửi email  thông báo chi trả thành công invest. cifCode: {order.CifCode}");
                return;
            }

            var distribution = _distributionRepository.FindById(order.DistributionId);
            if (distribution == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSuccess)}: Không tìm thấy thông tin phân phối invest. distributionId: {order.DistributionId}, orderId = {interestPayment.OrderId}");
                return;
            }

            var project = _projectRepository.FindById(order.ProjectId);
            if (project == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSuccess)}: Không tìm thấy thông tin dự án invest. projectId: {order.ProjectId}, orderId = {interestPayment.OrderId}");
                return;
            }

            var policy = _investPolicyRepository.FindPolicyById(order.PolicyId);
            if (policy == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSuccess)}: Không tìm thấy thông tin chính sách invest. PolicyId: {order.PolicyId}, orderId = {interestPayment.OrderId}");
                return;
            }

            var policyDetail = _investPolicyRepository.FindPolicyDetailById(order.PolicyDetailId);
            if (policyDetail == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSuccess)}: Không tìm thấy thông tin kỳ hạn invest. PolicyDetailId: {order.PolicyDetailId}, orderId = {interestPayment.OrderId}");
                return;
            }

            var tradingProvider = _tradingProviderRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSuccess)}: Không tìm thấy đại lý khi gửi email  thông báo chi trả thành công. tradingProviderId = {order.TradingProviderId}, orderId = {interestPayment.OrderId}");
                return;
            }

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSuccess)}: Không tìm thấy business customer là đại lý khi gửi email  thông báo chi trả thành công. tradingProviderId = {order.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }

            EmailDataInvest data = GetDataInvest(cifCode, order);
            string fullName = null;
            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer.Name;
            }

            if (order.Status == InvestOrderStatus.DANG_DAU_TU || order.Status == InvestOrderStatus.TAT_TOAN)
            {
                var orderContractFiles = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
                string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : order.ContractCode;

                var content = new InvestOrderInterestPaymentSuccessDto()
                {
                    InvCode = project?.InvCode,
                    InvName = project?.InvName,
                    CustomerName = fullName,
                    TotalValue = NumberToText.ConvertNumberIS((double?)order?.TotalValue),
                    ContractCode = contractCode,
                    Tenor = $"{policyDetail?.PeriodQuantity} {ContractDataUtils.GetNameDateType(policyDetail?.PeriodType)}",
                    PaymentFullDate = order?.PaymentFullDate?.ToString("dd/MM/yyyy"),
                    PolicyName = policy?.Name,
                    Profit = NumberToText.ConvertNumberIS((double?)policyDetail?.Profit ?? 0) + "%/năm",
                    TradingProviderName = businessCustomer?.Name,
                    AmountMoneyPay = NumberToText.ConvertNumberIS((double?)interestPayment?.AmountMoney),
                    PayDate = interestPayment?.PayDate?.ToString("dd/MM/yyyy"),
                    PeriodIndex = (interestPayment?.IsLastPeriod == YesNo.YES) ? "Kỳ cuối" : $"Kỳ {interestPayment?.PeriodIndex}"
                };
                var receiver = new Receiver
                {
                    Phone = data.Investor?.Phone,
                    Email = new EmailNotifi
                    {
                        Address = data.Investor?.Email,
                        Title = TitleEmail.CHI_TRA_THANH_CONG_INVEST
                    },
                    UserId = data.Users?.UserId.ToString(),
                    FcmTokens = data.FcmTokens
                };
                await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.INVEST_CHI_TRA_LOI_TUC, receiver, data.OtherParams);
            }
        }

        /// <summary>
        /// Thông báo chi trả tất toán đúng hạn
        /// </summary>
        /// <param name="interestPaymentId"></param>
        /// <returns></returns>
        public async Task SendEmailInvestInterestPaymentSettlementSuccess(int interestPaymentId)
        {
            var interestPayment = _interestPaymentEFRepository.FindById(interestPaymentId);
            if (interestPayment == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSettlementSuccess)}: Không tìm thấy thông tin chi trả {interestPaymentId}");
                return;
            }

            var order = _investOrderRepository.FindById(interestPayment.OrderId);
            if (order == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSettlementSuccess)}: Không tìm thấy thông tin hợp đồng khi gửi email thông báo chi trả thành công orderId = {interestPayment.OrderId}");
                return;
            }

            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSettlementSuccess)}: Không tìm thấy mã khách hàng khi gửi email  thông báo chi trả thành công invest. cifCode: {order.CifCode}");
                return;
            }

            var distribution = _distributionRepository.FindById(order.DistributionId);
            if (distribution == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSettlementSuccess)}: Không tìm thấy thông tin phân phối invest. distributionId: {order.DistributionId}, orderId = {interestPayment.OrderId}");
                return;
            }

            var project = _projectRepository.FindById(order.ProjectId);
            if (project == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSettlementSuccess)}: Không tìm thấy thông tin dự án invest. projectId: {order.ProjectId}, orderId = {interestPayment.OrderId}");
                return;
            }

            var policy = _investPolicyRepository.FindPolicyById(order.PolicyId);
            if (policy == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSettlementSuccess)}: Không tìm thấy thông tin chính sách invest. PolicyId: {order.PolicyId}, orderId = {interestPayment.OrderId}");
                return;
            }

            var policyDetail = _investPolicyRepository.FindPolicyDetailById(order.PolicyDetailId);
            if (policyDetail == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSettlementSuccess)}: Không tìm thấy thông tin kỳ hạn invest. PolicyDetailId: {order.PolicyDetailId}, orderId = {interestPayment.OrderId}");
                return;
            }

            var tradingProvider = _tradingProviderRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSettlementSuccess)}: Không tìm thấy đại lý. tradingProviderId = {order.TradingProviderId}, orderId = {interestPayment.OrderId}");
                return;
            }

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestInterestPaymentSettlementSuccess)}: Không tìm thấy business customer là đại lý. tradingProviderId = {order.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }

            EmailDataInvest data = GetDataInvest(cifCode, order);
            string fullName = null;
            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer.Name;
            }

            if (order.Status == InvestOrderStatus.DANG_DAU_TU || order.Status == InvestOrderStatus.TAT_TOAN)
            {
                var orderContractFiles = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
                string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : order.ContractCode;

                var content = new InvestOrderInterestPaymentSuccessDto()
                {
                    InvCode = project?.InvCode,
                    InvName = project?.InvName,
                    CustomerName = fullName,
                    TotalValue = NumberToText.ConvertNumberIS((double?)order?.TotalValue),
                    InitTotalValue = NumberToText.ConvertNumberIS((double?)order?.InitTotalValue),
                    ContractCode = contractCode,
                    Tenor = $"{policyDetail?.PeriodQuantity} {ContractDataUtils.GetNameDateType(policyDetail?.PeriodType)}",
                    PaymentFullDate = order?.PaymentFullDate?.ToString("dd/MM/yyyy"),
                    PolicyName = policy?.Name,
                    Profit = NumberToText.ConvertNumberIS((double?)policyDetail?.Profit ?? 0) + "%/năm",
                    TradingProviderName = businessCustomer?.Name,
                    AmountMoneyPay = NumberToText.ConvertNumberIS((double?)interestPayment?.AmountMoney),
                    PayDate = interestPayment?.PayDate?.ToString("dd/MM/yyyy"),
                    DueDate = interestPayment?.PayDate?.ToString("dd/MM/yyyy"),
                    InvestDate = order?.InvestDate?.ToString("dd/MM/yyyy"),
                    PeriodIndex = (interestPayment?.IsLastPeriod == YesNo.YES) ? "Kỳ cuối" : $"Kỳ {interestPayment?.PeriodIndex}"
                };
                var receiver = new Receiver
                {
                    Phone = data.Investor?.Phone,
                    Email = new EmailNotifi
                    {
                        Address = data.Investor?.Email,
                        Title = TitleEmail.INVEST_TAT_TOAN_DUNG_HAN
                    },
                    UserId = data.Users?.UserId.ToString(),
                    FcmTokens = data.FcmTokens
                };
                await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.INVEST_TAT_TOAN_DUNG_HAN, receiver, data.OtherParams);
            }
        }

        /// <summary>
        /// Invest thông báo rút vốn thành công
        /// </summary>
        /// <param name="withdrawalId"></param>
        /// <returns></returns>
        public async Task SendEmailInvestWithdrawalSuccess(long withdrawalId)
        {
            var withdrawal = _withdrawalRepository.FindById(withdrawalId);
            if (withdrawal == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestWithdrawalSuccess)}: Không tìm thấy thông tin chi trả {withdrawalId}");
                return;
            }

            var order = _investOrderRepository.FindById(withdrawal.OrderId ?? 0);
            if (order == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestWithdrawalSuccess)}: Không tìm thấy thông tin hợp đồng khi gửi email thông báo rút vốn thành công orderId = {withdrawal.OrderId}");
                return;
            }

            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestWithdrawalSuccess)}: Không tìm thấy mã khách hàng khi gửi email thông báo rút vốn thành công invest. cifCode: {order.CifCode}");
                return;
            }

            var distribution = _distributionRepository.FindById(order.DistributionId);
            if (distribution == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestWithdrawalSuccess)}: Không tìm thấy thông tin phân phối invest. distributionId: {order.DistributionId}, orderId = {withdrawal.OrderId}");
                return;
            }

            var project = _projectRepository.FindById(order.ProjectId);
            if (project == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestWithdrawalSuccess)}: Không tìm thấy thông tin dự án invest. projectId: {order.ProjectId}, orderId = {withdrawal.OrderId}");
                return;
            }

            var policy = _investPolicyRepository.FindPolicyById(order.PolicyId);
            if (policy == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestWithdrawalSuccess)}: Không tìm thấy thông tin chính sách invest. PolicyId: {order.PolicyId}, orderId = {withdrawal.OrderId}");
                return;
            }

            var policyDetail = _investPolicyRepository.FindPolicyDetailById(order.PolicyDetailId);
            if (policyDetail == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestWithdrawalSuccess)}: Không tìm thấy thông tin kỳ hạn invest. PolicyDetailId: {order.PolicyDetailId}, orderId = {withdrawal.OrderId}");
                return;
            }

            var tradingProvider = _tradingProviderRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestWithdrawalSuccess)}: Không tìm thấy đại lý khi gửi email thông báo rút vốn thành công. tradingProviderId = {order.TradingProviderId}, orderId = {withdrawal.OrderId}");
                return;
            }

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestWithdrawalSuccess)}: Không tìm thấy business customer là đại lý khi gửi email thông báo rút vốn thành công. tradingProviderId = {order.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }

            EmailDataInvest data = GetDataInvest(cifCode, order);
            string fullName = null;
            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer.Name;
            }

            if (order.Status == InvestOrderStatus.DANG_DAU_TU || order.Status == InvestOrderStatus.TAT_TOAN)
            {
                var orderContractFiles = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
                string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : order.ContractCode;

                var content = new InvestOrderWithdrawalSuccessContent()
                {
                    InvCode = project?.InvCode,
                    InvName = project?.InvName,
                    CustomerName = fullName,
                    TotalValue = NumberToText.ConvertNumberIS((double?)order?.TotalValue),
                    ContractCode = contractCode,
                    Tenor = $"{policyDetail?.PeriodQuantity} {ContractDataUtils.GetNameDateType(policyDetail?.PeriodType)}",
                    PaymentFullDate = order?.PaymentFullDate?.ToString("dd/MM/yyyy"),
                    PolicyName = policy?.Name,
                    Profit = NumberToText.ConvertNumberIS((double?)policyDetail?.Profit ?? 0) + "%",
                    TradingProviderName = businessCustomer?.Name,
                    WithdrawDate = withdrawal?.WithdrawalDate.ToString("dd/MM/yyyy"),
                    ActuallyAmount = NumberToText.ConvertNumberIS((double?)withdrawal?.ActuallyAmount),
                    WithdrawAmount = NumberToText.ConvertNumberIS((double?)withdrawal?.AmountMoney),
                    AmountRemain = NumberToText.ConvertNumberIS((double?)order?.TotalValue),
                };
                var receiver = new Receiver
                {
                    Phone = data.Investor?.Phone,
                    Email = new EmailNotifi
                    {
                        Address = data.Investor?.Email,
                        Title = TitleEmail.INVEST_RUT_VON_THANH_CONG
                    },
                    UserId = data.Users?.UserId.ToString(),
                    FcmTokens = data.FcmTokens
                };
                await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.INVEST_RUT_VON_THANH_CONG, receiver, data.OtherParams);
            }
        }

        /// <summary>
        /// Thông báo sản phẩm tất toán trước hạn (Rút toàn bộ tiền trong hợp đồng
        /// </summary>
        /// <param name="withdrawalId"></param>
        /// <returns></returns>
        public async Task SendEmailInvestPrePayment(long withdrawalId)
        {
            var withdrawal = _withdrawalRepository.FindById(withdrawalId);
            if (withdrawal == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestPrePayment)}: Không tìm thấy thông tin chi trả {withdrawalId}");
                return;
            }

            var order = _investOrderRepository.FindById(withdrawal.OrderId ?? 0);
            if (order == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestPrePayment)}: Không tìm thấy thông tin hợp đồng khi gửi email thông báo rút vốn thành công orderId = {withdrawal.OrderId}");
                return;
            }

            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestPrePayment)}: Không tìm thấy mã khách hàng khi gửi email thông báo rút vốn thành công invest. cifCode: {order.CifCode}");
                return;
            }

            var distribution = _distributionRepository.FindById(order.DistributionId);
            if (distribution == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestPrePayment)}: Không tìm thấy thông tin phân phối invest. distributionId: {order.DistributionId}, orderId = {withdrawal.OrderId}");
                return;
            }

            var project = _projectRepository.FindById(order.ProjectId);
            if (project == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestPrePayment)}: Không tìm thấy thông tin dự án invest. projectId: {order.ProjectId}, orderId = {withdrawal.OrderId}");
                return;
            }

            var policy = _investPolicyRepository.FindPolicyById(order.PolicyId);
            if (policy == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestPrePayment)}: Không tìm thấy thông tin chính sách invest. PolicyId: {order.PolicyId}, orderId = {withdrawal.OrderId}");
                return;
            }

            var policyDetail = _investPolicyRepository.FindPolicyDetailById(order.PolicyDetailId);
            if (policyDetail == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestPrePayment)}: Không tìm thấy thông tin kỳ hạn invest. PolicyDetailId: {order.PolicyDetailId}, orderId = {withdrawal.OrderId}");
                return;
            }

            var tradingProvider = _tradingProviderRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestPrePayment)}: Không tìm thấy đại lý khi gửi email thông báo rút vốn thành công. tradingProviderId = {order.TradingProviderId}, orderId = {withdrawal.OrderId}");
                return;
            }

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestPrePayment)}: Không tìm thấy business customer là đại lý khi gửi email thông báo rút vốn thành công. tradingProviderId = {order.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }

            EmailDataInvest data = GetDataInvest(cifCode, order);
            string fullName = null;
            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer.Name;
            }

            if (order.Status == InvestOrderStatus.DANG_DAU_TU || order.Status == InvestOrderStatus.TAT_TOAN)
            {
                var orderContractFiles = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
                string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : order.ContractCode;

                var content = new InvestOrderWithdrawalSuccessContent()
                {
                    InvCode = project?.InvCode,
                    InvName = project?.InvName,
                    CustomerName = fullName,
                    TotalValue = NumberToText.ConvertNumberIS((double?)order?.TotalValue),
                    ContractCode = contractCode,
                    Tenor = $"{policyDetail?.PeriodQuantity} {ContractDataUtils.GetNameDateType(policyDetail?.PeriodType)}",
                    PaymentFullDate = order?.PaymentFullDate?.ToString("dd/MM/yyyy"),
                    PolicyName = policy?.Name,
                    Profit = NumberToText.ConvertNumberIS((double?)policyDetail?.Profit ?? 0) + "%",
                    TradingProviderName = businessCustomer?.Name,
                    WithdrawDate = withdrawal?.WithdrawalDate.ToString("dd/MM/yyyy"),
                    WithdrawAmount = NumberToText.ConvertNumberIS((double?)withdrawal?.AmountMoney),
                    ActuallyAmount = NumberToText.ConvertNumberIS((double?)withdrawal?.ActuallyAmount),
                    SettlementDate = order?.SettlementDate?.ToString("dd/MM/yyyy"),
                };
                var receiver = new Receiver
                {
                    Phone = data.Investor?.Phone,
                    Email = new EmailNotifi
                    {
                        Address = data.Investor?.Email,
                        Title = TitleEmail.INVEST_TAT_TOAN_TRUOC_HAN
                    },
                    UserId = data.Users?.UserId.ToString(),
                    FcmTokens = data.FcmTokens
                };
                await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.INVEST_TAT_TOAN_TRUOC_HAN, receiver, data.OtherParams);
            }
        }

        /// <summary>
        /// Thông báo đến quản trị viên có yêu cầu tái tục
        /// </summary>
        /// <param name="renewalRequestId"></param>
        /// <returns></returns>
        public async Task SendEmailInvestRenewalRequest(int renewalRequestId)
        {
            var renewalRequestFind = (from renewalRequest in _dbContext.InvestRenewalsRequests
                                      join investApprove in _dbContext.InvestApproves on renewalRequest.Id equals investApprove.ReferId
                                      where renewalRequest.Id == renewalRequestId && investApprove.DataType == InvestApproveDataTypes.EP_INV_RENEWALS_REQUEST
                                      select new
                                      {
                                          renewalRequest.SettlementMethod,
                                          renewalRequest.OrderId,
                                          renewalRequest.RenewalsPolicyDetailId,
                                          investApprove.RequestDate
                                      }).FirstOrDefault();
            if (renewalRequestFind == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalRequest)}: Không tìm thấy thông tin yêu cầu tái tục {renewalRequestId}");
                return;
            }

            var orderFind = _investOrderEFRepository.Entity.FirstOrDefault(o => o.Id == renewalRequestFind.OrderId && o.Deleted == YesNo.NO);
            if (orderFind == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalRequest)}: Không tìm thấy thông tin hợp đồng khi gửi email thông báo sau khi yêu cầu tái tục orderId = {orderFind.Id}");
                return;
            }

            var cifCode = _cifCodeRepository.GetByCifCode(orderFind.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalRequest)}: Không tìm thấy mã khách hàng khi gửi email thông báo sau khi yêu cầu tái tục invest. cifCode: {orderFind.CifCode}");
                return;
            }

            var project = _projectRepository.FindById(orderFind.ProjectId);
            if (project == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalRequest)}: Không tìm thấy thông tin dự án invest. projectId: {orderFind.ProjectId}, orderId = {orderFind.Id}");
                return;
            }

            var policy = _investPolicyRepository.FindPolicyById(orderFind.PolicyId);
            if (policy == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalRequest)}: Không tìm thấy thông tin chính sách invest. PolicyId: {orderFind.PolicyId}, orderId = {orderFind.Id}");
                return;
            }

            var policyDetail = _investPolicyRepository.FindPolicyDetailById(orderFind.PolicyDetailId);
            if (policyDetail == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalRequest)}: Không tìm thấy thông tin kỳ hạn invest. PolicyDetailId: {orderFind.PolicyDetailId}, orderId = {orderFind.Id}");
                return;
            }

            if (orderFind.InvestDate == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalRequest)}: Không tìm thấy thông tin ngày đầu tư: {orderFind.InvestDate}");
                return;
            }

            var distribution = _dbContext.InvestDistributions.FirstOrDefault(x => x.Id == orderFind.DistributionId && x.Deleted == YesNo.NO);
            if (distribution == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalRequest)}: Không tìm thấy thông tin phan phoi invest. PolicyDetailId: {orderFind.DistributionId}, orderId = {orderFind.Id}");
                return;
            }

            var calculateDueDate = _investOrderRepository.CalculateDueDate(policyDetail, orderFind.InvestDate.Value, distribution.CloseCellDate);

            var tradingProvider = _tradingProviderRepository.FindById(orderFind.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalRequest)}: Không tìm thấy đại lý. tradingProviderId = {orderFind.TradingProviderId}, orderId = {orderFind.Id}");
                return;
            }

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalRequest)}: Không tìm thấy business customer là đại lý. tradingProviderId = {orderFind.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }

            var policyDetailNew = _investPolicyRepository.FindPolicyDetailById(renewalRequestFind.RenewalsPolicyDetailId);
            if (policyDetailNew == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalRequest)}: Không tìm thấy thông tin kỳ hạn mới sau khi yêu cầu tái tục. PolicyDetailId: {renewalRequestFind.RenewalsPolicyDetailId}, orderId = {orderFind.Id}");
                return;
            }


            var policyNew = _investPolicyRepository.FindPolicyById(policyDetailNew.PolicyId);
            if (policyNew == null)
            {
                _logger.LogError($"{nameof(SendEmailInvestRenewalRequest)}: Không tìm thấy thông tin chính sách mới sau khi yêu cầu tái tục. PolicyId: {policyDetailNew.PolicyId}, orderId = {orderFind.Id}");
                return;
            }
            EmailDataInvest data = GetDataInvest(cifCode, orderFind);
            string fullName = null;
            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer.Name;
            }

            if (orderFind.Status == InvestOrderStatus.DANG_DAU_TU || orderFind.Status == InvestOrderStatus.TAT_TOAN)
            {
                var orderContractFiles = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == orderFind.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
                string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : orderFind.ContractCode;

                var content = new InvestOrderRenewalRequestContent()
                {
                    InvCode = project?.InvCode,
                    InvName = project?.InvName,
                    CustomerName = fullName,
                    TotalValue = NumberToText.ConvertNumberIS((double?)orderFind?.TotalValue),
                    ContractCode = contractCode,
                    Tenor = $"{policyDetail?.PeriodQuantity} {ContractDataUtils.GetNameDateType(policyDetail?.PeriodType)}",
                    PaymentFullDate = orderFind?.PaymentFullDate?.ToString("dd/MM/yyyy"),
                    InvestDate = orderFind?.InvestDate?.ToString("dd/MM/yyyy"),
                    DueDate = calculateDueDate.ToString("dd/MM/yyyy"),
                    RequestDate = renewalRequestFind?.RequestDate?.ToString("dd/MM/yyyy"),
                    PolicyName = policy?.Name,
                    Profit = NumberToText.ConvertNumberIS((double?)policyDetail?.Profit ?? 0) + "%",
                    TradingProviderName = businessCustomer?.Name,
                    SettlementMethodName = SettlementMethod.Name(renewalRequestFind.SettlementMethod),
                    NewTenor = $"{policyDetailNew?.PeriodQuantity} {ContractDataUtils.GetNameDateType(policyDetailNew?.PeriodType)}",
                    NewPolicyName = policyNew.Name,
                };
                var receiver = new Receiver
                {
                    Email = new EmailNotifi
                    {
                        Title = TitleEmail.INVEST_YEU_CAU_TAI_TUC
                    },
                };
                await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.INVEST_YEU_CAU_TAI_TUC, receiver,
                new ParamsChooseTemplate { TradingProviderId = orderFind.TradingProviderId });
            }
        }

        /// <summary>
        /// Thông báo đến hạn thanh toán
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="dueDate"></param>
        /// <returns></returns>
        public async Task SendEmailNoticePaymentDueDate(long? orderId, DateTime? dueDate)
        {
            var order = _investOrderRepository.FindById(orderId ?? 0);
            if (order == null)
            {
                _logger.LogError($"{nameof(SendEmailNoticePaymentDueDate)}: Không tìm thấy thông tin hợp đồng khi gửi email thông báo đến hạn thanh toán orderId = {orderId}");
                return;
            }

            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(SendEmailNoticePaymentDueDate)}: Không tìm thấy mã khách hàng khi gửi email thông báo rút vốn đến hạn thanh toán invest. cifCode: {order.CifCode}");
                return;
            }

            var distribution = _distributionRepository.FindById(order.DistributionId);
            if (distribution == null)
            {
                _logger.LogError($"{nameof(SendEmailNoticePaymentDueDate)}: Không tìm thấy thông tin phân phối invest. distributionId: {order.DistributionId}, orderId = {orderId}");
                return;
            }

            var policy = _investPolicyRepository.FindPolicyById(order.PolicyId);
            if (policy == null)
            {
                _logger.LogError($"{nameof(SendEmailNoticePaymentDueDate)}: Không tìm thấy thông tin chính sách invest. PolicyId: {order.PolicyId}, orderId = {orderId}");
                return;
            }

            var policyDetail = _investPolicyRepository.FindPolicyDetailById(order.PolicyDetailId);
            if (policyDetail == null)
            {
                _logger.LogError($"{nameof(SendEmailNoticePaymentDueDate)}: Không tìm thấy thông tin kỳ hạn invest. PolicyDetailId: {order.PolicyDetailId}, orderId = {orderId}");
                return;
            }

            var tradingProvider = _tradingProviderRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(SendEmailNoticePaymentDueDate)}: Không tìm thấy đại lý khi gửi email thông báo rút vốn thành công. tradingProviderId = {order.TradingProviderId}, orderId = {orderId}");
                return;
            }

            var project = _projectRepository.FindById(order.ProjectId);
            if (project == null)
            {
                _logger.LogError($"{nameof(SendEmailNoticePaymentDueDate)}: Không tìm thấy thông tin dự án invest. projectId: {order.ProjectId}, orderId = {orderId}");
                return;
            }

            EmailDataInvest data = GetDataInvest(cifCode, order);
            string fullName = null;
            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer.Name;
            }

            if (order.Status == InvestOrderStatus.DANG_DAU_TU || order.Status == InvestOrderStatus.TAT_TOAN)
            {
                var orderContractFiles = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
                string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : order.ContractCode;

                var content = new InvestNoticePaymentDueDateContent()
                {
                    InvName = project?.InvName,
                    CustomerName = fullName,
                    ContractCode = contractCode,
                    DueDate = dueDate?.ToString("dd/MM/yyyy"),
                };
                var receiver = new Receiver
                {
                    Phone = data.Investor?.Phone,
                    Email = new EmailNotifi
                    {
                        Address = data.Investor?.Email,
                        Title = TitleEmail.INVEST_HOP_DONG_DEN_HAN
                    },
                    UserId = data.Users?.UserId.ToString(),
                    FcmTokens = data.FcmTokens
                };
                await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.INVEST_TK_INVEST_SAP_HET_HAN, receiver, data.OtherParams);
            }
        }
        #endregion

        #region thông báo quản trị
        /// <summary>
        /// Thông báo quản trị khách vào tiền
        /// </summary>
        /// <param name="orderPaymentId"></param>
        /// <returns></returns>
        public async Task SendNotifyAdminCustomerPayment(long orderPaymentId)
        {
            _logger.LogInformation($"{nameof(SendNotifyAdminCustomerPayment)}: orderPaymentId = {orderPaymentId}");
            var paymentFind = _investOrderPaymentRepository.FindById(orderPaymentId);
            if (paymentFind == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.InvestOrderPaymentNotFound);
                return;
            }

            var order = _investOrderRepository.FindById(paymentFind.OrderId);
            if (order == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.InvestOrderNotFound);
                return;
            }
            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.CoreCifCodeNotFound);
                return;
            }

            var policy = _investPolicyRepository.FindPolicyById(order.PolicyId);
            if (policy == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.InvestPolicyNotFound);
                return;
            }

            var policyDetail = _investPolicyRepository.FindPolicyDetailById(order.PolicyDetailId);
            if (policyDetail == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.InvestPolicyDetailNotFound);
                return;
            }

            var tradingProvider = _tradingProviderRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.TradingProviderNotFound);
                return;
            }

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.CoreBussinessCustomerNotFound);
                return;
            }

            var project = _projectRepository.FindById(order.ProjectId);
            if (project == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.InvestProjectNotFound);
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

            var orderContractFiles = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
            string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : order.ContractCode;

            var content = new InvestPaymentSuccessContent()
            {
                InvCode = project?.InvCode,
                InvName = project?.InvName,
                CustomerName = fullName,
                PaymentAmount = NumberToText.ConvertNumberIS((double?)paymentFind?.PaymentAmnount),
                ContractCode = contractCode,
                TradingProviderName = businessCustomer?.Name,
                TranDate = paymentFind?.TranDate?.ToString("dd/MM/yyyy HH:mm:ss"),
                TranNote = $"{PaymentNotes.THANH_TOAN}{order?.ContractCode}",
            };

            var receiver = new Receiver
            {
                Email = new EmailNotifi
                {
                    Title = TitleEmail.INVEST_ADMIN_KHACH_VAO_TIEN
                },
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.INVEST_ADMIN_KHACH_VAO_TIEN, receiver,
                new ParamsChooseTemplate { TradingProviderId = paymentFind.TradingProviderId });
        }

        /// <summary>
        /// Thông báo quản trị về việc khách rút tiền
        /// </summary>
        /// <returns></returns>
        public async Task SendNotifyAdminCustomerWithdrawal(long withdrawalId)
        {
            _logger.LogInformation($"{nameof(SendNotifyAdminCustomerWithdrawal)}: withdrawalId = {withdrawalId}");
            var withdrawalFind = _withdrawalRepository.FindById(withdrawalId);
            if (withdrawalFind == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.InvestWithdrawalNotFound);
                return;
            }

            var order = _investOrderRepository.FindById(withdrawalFind.OrderId ?? 0);
            if (order == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.InvestOrderNotFound);
                return;
            }
            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.CoreCifCodeNotFound);
                return;
            }

            var policy = _investPolicyRepository.FindPolicyById(order.PolicyId);
            if (policy == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.InvestPolicyNotFound);
                return;
            }

            var policyDetail = _investPolicyRepository.FindPolicyDetailById(order.PolicyDetailId);
            if (policyDetail == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.InvestPolicyDetailNotFound);
                return;
            }

            var tradingProvider = _tradingProviderRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.TradingProviderNotFound);
                return;
            }

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.CoreBussinessCustomerNotFound);
                return;
            }

            var project = _projectRepository.FindById(order.ProjectId);
            if (project == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.InvestProjectNotFound);
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

            var orderContractFiles = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
            string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : order.ContractCode;

            var content = new InvestOrderWithdrawalSuccessContent()
            {
                InvCode = project?.InvCode,
                InvName = project?.InvName,
                CustomerName = fullName,
                TotalValue = NumberToText.ConvertNumberIS((double?)order?.TotalValue),
                ContractCode = contractCode,
                Tenor = $"{policyDetail?.PeriodQuantity} {ContractDataUtils.GetNameDateType(policyDetail?.PeriodType)}",
                PaymentFullDate = order?.PaymentFullDate?.ToString("dd/MM/yyyy"),
                PolicyName = policy?.Name,
                Profit = NumberToText.ConvertNumberIS((double?)policyDetail?.Profit ?? 0) + "%",
                TradingProviderName = businessCustomer?.Name,
                WithdrawDate = withdrawalFind?.WithdrawalDate.ToString("dd/MM/yyyy"),
                WithdrawAmount = NumberToText.ConvertNumberIS((double?)withdrawalFind?.AmountMoney),
                ActuallyAmount = NumberToText.ConvertNumberIS((double?)withdrawalFind?.ActuallyAmount),
                SettlementDate = order?.SettlementDate?.ToString("dd/MM/yyyy"),
                AmountRemain = NumberToText.ConvertNumberIS((double?)order?.TotalValue),
            };
            var receiver = new Receiver
            {
                Email = new EmailNotifi
                {
                    Title = TitleEmail.INVEST_ADMIN_KHACH_RUT_TIEN
                },
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.INVEST_ADMIN_KHACH_RUT_TIEN, receiver,
                new ParamsChooseTemplate { TradingProviderId = withdrawalFind.TradingProviderId });
        }

        /// <summary>
        /// Thông báo khách quét QR giao nhận hợp đồng thành công
        /// </summary>
        /// <returns></returns>
        public async Task SendNotifyQRContractDelivery(string deliveryCode)
        {
            _logger.LogInformation($"{nameof(SendNotifyQRContractDelivery)}: deliveryCode = {deliveryCode}");

            var orderFind = _dbContext.InvOrders.FirstOrDefault(r => r.DeliveryCode == deliveryCode && r.ReceivedDate != null && r.Deleted == YesNo.NO);
            if (orderFind == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.InvestOrderNotFound);
                return;
            }
            var tradingProvider = _tradingProviderRepository.FindById(orderFind.TradingProviderId);
            if (tradingProvider == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.TradingProviderNotFound);
                return;
            }

            var project = _projectRepository.FindById(orderFind.ProjectId);
            if (project == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.InvestProjectNotFound);
                return;
            }

            var policy = _investPolicyRepository.FindPolicyById(orderFind.PolicyId);
            if (policy == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.InvestPolicyNotFound);
                return;
            }

            var cifCode = _cifCodeRepository.GetByCifCode(orderFind.CifCode);
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

            var orderContractFiles = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == orderFind.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
            string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : orderFind.ContractCode;

            var content = new InvestOrderSuccessDelivery()
            {
                InvestDate = orderFind?.InvestDate.Value.ToString("dd/MM/yyyy"),
                CustomerName = fullName,
                ContractCode = contractCode,
                InvName = project.InvName,
                ReceivedDate = orderFind.ReceivedDate.Value.ToString("dd/MM/yyyy"),
                InitTotalValue = NumberToText.ConvertNumberIS((double?)orderFind.InitTotalValue),
                DeliveryCode = orderFind.DeliveryCode,
            };
            var receiver = new Receiver
            {
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.INVEST_QR_CONTRACT_DELIVERY
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.INVEST_KHACH_QUET_QR_GIAO_NHAN_HD, receiver, data.OtherParams);
        }
        #endregion
    }
}
