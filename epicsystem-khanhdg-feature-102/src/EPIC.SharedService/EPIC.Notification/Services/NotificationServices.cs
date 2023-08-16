using AutoMapper;
using EPIC.BondEntities.DataEntities;
using EPIC.BondRepositories;
using EPIC.CoreEntities.Dto.CollabContract;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Notification.ResetPassword;
using EPIC.Entities.Dto.User;
using EPIC.IdentityRepositories;
using EPIC.InvestRepositories;
using EPIC.Notification.Dto.CoreNotification;
using EPIC.Notification.Dto.EmailBond;
using EPIC.Notification.Dto.IdentityNotification;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Notification;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.SharedApiService;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.FillReferralCode;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.InvestSuccess;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.ModifyBankAccount;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.ModifyTradingAddressDefault;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.NotifyInvestorProf;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.NotifyVerifyEmailTemplate;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.PaymentSuccess;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.RegisterAccount;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.ResetPin;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.Saler;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.SendOTP;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.VerifyEmailDto;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderStatus = EPIC.Utils.ConstantVariables.Shared.OrderStatus;

namespace EPIC.Notification.Services
{
    /// <summary>
    /// Thông báo
    /// </summary>
    public class NotificationServices
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

        #region bond
        private readonly BondOrderRepository _bondOrderRepository;
        private readonly BondOrderPaymentRepository _bondOrderPaymentRepository;
        private readonly BondSecondaryRepository _productBondSecondaryRepository;
        private readonly BondInfoRepository _productBondInfoRepository;
        private readonly BondSecondaryContractRepository _bondSecondaryContractRepository;
        private readonly BondInterestPaymentRepository _bondInterestPaymentRepository;
        #endregion

        #region invest
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
        private readonly SaleEFRepository _saleEFRepository;
        private readonly InvestInterestPaymentRepository _investInterestPaymentRepository;
        private readonly InvestInterestPaymentEFRepository _interestPaymentEFRepository;
        #endregion

        public NotificationServices(ILogger<NotificationServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IOptions<RecognitionApiConfiguration> recognitionApiConfiguration,
            SharedNotificationApiUtils sharedEmailApiUtils,
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
            _bondOrderRepository = new BondOrderRepository(_connectionString, _logger);
            _bondOrderPaymentRepository = new BondOrderPaymentRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContextAccessor);
            _productBondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            _sysVarRepository = new SysVarRepository(_connectionString, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _bondInterestPaymentRepository = new EPIC.BondRepositories.BondInterestPaymentRepository(_connectionString, _logger);
            _investInterestPaymentRepository = new InvestInterestPaymentRepository(_connectionString, _logger);
            _interestPaymentEFRepository = new InvestInterestPaymentEFRepository(_dbContext, _logger);
            _sharedEmailApiUtils = sharedEmailApiUtils;
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _productBondInfoRepository = new BondInfoRepository(_connectionString, _logger);
            _bondSecondaryContractRepository = new BondSecondaryContractRepository(_connectionString, _logger);
            _departmentRepository = new DepartmentRepository(_connectionString, _logger);
            _investOrderPaymentRepository = new InvestOrderPaymentRepository(_connectionString, _logger);
            _investOrderRepository = new InvestOrderRepository(_connectionString, _logger);
            _investPolicyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _distributionRepository = new DistributionRepository(_connectionString, _logger);
            _projectRepository = new ProjectRepository(_connectionString, _logger);
            _investOrderContractFileRepository = new OrderContractFileRepository(_connectionString, _logger);
            _withdrawalRepository = new WithdrawalRepository(_connectionString, _logger);
            _investContractTemplateEFRepository = new InvestContractTemplateEFRepository(_dbContext, _logger);
            _saleEFRepository = new SaleEFRepository(_dbContext, _logger);
        }

        #region Send Email/SMS/PUSH APP Tài khoản
        public EmailInvestorDataDto GetEmailInvestorData(int investorId, int? tradingProviderId = null)
        {
            var result = new EmailInvestorDataDto();
            result.OtherParams = new ParamsChooseTemplate()
            {
                TradingProviderId = tradingProviderId
            };
            result.Investor = _investorRepository.FindById(investorId);
            if (result.Investor == null)
            {
                _logger.LogError($"Không tìm thấy khách hàng khi gửi email/ sms/ push app. investorId: {investorId}");
                return result;
            }
            result.CifCode = _cifCodeRepository.GetByCustomerId(result.Investor.InvestorId);
            if (result.CifCode == null)
            {
                _logger.LogError($"Không tìm thấy cif code khi gửi email/ sms/ push app. investorId: {investorId}");
                return result;
            }

            //lấy thông tin giấy tờ mặc định
            var listIdenDb = _managerInvestorRepository.GetListIdentification(investorId, null, false);
            var idenDefault = listIdenDb?.FirstOrDefault(i => i.IsDefault == YesNo.YES);
            if (idenDefault == null)
            {
                idenDefault = listIdenDb?.FirstOrDefault();
            }
            result.InvestorIdentification = idenDefault;
            if (result.InvestorIdentification == null)
            {
                _logger.LogError($"Không tìm thấy thông tin giấy tờ mặc định của khách hàng khi gửi email/ sms/ push app. investorId: {investorId}");
            }

            result.InvestorContactAddress = _investorRepository.GetContactAddressDefault(investorId);
            if (result.InvestorContactAddress == null)
            {
                _logger.LogError($"Không tìm thấy địa chỉ giao dịch mặc định của khách hàng khi gửi email/ sms/ push app. investorId: {investorId}");
            }
            result.InvestorBankAccount = _managerInvestorRepository.GetDefaultBank(investorId);
            if (result.InvestorBankAccount == null)
            {
                _logger.LogError($"Không tìm thấy tài khoản thụ hưởng của khách hàng khi gửi email/ sms/ push app. investorId: {investorId}");
            }
            var user = _userRepository.FindByInvestorId(investorId);
            if (user != null)
            {
                result.Users = _mapper.Map<UserDto>(user);
            }
            else
            {
                _logger.LogError($"Không tìm thấy tài khoản đăng nhập của khách hàng khi gửi email/ sms/ push app. investorId: {investorId}");
            }
            result.Otp = _userRepository.GetOtpByUserId(result.Users.UserId);
            result.FcmTokens = _userRepository.GetFcmToken(result.Users.UserId);
            return result;
        }

