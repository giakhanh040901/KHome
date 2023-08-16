using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.User;
using EPIC.IdentityRepositories;
using EPIC.Notification.Dto.DataEmail;
using EPIC.Notification.Dto.RealEstateNotification;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstGenContractCode;
using EPIC.RealEstateRepositories;
using EPIC.RealEstateSharedDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Notification;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.SharedApiService;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.Notification.Services
{
    public class RealEstateNotificationServices
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly UsersEFRepository _usersEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstOrderEFRepository _rstOrderEFRepository;
        private readonly RstOrderPaymentEFRepository _rstOrderPaymentEFRepository;
        private readonly string _baseUrl;
        private readonly RstProductItemEFRepository _rstProductItemEFRepository;
        private readonly RstOpenSellDetailEFRepository _rstOpenSellDetailEFRepository;
        private readonly RstProjectEFRepository _rstProjectEFRepository;
        private readonly RstDistributionPolicyEFRepository _rstDistributionPolicyEFRepository;
        private readonly IRstContractCodeServices _rstContractCodeServices;

        //Common
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SharedNotificationApiUtils _sharedEmailApiUtils;
        private readonly IConfiguration _configuration;


        public RealEstateNotificationServices(EpicSchemaDbContext dbContext,
            ILogger<RealEstateNotificationServices> logger,
            IMapper mapper,
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor,
            SharedNotificationApiUtils sharedEmailApiUtils,
            IRstContractCodeServices rstContractCodeServices,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _env = env;

            _cifCodeEFRepository = new CifCodeEFRepository(_dbContext, _logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(_dbContext, _logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(_dbContext, _logger);
            _investorEFRepository = new InvestorEFRepository(_dbContext, _logger);
            _usersEFRepository = new UsersEFRepository(_dbContext, _logger);
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstOrderEFRepository = new RstOrderEFRepository(dbContext, logger);
            _rstOrderPaymentEFRepository = new RstOrderPaymentEFRepository(dbContext, logger);
            _rstProductItemEFRepository = new RstProductItemEFRepository(dbContext, logger);
            _rstOpenSellDetailEFRepository = new RstOpenSellDetailEFRepository(dbContext, logger);
            _rstDistributionPolicyEFRepository = new RstDistributionPolicyEFRepository(dbContext, logger);
            _rstProjectEFRepository = new RstProjectEFRepository(dbContext, logger);
            _rstContractCodeServices = rstContractCodeServices;

            //Common
            _httpContextAccessor = httpContextAccessor;
            _sharedEmailApiUtils = sharedEmailApiUtils;
            _configuration = configuration;
            _baseUrl = _configuration["SharedApi:BaseUrl"];
        }

        public DataEmail GetDataInvestor(CifCodes cifCode, RstOrder order)
        {
            var result = new DataEmail();
            if (cifCode.InvestorId != null)
            {
                result.Investor = _investorEFRepository.FindById(cifCode.InvestorId ?? 0);
                if (result.Investor == null)
                {
                    _logger.LogError($"{nameof(GetDataInvestor)}: Không tìm thấy khách hàng khi gửi email/ sms/ push app. investorId: {cifCode.InvestorId}");
                    return new();
                }
                result.InvestorIdentification = _investorEFRepository.GetIdentificationById(order.InvestorIdenId ?? 0);
                if (result.InvestorIdentification == null)
                {
                    _logger.LogError($"{nameof(GetDataInvestor)}: Không tìm thấy thông tin giấy tờ khách hàng khi gửi email/ sms/ push app. investorIdentificationId: {order.InvestorIdenId}");
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

        public DataEmail GetDataInvestor(CifCodes cifCode, int? tradingProviderId)
        {
            var result = new DataEmail();
            if (cifCode.InvestorId != null)
            {
                result.Investor = _investorEFRepository.FindById(cifCode.InvestorId ?? 0);
                if (result.Investor == null)
                {
                    _logger.LogError($"{nameof(GetDataInvestor)}: Không tìm thấy khách hàng khi gửi email/ sms/ push app. investorId: {cifCode.InvestorId}");
                    return new();
                }
                result.InvestorIdentification = _investorEFRepository.GetDefaultIdentification(cifCode.InvestorId ?? 0);
                if (result.InvestorIdentification == null)
                {
                    _logger.LogError($"{nameof(GetDataInvestor)}: Không tìm thấy thông tin giấy tờ khách hàng khi gửi email/ sms/ push app. InvestorId: {cifCode.InvestorId}");
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
        public async Task SendNotifyRstApprovePayment(int orderPaymentId)
        {
            _logger.LogInformation($"{nameof(SendNotifyRstApprovePayment)}: orderPaymentId: {orderPaymentId}");
            var orderPayment = _rstOrderPaymentEFRepository.FindById(orderPaymentId);
            if (orderPayment == null)
            {
                _logger.LogError($"{nameof(SendNotifyRstApprovePayment)}: Không tìm thấy thông tin thanh toán khi thông báo chuyển tiền thành công. orderPaymentId: {orderPaymentId}");
                return;
            }
            // Số tiền đã thanh toán cọc
            var paymentMoney = _rstOrderPaymentEFRepository.SumPaymentDepositAmount(orderPayment.OrderId);
            var order = _rstOrderEFRepository.FindById(orderPayment.OrderId);
            if (order == null)
            {
                _logger.LogError($"{nameof(SendNotifyRstApprovePayment)}: Không tìm thấy sổ lệnh khi thông báo chuyển tiền thành công. orderId: {orderPayment.OrderId}");
                return;
            }
            var openSellInfo = _rstOpenSellDetailEFRepository.OpenSellDetailInfo(order.OpenSellDetailId);
            if (openSellInfo == null)
            {
                _logger.LogError($"{nameof(SendNotifyRstOrderActive)}: Không tìm thấy thông tin sản phẩm mở bán order.OpenSellDetailId: {order.OpenSellDetailId}");
                return;
            }

            var productItem = _rstProductItemEFRepository.FindById(order.ProductItemId);
            if (productItem == null)
            {
                _logger.LogError($"{nameof(SendNotifyRstOrderActive)}: Không tìm thấy thông tin căn hộ order.ProductItemId: {order.ProductItemId}");
                return;
            }
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(SendNotifyRstApprovePayment)}: Không tìm thấy mã khách hàng khi thông báo chuyển tiền thành công. cifCode: {order.CifCode}");
                return;
            }

            var tradingProvider = _tradingProviderEFRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(SendNotifyRstApprovePayment)}: Không tim thấy đại lý khi gửi email thông báo chuyển tiền thành công. tradingProviderId = {order.TradingProviderId}, orderPaymentId = {orderPaymentId}");
                return;
            }

            var businessCustomer = _businessCustomerEFRepository.FindById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"{nameof(SendNotifyRstApprovePayment)}: Không tim thấy business customer là đại lý khi gửi email thông báo chuyển tiền thành công. tradingProviderId = {order.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }

            var data = GetDataInvestor(cifCode, order);
            string fullName = null;

            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification?.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer?.Name;
            }

            var content = new RstOrderPaymentSuccessContent()
            {
                CustomerName = fullName,
                PaymentAmount = NumberToText.ConvertNumberIS((double)orderPayment?.PaymentAmount),
                ProjectName = openSellInfo.ProjectName,
                ProductItemCode = openSellInfo.Code,
                ContractCode = order?.ContractCode,
                TradingProviderName = businessCustomer?.Name,
                TranDate = orderPayment?.TranDate?.ToString("dd/MM/yyyy HH:mm:ss"),
                TranNote = $"{PaymentNotes.THANH_TOAN}{order?.ContractCode}",
                DepositMoney = NumberToText.ConvertNumberIS((double?)order?.DepositMoney),
                PaymentMoney = NumberToText.ConvertNumberIS((double?)paymentMoney)
            };
            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.REAL_ESTATE_CHUYEN_TIEN_THANH_CONG
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.REAL_ESTATE_CHUYEN_TIEN_THANH_CONG, receiver, data.OtherParams);
        }

        /// <summary>
        /// Gửi thông báo khi đặt cọc thành công
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task SendNotifyRstOrderActive(int orderId)
        {
            var order = _rstOrderEFRepository.FindById(orderId);
            if (order == null)
            {
                _logger.LogError($"{nameof(SendNotifyRstOrderActive)}: Không tìm thấy sổ lệnh orderId: {orderId}");
                return;
            }

            var openSellInfo = _rstOpenSellDetailEFRepository.OpenSellDetailInfo(order.OpenSellDetailId);
            if (openSellInfo == null)
            {
                _logger.LogError($"{nameof(SendNotifyRstOrderActive)}: Không tìm thấy thông tin sản phẩm mở bán order.OpenSellDetailId: {order.OpenSellDetailId}");
                return;
            }

            var productItem = _rstProductItemEFRepository.FindById(order.ProductItemId);
            if (productItem == null)
            {
                _logger.LogError($"{nameof(SendNotifyRstOrderActive)}: Không tìm thấy thông tin căn hộ order.ProductItemId: {order.ProductItemId}");
                return;
            }

            if (order.Status != OrderStatus.DANG_DAU_TU)
            {
                _logger.LogError($"{nameof(SendNotifyRstOrderActive)}: Lệnh không ở trạng thái đã cọc. orderId: {orderId}");
                return;
            }

            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(SendNotifyRstOrderActive)}: Không tìm thấy mã khách hàng. cifCode: {order.CifCode}");
                return;
            }

            var tradingProvider = _tradingProviderEFRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(SendNotifyRstOrderActive)}: Không tìm thấy đại lý. tradingProviderId = {order.TradingProviderId}, orderId = {orderId}");
                return;
            }

            var businessCustomer = _businessCustomerEFRepository.FindById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"{nameof(SendNotifyRstOrderActive)}: Không tìm thấy business customer là đại lý. tradingProviderId = {order.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }
            // Số tiền đã thanh toán cọc
            var paymentMoney = _rstOrderPaymentEFRepository.SumPaymentDepositAmount(order.Id);
            DataEmail data = GetDataInvestor(cifCode, order);
            string fullName = null;

            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification?.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer?.Name;
            }

            var orderContractFiles = _dbContext.RstOrderContractFiles.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
            string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : order.ContractCode;

            var content = new RstOrderActiveSuccessContent()
            {
                CustomerName = fullName,
                ProjectName = openSellInfo.ProjectName,
                DepositMoney = NumberToText.ConvertNumberIS((double?)order?.DepositMoney),
                ProductItemCode = productItem?.Code,
                ProductItemPrice = NumberToText.ConvertNumberIS((double?)productItem?.Price),
                ContractCode = contractCode,
                DepositDate = order?.DepositDate?.ToString("dd/MM/yyyy"),
                TradingProviderName = businessCustomer?.Name,
                PaymentMoney = NumberToText.ConvertNumberIS((double?)paymentMoney)
            };
            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.REAL_ESTATE_DAT_COC_THANH_CONG
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.REAL_ESTATE_DAT_COC_THANH_CONG, receiver, data.OtherParams);
        }

        public async Task SendNotifySaleOrderActive(int orderId)
        {
            var order = _rstOrderEFRepository.FindById(orderId);
            if (order == null)
            {
                _logger.LogError($"{nameof(SendNotifySaleOrderActive)}: Không tìm thấy sổ lệnh orderId: {orderId}");
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
                _logger.LogError($"{nameof(SendNotifySaleOrderActive)}: Không tìm thấy thông tin tư vấn viên có mã: {order.SaleReferralCode}");
                return;
            }

            var openSellInfo = _rstOpenSellDetailEFRepository.OpenSellDetailInfo(order.OpenSellDetailId);
            if (openSellInfo == null)
            {
                _logger.LogError($"{nameof(SendNotifySaleOrderActive)}: Không tìm thấy thông tin sản phẩm mở bán order.OpenSellDetailId: {order.OpenSellDetailId}");
                return;
            }

            var productItem = _rstProductItemEFRepository.FindById(order.ProductItemId);
            if (productItem == null)
            {
                _logger.LogError($"{nameof(SendNotifySaleOrderActive)}: Không tìm thấy thông tin căn hộ order.ProductItemId: {order.ProductItemId}");
                return;
            }

            if (order.Status != OrderStatus.DANG_DAU_TU)
            {
                _logger.LogError($"{nameof(SendNotifySaleOrderActive)}: Lệnh không ở trạng thái đã cọc. orderId: {orderId}");
                return;
            }

            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"{nameof(SendNotifySaleOrderActive)}: Không tìm thấy mã khách hàng. cifCode: {order.CifCode}");
                return;
            }

            var tradingProvider = _tradingProviderEFRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"{nameof(SendNotifySaleOrderActive)}: Không tìm thấy đại lý. tradingProviderId = {order.TradingProviderId}, orderId = {orderId}");
                return;
            }

            var businessCustomer = _businessCustomerEFRepository.FindById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"{nameof(SendNotifySaleOrderActive)}: Không tìm thấy business customer là đại lý. tradingProviderId = {order.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }

            // Số tiền đã thanh toán cọc
            var paymentMoney = _rstOrderPaymentEFRepository.SumPaymentDepositAmount(order.Id);
            DataEmail data = GetDataInvestor(cifCode, order);
            string fullName = null;

            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification?.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer?.Name;
            }

            var orderContractFiles = _dbContext.RstOrderContractFiles.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
            string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : order.ContractCode;

            var content = new RstSaleOrderActiveSuccessContent()
            {
                SaleName = _investorEFRepository.GetDefaultIdentification(saleInvestor.InvestorId)?.Fullname,
                CustomerName = fullName,
                ProjectName = openSellInfo.ProjectName,
                DepositMoney = NumberToText.ConvertNumberIS((double?)order?.DepositMoney),
                ProductItemCode = productItem?.Code,
                ProductItemPrice = NumberToText.ConvertNumberIS((double?)productItem?.Price),
                ContractCode = contractCode,
                DepositDate = order?.DepositDate?.ToString("dd/MM/yyyy"),
                TradingProviderName = businessCustomer?.Name,
                PaymentMoney = NumberToText.ConvertNumberIS((double?)paymentMoney),
                ApproveDate = order.ApproveDate?.ToString("dd/MM/yyyy")
            };

            var user = _usersEFRepository.FindByInvestorId(saleInvestor.InvestorId);

            var receiver = new Receiver
            {
                Phone = saleInvestor?.Phone,
                Email = new EmailNotifi
                {
                    Address = saleInvestor?.Email,
                    Title = TitleEmail.REAL_ESTATE_SALE_KHACH_DAT_COC_THANH_CONG
                },
                UserId = user?.UserId.ToString(),
                FcmTokens = user != null ? _usersEFRepository.GetFcmTokenByUserId(user.UserId) : null
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.REAL_ESTATE_SALE_KHACH_DAT_COC_THANH_CONG, receiver, data.OtherParams);
        }
        #region thông báo quản trị
        /// <summary>
        /// Thông báo quản trị khách vào tiền
        /// </summary>
        /// <param name="orderPaymentId"></param>
        /// <returns></returns>
        public async Task SendNotifyAdminCustomerPayment(int orderPaymentId)
        {
            _logger.LogInformation($"{nameof(SendNotifyAdminCustomerPayment)}: orderPaymentId = {orderPaymentId}");
            var paymentFind = _rstOrderPaymentEFRepository.FindById(orderPaymentId);
            if (paymentFind == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.InvestOrderPaymentNotFound);
                return;
            }

            var order = _rstOrderEFRepository.FindById(paymentFind.OrderId);
            if (order == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.InvestOrderNotFound);
                return;
            }
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.CoreCifCodeNotFound);
                return;
            }

            var distributionPolicy = _rstDistributionPolicyEFRepository.FindById(order.DistributionPolicyId);
            if (distributionPolicy == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.RstDistributionPolicyNotFound);
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
            var productItem = _rstProductItemEFRepository.FindById(order.ProductItemId);
            if (productItem == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.RstProductItemNotFound);
                return;
            }
            var project = _rstProjectEFRepository.FindById(productItem.ProjectId);
            if (project == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.RstProjectNotFound);
                return;
            }

            var data = GetDataInvestor(cifCode, order);
            string fullName = null;

            if (cifCode.InvestorId != null)
            {
                fullName = data?.InvestorIdentification?.Fullname;
            }
            else
            {
                fullName = data?.BusinessCustomer?.Name;
            }

            var orderContractFiles = _dbContext.RstOrderContractFiles.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
            string contractCode = orderContractFiles.Count() == 1 ? orderContractFiles.First() : order.ContractCode;

            //Số tiền đã thanh toán
            var paymentAmount = _rstOrderPaymentEFRepository.SumPaymentDepositAmount(order.Id);

            var content = new RstOrderPaymentSuccessContent()
            {
                CustomerName = fullName,
                ContractCode = contractCode,
                PaymentAmount = NumberToText.ConvertNumberIS((double)paymentFind?.PaymentAmount),
                TranDate = paymentFind?.TranDate?.ToString("dd/MM/yyyy HH:mm:ss"),
                TranNote = $"{PaymentNotes.THANH_TOAN}{contractCode}",
                TradingProviderName = businessCustomer?.Name,
                DepositMoney = NumberToText.ConvertNumberIS((double)order?.DepositMoney),
                PaymentMoney = NumberToText.ConvertNumberIS((double)paymentAmount),
            };
            var receiver = new Receiver
            {
                Email = new EmailNotifi
                {
                    Title = TitleEmail.REAL_ESTATE_ADMIN_KHACH_VAO_TIEN
                },
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.REAL_ESTATE_ADMIN_KHACH_VAO_TIEN, receiver,
                new ParamsChooseTemplate { TradingProviderId = paymentFind.TradingProviderId });
        }
        #endregion
    }
}