        /// <summary>
        /// Send email thay đổi địa chỉ giao dịch thành công
        /// </summary>
        /// <param name="contactAddressId"></param>
        public async Task SendEmailSetContactAddressDefaultSuccess(int contactAddressId, int? tradingProviderId = null)
        {
            var contactAddress = _managerInvestorRepository.GetContactdAddressById(contactAddressId);
            if (contactAddress == null)
            {
                _logger.LogError($"Không tim thấy địa chỉ giao dịch khi gửi email thông báo thay đổi địa chỉ giao dịch mặc định. contactAddressId: {contactAddressId}");
                return;
            }
            var data = GetEmailInvestorData(contactAddress.InvestorId, tradingProviderId);

            var content = new ModifyTradingAddressContent()
            {
                CustomerName = data.InvestorIdentification?.Fullname ?? "",
                Address = data.InvestorContactAddress?.ContactAddress,
                DetailAddress = data.InvestorContactAddress?.DetailAddress,
                ProvinceName = data.InvestorContactAddress?.ProvinceName,
                DistrictName = data.InvestorContactAddress?.DistrictName,
                WardName = data.InvestorContactAddress?.WardName
            };
            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.MODIFY_TRADING_ADDRESS_DEFAULT_SUCCESS
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TK_THAYDOI_DIACHI_GIAO_DICH_MACDINH, receiver, data.OtherParams);
        }

        /// <summary>
        /// Send email thay đổi tài khoản thụ hưởng thành công
        /// </summary>
        /// <param name="investorBankId"></param>
        public async Task SendEmailSetBankDefaultSuccess(int investorBankId, int? tradingProviderId = null)
        {
            var bank = _managerInvestorRepository.GetBankById(investorBankId);
            if (bank == null)
            {
                _logger.LogError($"Không tìm thấy tài khoản thụ hưởng khi gửi email thông báo thay đổi tài khoản thụ hưởng. investorBankId: {investorBankId}");
                return;
            }
            var data = GetEmailInvestorData(bank.InvestorId, tradingProviderId);

            var content = new ModifyBankAccountContent()
            {
                CustomerName = data.InvestorIdentification?.Fullname ?? "",
                BankName = data.InvestorBankAccount.BankName,
                BankAccNo = data.InvestorBankAccount.BankAccount
            };
            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.MODIFY_BANK_ACCOUNT
                },
                UserId = data.Users.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TK_THAY_DOI_TK_THU_HUONG, receiver, data.OtherParams);
        }

        public async Task SendEmailOtpAnonymous(string otp, DateTime exp, string email)
        {
            var sysvar = _sysVarRepository.GetVarByName("EXPIRE", "OTP_EXP");

            var content = new SendOTPContent()
            {
                OTP = otp,
                OtpExpiredTime = $"{sysvar.VarValue} giây"
            };
            var receiver = new Receiver
            {
                Phone = null,
                Email = new EmailNotifi
                {
                    Address = email,
                    Title = TitleEmail.SEND_OTP_VERIFY
                },
                UserId = null,
                FcmTokens = null
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TK_THONG_BAO_OTP_EMAIL, receiver, new ParamsChooseTemplate());
        }

        public async Task SendSmsOtpAnonymous(string otp, DateTime exp, string phone)
        {
            var sysvar = _sysVarRepository.GetVarByName("EXPIRE", "OTP_EXP");

            var content = new SendOTPContent()
            {
                OTP = otp,
                OtpExpiredTime = $"{sysvar.VarValue} giây"
            };
            var receiver = new Receiver
            {
                Phone = phone,
                UserId = null,
                FcmTokens = null
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TK_THONGBAO_OTP, receiver, new ParamsChooseTemplate());
        }

        /// <summary>
        /// Send email OTP
        /// </summary>
        public async Task SendEmailOtp()
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            var data = GetEmailInvestorData(investorId);
            var sysvar = _sysVarRepository.GetVarByName("EXPIRE", "OTP_EXP");
            var content = new SendOTPContent()
            {
                OTP = data.Otp.OtpCode,
                OtpExpiredTime = $"{sysvar.VarValue} giây",
                CustomerName = data.InvestorIdentification?.Fullname ?? "",
            };
            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.SEND_OTP_SUCCESS
                },
                UserId = data.Users.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };

            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TK_THONGBAO_OTP, receiver, data.OtherParams);
        }

        /// <summary>
        /// Send email OTP giao nhận hợp đồng
        /// </summary>
        public async Task SendEmailOtpByPhone(string phone, int tradingProviderId)
        {
            var data = _userRepository.GetOtpByPhone(phone);
            var sysvar = _sysVarRepository.GetVarByName("EXPIRE", "OTP_EXP");
            var content = new SendOTPContent()
            {
                OTP = data?.OtpCode,
                OtpExpiredTime = $"{sysvar.VarValue} giây"
            };
            var receiver = new Receiver
            {
                Phone = phone,
                Email = new EmailNotifi
                {
                    Address = "",
                    Title = TitleEmail.SEND_OTP_SUCCESS
                },
                UserId = ""
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TK_THONGBAO_OTP, receiver, null);
        }

        /// <summary>
        /// Send email OTP
        /// </summary>
        public async Task SendEmailOtpEmail()
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            var data = GetEmailInvestorData(investorId);
            var sysvar = _sysVarRepository.GetVarByName("EXPIRE", "OTP_EXP");
            var content = new SendOTPContent()
            {
                OTP = data.Otp.OtpCode,
                OtpExpiredTime = $"{sysvar.VarValue} giây",
                CustomerName = data.InvestorIdentification?.Fullname ?? "",
            };
            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.NOTIFY_OTP_EMAIL_TEMPLATE
                },
                UserId = data.Users.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TK_THONG_BAO_OTP_EMAIL, receiver, data.OtherParams);
        }

        /// <summary>
        /// Send email đăng ký tài khoản, xác thực thành công, sau khi thêm bank
        /// </summary>
        /// <param name="phone"></param>
        public async Task SendEmailRegisterSuccess(string phone)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: phone - {phone}");

            var investor = _investorRepository.GetByEmailOrPhone(phone);
            if (investor == null)
            {
                _logger.LogError($"Không tim thấy khách hàng khi gửi email đăng ký tài khoản thành công. phone: {phone}");
                return;
            }
            var data = GetEmailInvestorData(investor.InvestorId);

            var content = new RegisterAccountContent()
            {
                CustomerName = data.InvestorIdentification?.Fullname ?? "",
                UserName = data.Users?.UserName,
                CreatedDate = data.Investor?.CreatedDate?.ToString("dd/MM/yyyy"),
                Phone = data.Investor?.Phone,
                Email = data.Investor?.Email,
                CifCode = data.CifCode.CifCode,
            };
            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.REGISTER_ACCOUNT_SUCCESS
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TK_DANGKY_TK_OK, receiver, data.OtherParams);
        }

        /// <summary>
        /// Thông báo xác thực tài khoản thành công
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public async Task SendEmailVerificationAccountSuccess(string phone)
        {
            _logger.LogInformation($"{nameof(SendEmailVerificationAccountSuccess)}: phone - {phone}");

            var investor = _dbContext.Investors.FirstOrDefault(i => i.Phone == phone && i.Deleted == YesNo.NO);
            if (investor == null)
            {
                _logger.LogError($"Không tìm thấy khách hàng khi gửi email thông báo xác thực tài khoản thành công. phone: {phone}");
                return;
            }
            if (investor.Step != InvestorAppStep.DA_ADD_BANK)
            {
                _logger.LogError($"Khách hàng chưa xác thực thành công khi gửi email thông báo xác thực tài khoản thành công. phone: {phone}");
                return;
            }
            var data = GetEmailInvestorData(investor.InvestorId);

            var content = new VerificationAccountSuccessContent()
            {
                CustomerName = data.InvestorIdentification?.Fullname ?? "",
                UserName = data.Users?.UserName,
                CreatedDate = investor.CreatedDate?.ToString("dd/MM/yyyy"),
                Phone = investor.Phone,
                Email = investor.Email,
                CifCode = data.CifCode.CifCode,
                FinalStepDate = investor.FinalStepDate?.ToString("dd/MM/yyyy"),
            };
            var receiver = new Receiver
            {
                Phone = investor.Phone,
                Email = new EmailNotifi
                {
                    Address = investor.Email,
                    Title = TitleEmail.VERIFY_ACCOUNT_SUCCESS
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TK_XAC_MINH_THANH_CONG, receiver, data.OtherParams);
        }

        /// <summary>
        /// Send email Reset Password thành công
        /// </summary>
        /// <param name="phone"></param>
        public async Task SendEmailResetPasswordSuccess(string phone, string password, int? investorId = null, int? tradingProviderId = null)
        {
            var investor = new Investor();
            if (investorId != null)
            {
                investor = _investorRepository.FindById(investorId ?? 0);
            }
            else
            {
                investor = _investorRepository.GetByEmailOrPhone(phone);
            }
            if (investor == null)
            {
                _logger.LogError($"Không tim thấy khách hàng khi gửi email reset password.");
                return;
            }
            var data = GetEmailInvestorData(investor.InvestorId, tradingProviderId);

            var content = new ResetPasswordContentDto
            {
                CustomerName = data.InvestorIdentification?.Fullname ?? "",
                Password = password
            };
            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.RESET_PASSWORD_SUCCESS
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TK_CAP_LAI_MK_THANH_CONG, receiver, data.OtherParams);
        }

        /// <summary>
        /// Send email Reset Pin thành công
        /// </summary>
        /// <param name="phone"></param>
        public async Task SendEmailResetPinSuccess(int userId, string pin, int? tradingProviderId = null)
        {
            var investor = _investorRepository.GetByUserId(userId);
            if (investor == null)
            {
                _logger.LogError($"Không tim thấy khách hàng khi gửi email reset pin. userId : {userId}");
                return;
            }
            var data = GetEmailInvestorData(investor.InvestorId, tradingProviderId);

            var content = new ResetPinContentDto
            {
                CustomerName = data.InvestorIdentification?.Fullname ?? "",
                Phone = data.Investor?.Phone,
                Email = data.Investor?.Email,
                PinCode = pin
            };
            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.RESET_PIN_SUCCESS
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TK_CAP_LAI_MA_PIN, receiver, data.OtherParams);
        }

        /// <summary>
        /// Gửi thông báo khi có người nhập mã giới thiệu
        /// </summary>
        /// <param name="investorId">người nhập mã</param>
        /// <returns></returns>
        public async Task SendNotifyEnterReferral(int investorId)
        {
            var investor = _investorRepository.FindById(investorId);
            if (investor == null)
            {
                _logger.LogError($"Không tìm thấy thông tin investor khi gửi email thông báo mã giới thiệu được nhập. investorId = {investorId}");
                return;
            }

            var identification = _investorRepository.GetIdentificationFirstWhenRegister(investorId);
            if (identification == null)
            {
                _logger.LogError($"Không tìm thấy thông tin giấy tờ khi gửi email thông báo mã giới thiệu được nhập. investorId = {investorId}");
                return;
            }

            if (investor?.ReferralCode != null)
            {
                var investorReferral = _managerInvestorRepository.GetInvestorByRefferalCode(investor?.ReferralCode);
                if (investorReferral == null)
                {
                    _logger.LogError($"Không tìm thấy investor bằng mã giới thiệu. referralCode = {investor?.ReferralCode}");
                    return;
                }

                var userReferral = _userRepository.FindByInvestorId(investorReferral.InvestorId);
                if (userReferral == null)
                {
                    _logger.LogError($"Không tìm thấy user giới thiệu với investorId = {investorId}");
                    return;
                }

                var content = new FillReferralCodeContent
                {
                    CustomerName = identification.Fullname
                };
                var receiver = new Receiver
                {
                    Phone = investorReferral?.Phone,
                    Email = new EmailNotifi
                    {
                        Address = investorReferral?.Email,
                        Title = TitleEmail.TK_NGUOI_NHAP_MA_GIOI_THIEU
                    },
                    UserId = userReferral?.UserId.ToString(),
                    FcmTokens = _userRepository.GetFcmToken(userReferral?.UserId ?? 0)
                };
                await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TK_NGUOI_NHAP_MA_GIOI_THIEU, receiver, new());
            }
        }

        /// <summary>
        /// Send email Khi có người nhập mã giới thiệu
        /// </summary>
        /// <param name="phone"></param>
        public async Task SendNotifyEnterReferralWhenRegister(string phone)
        {
            var investorRegister = _investorRepository.GetByEmailOrPhone(phone);
            if (investorRegister == null)
            {
                _logger.LogError($"Không tìm thấy khách hàng khi gửi email thông báo mã giới thiệu được nhập. phoneOrEmail: {phone}");
                return;
            }
            await SendNotifyEnterReferral(investorRegister.InvestorId);
        }

        /// <summary>
        /// Send email Verify
        /// </summary>
        public async Task SendEmailVerify()
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            _logger.LogError($"SendEmailVerify => Gửi thông báo xác nhận email: investorId = {investorId}");

            var configLinkVerifyEmail = _configuration.GetSection("LinkVerifyEmail").Value;
            string verifyEmailCode = _investorRepository.GenVerifyEmailCode(investorId);
            string linkVerify = $"{configLinkVerifyEmail}/{verifyEmailCode}";
            var data = GetEmailInvestorData(investorId);
            var sysvar = _sysVarRepository.GetVarByName("AUTH", "VERIFY_EMAIL_EXPIRED");
            var content = new NotifyVerifyEmailTemplateContent
            {
                CustomerName = data.InvestorIdentification.Fullname,
                LinkVerify = linkVerify,
                EmailExpiredTime = $"{sysvar.VarValue} ngày"
            };
            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.NOTIFY_VERIFY_EMAIL_TEMPLATE
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TK_XAC_MINH_EMAIL, receiver, data.OtherParams);
        }

        /// <summary>
        /// Send email Verify
        /// </summary>
        /// <param name="verifyEmailCode"></param>
        public async Task SendEmailVerifyEmailSuccess(string verifyEmailCode)
        {
            var investor = _investorRepository.GetByVeryfiEmailCode(verifyEmailCode);
            if (investor == null)
            {
                _logger.LogError($"Không tim thấy khách hàng khi gửi email xác thực email. veryifyEmailCode: {verifyEmailCode}");
                return;
            }
            var data = GetEmailInvestorData(investor.InvestorId);

            var content = new VerifyEmailContent
            {
                CustomerName = data.InvestorIdentification?.Fullname ?? "",
                Email = data.Investor?.Email
            };
            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.VERIFY_EMAIL_SUCCESS
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TK_EMAIL_XACMINH_OK, receiver, data.OtherParams);

        }

        public async Task SendEmailInvestorProf(int investorId, int? tradingProviderId = null)
        {
            var data = GetEmailInvestorData(investorId, tradingProviderId);
            var content = new NotifyInvestorProfDto
            {
                CustomerName = data.InvestorIdentification?.Fullname ?? "",
                Phone = data.Investor?.Phone,
                Email = data.Investor?.Email,
                ProfStartDate = data.Investor?.ProfStartDate?.ToString("dd/MM/yyyy"),
                ProfDueDate = data.Investor?.ProfDueDate?.ToString("dd/MM/yyyy")
            };
            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.NOTIFY_INVESTOR_PROF
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TK_CHUNG_MINH_NHA_DAU_TU_CHUYEN_NGHIEP, receiver, data.OtherParams);
        }

        /// <summary>
        /// Gửi thông báo sinh nhật của khách hàng đến tư vấn viên
        /// </summary>
        public async Task SendEmailNotiCustomerHappyBirthDay(int investorId, int tradingProviderId)
        {
            var identificationInvestor = _dbContext.InvestorIdentifications.AsNoTracking().Where(r => r.InvestorId == investorId && r.Deleted == YesNo.NO)
                                            .OrderByDescending(ii => ii.IsDefault).ThenByDescending(ii => ii.Id).FirstOrDefault();
            if (identificationInvestor == null)
            {
                _logger.LogError($"Không tìm thấy thông tin giấy tờ của investor khi gửi email/ sms/ push app thông báo sinh nhật của nhà đầu tư. investorId: {investorId}");
                return;
            }

            var tradingProviderInfo = (from tradingProvider in _dbContext.TradingProviders.AsNoTracking()
                                       join businessCustomer in _dbContext.BusinessCustomers.AsNoTracking() on tradingProvider.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                                       where tradingProvider.Deleted == YesNo.NO && businessCustomer.Deleted == YesNo.NO
                                       select businessCustomer).FirstOrDefault();
            if (tradingProviderInfo == null)
            {
                _logger.LogError($"Không tìm thấy thông tin đại lý khi gửi email/ sms/ push app thông báo sinh nhật của nhà đầu tư. investorId: {investorId}");
                return;
            }
            var data = GetEmailInvestorData(investorId, tradingProviderId);

            var content = new CoreBirthdayCustomerForSaleContentDto()
            {
                TradingProviderName = tradingProviderInfo.Name,
                CustomerName = identificationInvestor.Fullname,
                DateOfBirth = identificationInvestor.DateOfBirth.Value.ToString("dd/MM/yyyy"),
            };

            var receiver = new Receiver
            {
                Phone = data?.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data?.Investor?.Email,
                    Title = TitleEmail.TB_CHUC_MUNG_SINH_NHAT_KHACH_HANG
                },
                UserId = data?.Users?.UserId.ToString(),
                FcmTokens = data?.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TB_CHUC_MUNG_SINH_NHAT_KHACH_HANG, receiver, data.OtherParams);
        }
        #endregion

        #region Send Email/SMS/Push APP Đầu tư bond
        public EmailDataBond GetDataBond(CifCodes cifCode, BondOrder order)
        {
            var result = new EmailDataBond();
            if (cifCode.InvestorId != null)
            {
                result.Investor = _investorRepository.FindById(cifCode.InvestorId ?? 0);
                if (result.Investor == null)
                {
                    _logger.LogError($"Không tìm thấy khách hàng khi gửi email/ sms/ push app. investorId: {cifCode.InvestorId}");
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
        public async Task SendEmailBondApprovePayment(int orderPaymentId)
        {
            var orderPayment = _bondOrderPaymentRepository.FindById(orderPaymentId);
            if (orderPayment == null)
            {
                _logger.LogError($"Không tìm thấy thông tin thanh toán khi thông báo chuyển tiền thành công. orderPaymentId: {orderPaymentId}");
                return;
            }
            var order = _bondOrderRepository.FindById(orderPayment.OrderId ?? 0, null);
            if (order == null)
            {
                _logger.LogError($"Không tìm thấy sổ lệnh khi thông báo chuyển tiền thành công. orderId: {orderPayment.OrderId}");
                return;
            }

            var bondInfo = _productBondInfoRepository.FindById(order.BondId);
            if (bondInfo == null)
            {
                _logger.LogError($"Không tìm thấy thông tin gói trái phiếu. productBondId: {order.BondId}, orderId = {orderPayment.OrderId}");
                return;
            }

            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"Không tìm thấy mã khách hàng khi thông báo chuyển tiền thành công. cifCode: {order.CifCode}");
                return;
            }
            var productPolicyDetail = _productBondSecondaryRepository.FindPolicyDetailById(order.PolicyDetailId, order.TradingProviderId);
            if (productPolicyDetail == null)
            {
                _logger.LogError($"Không tim thấy kỳ hạn khi gửi email thông báo chuyển tiền thành công. policyDetailId = {order.PolicyDetailId}, tradingProviderId = {order.TradingProviderId}, orderPaymentId = {orderPaymentId}");
                return;
            }

            var tradingProvider = _tradingProviderRepository.FindById(order.TradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"Không tim thấy đại lý khi gửi email thông báo chuyển tiền thành công. tradingProviderId = {order.TradingProviderId}, orderPaymentId = {orderPaymentId}");
                return;
            }

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"Không tim thấy business customer là đại lý khi gửi email thông báo chuyển tiền thành công. tradingProviderId = {order.TradingProviderId}, businessCustomerId = {tradingProvider.BusinessCustomerId}");
                return;
            }

            var data = GetDataBond(cifCode, order);
            string fullName = null;
            if (cifCode.InvestorId != null)
            {
                fullName = data.InvestorIdentification.Fullname;
            }
            else
            {
                fullName = data.BusinessCustomer.Name;
            }

            //thông báo chuyển tiền thành công
            var contentPayment = new BondPaymentSuccessContent()
            {
                BondCode = bondInfo?.BondCode,
                BondName = bondInfo?.BondName,
                CustomerName = fullName,
                PaymentAmount = NumberToText.ConvertNumberIS((double?)orderPayment.PaymentAmnount),
                ContractCode = order.ContractCode,
                TradingProviderName = businessCustomer.Name,
                TranDate = orderPayment.TranDate?.ToString("dd/MM/yyyy HH:mm:ss"),
                TranNote = $"{PaymentNotes.THANH_TOAN}{order.ContractCode}"
            };

            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.CHUYEN_TIEN_THANH_CONG_BOND
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(contentPayment, KeyTemplate.DAU_TU_CHUYEN_TIEN_THANH_CONG_BOND, receiver, data.OtherParams);

            if (order.Status == OrderStatus.DANG_DAU_TU)
            {
                var contentOrderActive = new BondOrderSuccessContent()
                {
                    BondCode = bondInfo?.BondCode,
                    BondName = bondInfo?.BondName,
                    CustomerName = fullName,
                    TotalValue = order.TotalValue.ToString("N0").Replace(",", "."),
                    ContractCode = order.ContractCode,
                    Tenor = $"{productPolicyDetail.PeriodQuantity} {ContractDataUtils.GetNameDateType(productPolicyDetail.PeriodType)}",
                    PaymentFullDate = order.PaymentFullDate?.ToString("dd/MM/yyyy")
                };
                await _sharedEmailApiUtils.SendEmailAsync(contentOrderActive, KeyTemplate.DAU_TU_THANH_CONG_BOND, receiver, data.OtherParams);
            }
        }

        /// <summary>
        /// Send email thông báo đầu tư thành công
        /// </summary>
        /// <param name="orderId"></param>
        public async Task SendEmailBondOrderActive(int orderId)
        {
            var order = _bondOrderRepository.FindById(orderId, null);
            if (order == null)
            {
                _logger.LogError($"Không tìm thấy sổ lệnh khi gửi email thông báo chuyển tiền thành công. orderId: {orderId}");
                return;
            }
            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"Không tìm thấy mã khách hàng khi gửi email thông báo chuyển tiền thành công. cifCode: {order.CifCode}");
                return;
            }

            var bondInfo = _productBondInfoRepository.FindById(order.BondId);
            if (bondInfo == null)
            {
                _logger.LogError($"Không tìm thấy thông tin gói trái phiếu. productBondId: {order.BondId}, orderId = {orderId}");
                return;
            }

            var bondPolicy = _productBondSecondaryRepository.FindPolicyById(order.PolicyId);
            if (bondPolicy == null)
            {
                _logger.LogError($"Không tìm thấy thông tin chính sách. BondPolicyId: {order.PolicyId}, orderId = {orderId}");
                return;
            }

            var bondPolicyDetail = _productBondSecondaryRepository.FindPolicyDetailById(order.PolicyDetailId);
            if (bondPolicyDetail == null)
            {
                _logger.LogError($"Không tìm thấy thông tin kỳ hạn. BondPolicyDetailId: {order.PolicyDetailId}, orderId = {orderId}");
                return;
            }

            var contracts = _bondSecondaryContractRepository.FindAll(orderId, order.TradingProviderId);
            if (order.Source == SourceOrder.OFFLINE)
            {
                contracts = new();
            }

            var data = GetDataBond(cifCode, order);
            string fullName = null;
            if (cifCode.InvestorId != null)
            {
                fullName = data.InvestorIdentification.Fullname;
            }
            else
            {
                fullName = data.BusinessCustomer.Name;
            }
            if (order.Status == OrderStatus.DANG_DAU_TU)
            {
                var content = new BondOrderSuccessContent()
                {
                    BondCode = bondInfo?.BondCode,
                    BondName = bondInfo?.BondName,
                    CustomerName = fullName,
                    TotalValue = NumberToText.ConvertNumberIS((double?)order.TotalValue),
                    ContractCode = order.ContractCode,
                    Tenor = $"{bondPolicyDetail?.PeriodQuantity} {ContractDataUtils.GetNameDateType(bondPolicyDetail?.PeriodType)}",
                    PaymentFullDate = order.PaymentFullDate?.ToString("dd/MM/yyyy"),
                    //StartDate = 
                    PolicyName = bondPolicy?.Name,
                    Profit = NumberToText.ConvertNumberIS((double?)bondPolicyDetail?.Profit ?? 0) + "%"
                };
                var receiver = new Receiver
                {
                    Phone = data.Investor?.Phone,
                    Email = new EmailNotifi
                    {
                        Address = data.Investor?.Email,
                        Title = TitleEmail.DAU_TU_THANH_CONG_BOND
                    },
                    UserId = data.Users?.UserId.ToString(),
                    FcmTokens = data.FcmTokens
                };
                var attachments = contracts.Select(c => (new Uri(new Uri(_baseUrl), c.FileSignatureUrl)).AbsoluteUri).ToList();
                await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.DAU_TU_THANH_CONG_BOND, receiver, data.OtherParams, attachments);
            }
        }
        #endregion

        #region Send Email/SMS/Push App Saler
        /// <summary>
        /// Yêu cầu duyệt
        /// </summary>
        /// <param name="investorRegisterId"></param>
        /// <param name="saleManagerId"></param>
        /// <returns></returns>
        public async Task SendEmailSaleRegisterForSaleManager(int investorRegisterId, int saleManagerId)
        {
            //lấy thông tin giấy tờ mặc định
            var listIdenDb = _managerInvestorRepository.GetListIdentification(investorRegisterId, null, false);
            var investorRegistorIdenDefault = listIdenDb?.FirstOrDefault(i => i.IsDefault == YesNo.YES);
            if (investorRegistorIdenDefault == null)
            {
                investorRegistorIdenDefault = listIdenDb?.FirstOrDefault();
            }

            if (investorRegistorIdenDefault == null)
            {
                _logger.LogError($"Không tìm thấy thông tin giấy tờ mặc định của investor khi gửi email/ sms/ push app đăng ký sale. investorId: {investorRegisterId}");
                return;
            }

            var investorRegister = _investorRepository.FindById(investorRegisterId);
            if (investorRegister == null)
            {
                _logger.LogError($"Không tìm thấy thông tin investor khi gửi email/ sms/ push app đăng ký sale. investorRegisterId: {investorRegisterId}");
                return;
            }

            var saleManager = _saleRepository.FindById(saleManagerId);
            if (saleManager == null)
            {
                _logger.LogError($"Không tìm thấy thông tin sale manager khi gửi email/ sms/ push app đăng ký sale. saleManagerId: {saleManagerId}");
                return;
            }

            var investorManager = _investorRepository.FindById(saleManager.InvestorId ?? 0);
            if (investorManager == null)
            {
                _logger.LogError($"Không tìm thấy thông tin investor của sale quản lý khi gửi email/ sms/ push app đăng ký sale. InvestorId: {saleManager.InvestorId}");
                return;
            }

            var userManager = _userRepository.FindByInvestorId(saleManager.InvestorId ?? 0);
            if (userManager == null)
            {
                _logger.LogError($"Không tìm thấy thông tin user của sale manager khi gửi email/ sms/ push app đăng ký sale. InvestorId: {saleManager.InvestorId}");
                return;
            }
            var fcmTokenUserManager = _userRepository.GetFcmToken(userManager.UserId);

            var content = new NotificationSaleRegisterForSaleManagerContent()
            {
                SaleRegisterName = investorRegistorIdenDefault?.Fullname,
                SaleRegisterPhone = investorRegister?.Phone,
                SaleRegisterEmail = investorRegister?.Email,
                SaleRegisterReferralCode = investorRegister?.ReferralCodeSelf,
            };
            var receiver = new Receiver
            {
                Phone = investorRegister?.Phone,
                Email = new EmailNotifi
                {
                    Address = investorRegister?.Email,
                    Title = TitleEmail.TB_YEU_CAU_DUYET_TU_VAN_VIEN
                },
                UserId = userManager.UserId.ToString(),
                FcmTokens = fcmTokenUserManager
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TB_YEU_CAU_DUYET_TU_VAN_VIEN, receiver, new ParamsChooseTemplate());
        }

        /// <summary>
        /// Ký thành công
        /// </summary>
        /// <param name="saleId"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="listContract"></param>
        /// <returns></returns>
        public async Task SendEmailSaleRegisterSuccess(int saleId, int tradingProviderId, List<CollabContractDto> listContract)
        {
            var saleRegister = _saleRepository.FindSaleById(saleId, tradingProviderId);
            if (saleRegister == null)
            {
                _logger.LogError($"Không tìm thấy thông tin sale khi gửi email/ sms/ push app thông báo được duyệt thành sale. saleManagerId: {saleId}");
                return;
            }

            var investorRegister = _investorRepository.FindById(saleRegister.InvestorId ?? 0);
            if (investorRegister == null)
            {
                _logger.LogError($"Không tìm thấy thông tin investor khi gửi email/ sms/ push app thông báo được duyệt thành sale. investorId: {saleRegister.InvestorId}");
                return;
            }

            var bankAcc = _managerInvestorRepository.GetBankById(saleRegister.InvestorBankAccId ?? 0);
            if (bankAcc == null)
            {
                _logger.LogError($"Không tìm thấy thông tin bank acc investor khi gửi email/ sms/ push app thông báo được duyệt thành sale. InvestorBankAccId: {saleRegister.InvestorBankAccId}");
                return;
            }

            //lấy thông tin giấy tờ mặc định
            var listIdenDb = _managerInvestorRepository.GetListIdentification(saleRegister.InvestorId ?? 0, null, false);
            var investorRegistorIdenDefault = listIdenDb?.FirstOrDefault(i => i.IsDefault == YesNo.YES);
            if (investorRegistorIdenDefault == null)
            {
                investorRegistorIdenDefault = listIdenDb?.FirstOrDefault();
            }

            if (investorRegistorIdenDefault == null)
            {
                _logger.LogError($"Không tìm thấy thông tin giấy tờ mặc định của investor khi gửi email/ sms/ push app thông báo được duyệt thành sale. investorId: {saleRegister.InvestorId}");
                return;
            }

            var userSaleRegister = _userRepository.FindByInvestorId(investorRegister.InvestorId);
            if (userSaleRegister == null)
            {
                _logger.LogError($"Không tìm thấy thông tin user của sale khi gửi email/ sms/ push app thông báo được duyệt thành sale. InvestorId: {investorRegister.InvestorId}");
                return;
            }
            var fcmTokenUserManager = _userRepository.GetFcmToken(userSaleRegister.UserId);

            var tradingProvider = _tradingProviderRepository.FindById(tradingProviderId);
            if (tradingProvider == null)
            {
                _logger.LogError($"Không tìm thấy thông tin đại lý khi gửi email/ sms/ push app thông báo được duyệt thành sale. tradingProviderId: {tradingProviderId}");
                return;
            }

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomer == null)
            {
                _logger.LogError($"Không tìm thấy thông tin doanh nghiệp là đại lý khi gửi email/ sms/ push app thông báo được duyệt thành sale. businessCustomerId: {tradingProvider.BusinessCustomerId}");
                return;
            }

            var department = _departmentRepository.FindBySaleId(saleId, tradingProviderId);
            if (department == null)
            {
                _logger.LogError($"Không tìm thấy thông tin phòng ban khi gửi email/ sms/ push app thông báo được duyệt thành sale. saleId: {saleId}, tradingProviderId: {tradingProviderId}");
                return;
            }

            var content = new NotificationSaleRegisterSuccessContent()
            {
                SaleRegisterName = investorRegistorIdenDefault?.Fullname,
                SaleRegisterPhone = investorRegister?.Phone,
                SaleRegisterEmail = investorRegister?.Email,
                SaleRegisterReferralCode = investorRegister?.ReferralCodeSelf,
                TradingProviderName = businessCustomer?.Name,
                DepartmentName = department?.DepartmentName,
                SaleBankAccNo = bankAcc?.BankAccount,
                BankName = bankAcc?.CoreFullBankName,
                ConfirmDate = saleRegister?.SaleTradingCreatedDate?.ToString("dd/MM/yyyy")
            };
            var receiver = new Receiver
            {
                Phone = investorRegister?.Phone,
                Email = new EmailNotifi
                {
                    Address = investorRegister?.Email,
                    Title = TitleEmail.TB_DANG_KY_TU_VAN_VIEN_OK
                },
                UserId = userSaleRegister.UserId.ToString(),
                FcmTokens = fcmTokenUserManager
            };
            var attachments = listContract.Select(c => (new Uri(new Uri(_baseUrl), c.FileSignatureUrl)).AbsoluteUri).ToList();
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TB_DANG_KY_TU_VAN_VIEN_OK, receiver, new ParamsChooseTemplate()
            {
                TradingProviderId = tradingProviderId
            }, attachments);
        }

        /// <summary>
        /// Điều hướng thành công
        /// </summary>
        /// <param name="saleTempId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public async Task SendEmailSaleDirectionSuccess(int saleTempId, int tradingProviderId)
        {
            var saleTemp = _saleRepository.FindSaleTemp(saleTempId, tradingProviderId);
            if (saleTemp == null)
            {
                _logger.LogError($"Không tìm thấy thông tin sale khi gửi email/ sms/ push app thông báo được điều hướng vào đại lý. saleTempId: {saleTempId}");
                return;
            }

            var investorRegister = _investorRepository.FindById(saleTemp.InvestorId ?? 0);
            if (investorRegister == null)
            {
                _logger.LogError($"Không tìm thấy thông tin investor khi gửi email/ sms/ push app thông báo được điều hướng vào đại lý. investorId: {saleTemp.InvestorId}");
                return;
            }

            var bankAcc = _managerInvestorRepository.GetBankById(saleTemp.InvestorBankAccId ?? 0);
            if (bankAcc == null)
            {
                _logger.LogError($"Không tìm thấy thông tin bank acc investor khi gửi email/ sms/ push app thông báo được điều hướng vào đại lý. InvestorBankAccId: {saleTemp.InvestorBankAccId}");
                return;
            }

            //lấy thông tin giấy tờ mặc định
            var listIdenDb = _managerInvestorRepository.GetListIdentification(saleTemp.InvestorId ?? 0, null, false);
            var investorRegistorIdenDefault = listIdenDb?.FirstOrDefault(i => i.IsDefault == YesNo.YES);
            if (investorRegistorIdenDefault == null)
            {
                investorRegistorIdenDefault = listIdenDb?.FirstOrDefault();
            }

            if (investorRegistorIdenDefault == null)
            {
                _logger.LogError($"Không tìm thấy thông tin giấy tờ mặc định của investor khi gửi email/ sms/ push app thông báo được điều hướng vào đại lý. investorId: {saleTemp.InvestorId}");
                return;
            }

            var userSaleRegister = _userRepository.FindByInvestorId(investorRegister.InvestorId);
            if (userSaleRegister == null)
            {
                _logger.LogError($"Không tìm thấy thông tin user của sale khi gửi email/ sms/ push app thông báo được điều hướng vào đại lý. InvestorId: {investorRegister.InvestorId}");
                return;
            }
            var fcmTokenUserManager = _userRepository.GetFcmToken(userSaleRegister.UserId);

            var content = new NotificationSaleDirectionSuccessContent()
            {
                SaleRegisterName = investorRegistorIdenDefault?.Fullname,
                SaleRegisterPhone = investorRegister?.Phone,
                SaleRegisterEmail = investorRegister?.Email,
                SaleRegisterReferralCode = investorRegister?.ReferralCodeSelf,
            };

            var receiver = new Receiver
            {
                Phone = investorRegister?.Phone,
                Email = new EmailNotifi
                {
                    Address = investorRegister?.Email,
                    Title = TitleEmail.TB_DUYET_YEU_CAU_TU_VAN_VIEN_OK
                },
                UserId = userSaleRegister?.UserId.ToString(),
                FcmTokens = fcmTokenUserManager
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TB_DIEU_HUONG_KENH_BAN, receiver, new ParamsChooseTemplate()
            {
                TradingProviderId = tradingProviderId
            });
        }

        /// <summary>
        /// Gửi thông báo sinh nhật của khách hàng đến tư vấn viên
        /// </summary>
        public async Task SendEmailSaleNotiCustomerBirthDay(int saleId, int investorId, int tradingProviderId)
        {
            var sale = _saleEFRepository.EntityNoTracking.FirstOrDefault(r => r.SaleId == saleId && r.Deleted == YesNo.NO);
            if (sale == null || sale.InvestorId == null)
            {
                _logger.LogError($"Không tìm thấy thông tin sale khi gửi email/ sms/ push app thông báo được điều hướng vào đại lý. saleTempId: {saleId}");
                return;
            }

            var saleName = _dbContext.InvestorIdentifications.Where(r => r.InvestorId == sale.InvestorId && r.Deleted == YesNo.NO)
                            .OrderByDescending(ii => ii.IsDefault).ThenByDescending(ii => ii.Id).Select(ii => ii.Fullname).FirstOrDefault();

            var identificationInvestor = _dbContext.InvestorIdentifications.Where(r => r.InvestorId == investorId && r.Deleted == YesNo.NO)
                                            .OrderByDescending(ii => ii.IsDefault).ThenByDescending(ii => ii.Id).FirstOrDefault();
            if (identificationInvestor == null)
            {
                _logger.LogError($"Không tìm thấy thông tin giấy tờ của investor khi gửi email/ sms/ push app thông báo sinh nhật của nhà đầu tư. investorId: {investorId}");
                return;
            }

            var dataSale = GetEmailInvestorData(sale.InvestorId ?? 0, tradingProviderId);

            var content = new CoreBirthdayCustomerForSaleContentDto()
            {
                CustomerName = identificationInvestor.Fullname,
                DateOfBirth = identificationInvestor.DateOfBirth.Value.ToString("dd/MM/yyyy"),
                SaleName = saleName,
            };

            var receiver = new Receiver
            {

                Phone = dataSale?.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = dataSale?.Investor?.Email,
                    Title = TitleEmail.TB_SINH_NHAT_KHACH_HANG_DEN_SALE
                },
                UserId = dataSale?.Users?.UserId.ToString(),
                FcmTokens = dataSale?.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TB_SINH_NHAT_KHACH_HANG_DEN_SALE, receiver, dataSale.OtherParams);
        }
        #endregion

        #region Tái tục, rút vốn
        /// <summary>
        /// Tái tục vốn Bond thành công
        /// </summary>
        /// <param name="interestPaymentId"></param>
        /// <param name="newOrderId">Id hợp đồng mới</param>
        /// <returns></returns>
        public async Task SendEmailBondRenewalsSuccess(int interestPaymentId, long? newOrderId)
        {
            var interestPayment = _bondInterestPaymentRepository.FindById(interestPaymentId);
            if (interestPayment == null)
            {
                _logger.LogError($"Không tìm thấy thông tin chi trả {interestPaymentId}");
                return;
            }

            var order = _bondOrderRepository.FindById(newOrderId ?? 0, null);
            if (order == null)
            {
                _logger.LogError($"Không tìm thấy sổ lệnh khi gửi email thông báo tái tục vốn thàng công. orderId: {newOrderId}");
                return;
            }
            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                _logger.LogError($"Không tìm thấy mã khách hàng khi gửi email thông báo tái tục vốn thàng công. cifCode: {order.CifCode}");
                return;
            }

            var bondInfo = _productBondInfoRepository.FindById(order.BondId);
            if (bondInfo == null)
            {
                _logger.LogError($"Không tìm thấy thông tin gói trái phiếu. productBondId: {order.BondId}, orderId = {newOrderId}");
                return;
            }

            var bondPolicy = _productBondSecondaryRepository.FindPolicyById(order.PolicyId);
            if (bondPolicy == null)
            {
                _logger.LogError($"Không tìm thấy thông tin chính sách. BondPolicyId: {order.PolicyId}, orderId = {newOrderId}");
                return;
            }

            var bondPolicyDetail = _productBondSecondaryRepository.FindPolicyDetailById(order.PolicyDetailId);
            if (bondPolicyDetail == null)
            {
                _logger.LogError($"Không tìm thấy thông tin kỳ hạn. BondPolicyDetailId: {order.PolicyDetailId}, orderId = {newOrderId}");
                return;
            }

            var data = GetDataBond(cifCode, order);
            string fullName = null;
            if (cifCode.InvestorId != null)
            {
                fullName = data.InvestorIdentification.Fullname;
            }
            else
            {
                fullName = data.BusinessCustomer.Name;
            }
            if (order.Status == OrderStatus.DANG_DAU_TU)
            {
                var content = new BondOrderRenewalsSuccessContent()
                {
                    BondCode = bondInfo?.BondCode,
                    BondName = bondInfo?.BondName,
                    CustomerName = fullName,
                    TotalValue = NumberToText.ConvertNumberIS((double?)order?.TotalValue),
                    ContractCode = order?.ContractCode,
                    Tenor = $"{bondPolicyDetail?.PeriodQuantity} {ContractDataUtils.GetNameDateType(bondPolicyDetail?.PeriodType)}",
                    PaymentFullDate = order?.PaymentFullDate?.ToString("dd/MM/yyyy"),
                    //StartDate = 
                    PolicyName = bondPolicy?.Name,
                    Profit = NumberToText.ConvertNumberIS((double?)bondPolicyDetail?.Profit ?? 0) + "%"
                };
                var receiver = new Receiver
                {
                    Phone = data.Investor?.Phone,
                    Email = new EmailNotifi
                    {
                        Address = data.Investor?.Email,
                        Title = TitleEmail.TAI_TUC_THANH_CONG_BOND
                    },
                    UserId = data.Users?.UserId.ToString(),
                    FcmTokens = data.FcmTokens
                };
                await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.TAI_TUC_THANH_CONG_BOND, receiver, data.OtherParams);
            }
        }
        #endregion
    }
}
