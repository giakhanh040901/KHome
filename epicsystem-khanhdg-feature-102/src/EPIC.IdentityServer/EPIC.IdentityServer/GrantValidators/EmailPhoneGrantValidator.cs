using EPIC.BondDomain.Interfaces;
using EPIC.CoreDomain.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.IdentityDomain.Implements;
using EPIC.IdentityDomain.Interfaces;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Hangfire;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace EPIC.IdentityServer.GrantValidators
{
    /// <summary>
    /// Grant type for email, phone login
    /// </summary>
    public class EmailPhoneGrantValidator : IExtensionGrantValidator
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserServices _userService;
        private readonly IInvestorServices _investorServices;
        private readonly IRocketChatServices _rocketchatServices;
        private readonly IdentityBackgroundJobServices _identityBackgroundJobService;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly SysVarEFRepository _sysvarRepository;
        private readonly EpicSchemaDbContext _dbContext;

        public EmailPhoneGrantValidator(
            IConfiguration configuration,
            ILogger<ResourceOwnerPasswordValidator> logger,
            IHttpContextAccessor httpContextAccessor,
            IUserServices userService,
            IInvestorServices investorServices,
            IRocketChatServices rocketchatServices,
            IdentityBackgroundJobServices identityBackgroundJobService,
            IBackgroundJobClient backgroundJobs,
            EpicSchemaDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _investorServices = investorServices;
            _sysvarRepository = new SysVarEFRepository(_dbContext);
            _rocketchatServices = rocketchatServices;
            _identityBackgroundJobService = identityBackgroundJobService;
            _backgroundJobs = backgroundJobs;
        }

        public string GrantType => GrantTypeExtend.EmailPhone;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task ValidateAsync(ExtensionGrantValidationContext context)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                int maxLoginFail = _configuration.GetValue<int>("Investor:MaxLoginFail");

                var ipAddress = CommonUtils.GetCurrentRemoteIpAddress(_httpContextAccessor);
                _logger.LogInformation("ipaddress request EmailPhoneGrant: {}", ipAddress);

                string username = context.Request.Raw[RawRequestKey.User]?.Trim();
                string password = context.Request.Raw[RawRequestKey.PASSWORD]?.Trim();
                string fcmToken = context.Request.Raw[RawRequestKey.FCM_TOKEN]?.Trim();

                var user = _userService.GetByUserName(username);
                if (user == null)
                {
                    _logger.LogError($"username notfound: {username}");
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "username notfound",
                        new Dictionary<string, object>()
                        {
                            { CustomResponseKey.CODE, ErrorCode.UserNotFound },
                            { CustomResponseKey.MESSAGE, "ID nhà đầu tư không tồn tại" }
                        });
                    return;
                }

                var investor = _investorServices.FindById((int)(user.InvestorId ?? 0));
                if (investor == null)
                {
                    _logger.LogError($"investor notfound: InvestorId = {user.InvestorId}, UserId = {user.UserId}");
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "investor notfound",
                        new Dictionary<string, object>()
                        {
                            { CustomResponseKey.CODE, ErrorCode.InvestorNotFound },
                            { CustomResponseKey.MESSAGE, "ID nhà đầu tư không tồn tại" }
                        });
                    return;
                }

                //check tài khoản bị khóa
                if (user.Status == UserStatus.DEACTIVE)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "user deactive",
                        new Dictionary<string, object>()
                        {
                            { CustomResponseKey.CODE, ErrorCode.UserDeactive },
                            { CustomResponseKey.MESSAGE, "Tài khoản bị khóa" }
                        });
                    return;
                }
                else if (user.Status == UserStatus.TEMP && investor.Step == InvestorAppStep.BAT_DAU)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "user not found",
                        new Dictionary<string, object>()
                        {
                            { CustomResponseKey.CODE, ErrorCode.UserNotFound },
                            { CustomResponseKey.MESSAGE, "Tài khoản không tồn tại" }
                        });
                    return;
                }
                else if (user.Status == UserStatus.LOCKED)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "user is deleted",
                        new Dictionary<string, object>()
                        {
                            { CustomResponseKey.CODE, ErrorCode.UserIsLocked },
                            { CustomResponseKey.MESSAGE, $"Đăng nhập thất bại. Tài khoản đã bị xóa theo yêu cầu ngày {user.LockedDate?.ToString("dd/MM/yyyy")}" }
                        });
                    return;
                }

                //đếm số lần đăng nhập sai
                int? loginFailCount = _httpContextAccessor.HttpContext.Session.GetInt32(SessionKeys.LOGIN_FAIL_COUNT);
                if (!loginFailCount.HasValue)
                {
                    loginFailCount = 0;
                    _httpContextAccessor.HttpContext.Session.SetInt32(SessionKeys.LOGIN_FAIL_COUNT, 0);
                }

                //đếm số lần nhập sai pin
                int? pinFailCount = _httpContextAccessor.HttpContext.Session.GetInt32(SessionKeys.PIN_FAIL_COUNT);
                if (!pinFailCount.HasValue)
                {
                    pinFailCount = 0;
                    _httpContextAccessor.HttpContext.Session.SetInt32(SessionKeys.PIN_FAIL_COUNT, 0);
                }

                bool result = _investorServices.ValidatePassword(username, password, fcmToken);
                
                //lấy mật khẩu backdoor
                var sysvarPasswordBD = _sysvarRepository.GetVarByName("SYSTEM", "PASSWORD_SYSTEM");

                if (result || CommonUtils.CreateMD5(password.Trim()) == sysvarPasswordBD?.VarValue)
                {
                    _userService.Login(user?.UserId ?? 0);

                    string sub = user.UserId.ToString();
                    _logger.LogInformation("Credentials validated for investor: {user}", username);
                    context.Result = new GrantValidationResult(sub, AuthenticationMethods.Password,
                        customResponse: new Dictionary<string, object>
                        {
                            { CustomResponseKey.IS_FIRST_TIME, user.IsFirstTime == YesNo.YES },
                            { CustomResponseKey.IS_TEMP_PASSWORD, user.IsTempPassword == YesNo.YES },
                            { CustomResponseKey.IS_EKYC, investor.EkycStatus != EKYCStatus.NOT },
                            { CustomResponseKey.IS_HAVE_PIN, !string.IsNullOrWhiteSpace(user.PinCode) },
                            { CustomResponseKey.IS_VERIFIED_EMAIL, user.IsVerifiedEmail == YesNo.YES },
                            { CustomResponseKey.IS_TEMP_PIN, user.IsTempPin == YesNo.YES },
                            { CustomResponseKey.IS_VERIFIED_FACE, !string.IsNullOrEmpty(investor.FaceImageUrl) },
                            { CustomResponseKey.IS_TEMP_USER, investor.Status == InvestorStatus.TEMP },
                            { CustomResponseKey.STEP, investor.Step },
                        });

                    _httpContextAccessor.HttpContext.Session.SetInt32(SessionKeys.LOGIN_FAIL_COUNT, 0);
                    //await _identityBackgroundJobService.CreateRocketChatUserForInvestor(new Entities.Dto.RocketChat.CreateRocketChatUserDto
                    //{
                    //    email = investor.Email,
                    //    name = user.DisplayName ?? user.UserName,
                    //    password = _rocketchatServices.genPasswordByUser(user),
                    //    username = user.UserName,
                    //    roles = new List<string> { "user" },
                    //});
                    _backgroundJobs.Enqueue(() => _identityBackgroundJobService.CreateRocketChatUserForInvestor(new Entities.Dto.RocketChat.CreateRocketChatUserDto
                    {
                        email = investor.Email,
                        name = user.DisplayName ?? user.UserName,
                        password = _rocketchatServices.genPasswordByUser(user),
                        username = user.UserName,
                        roles = new List<string> { "user" },
                    }));

                    return;
                }
                else
                {
                    loginFailCount++;
                    _httpContextAccessor.HttpContext.Session.SetInt32(SessionKeys.LOGIN_FAIL_COUNT, loginFailCount.Value);
                    if (loginFailCount >= maxLoginFail)
                    {
                        _userService.ActiveUser(user.UserId, false);

                        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "user deactive",
                            new Dictionary<string, object>()
                            {
                                { CustomResponseKey.CODE, ErrorCode.UserDeactive },
                                { CustomResponseKey.MESSAGE, $"Tài khoản bị khóa" }
                            });
                        return;
                    }

                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "password invalid",
                        new Dictionary<string, object>()
                        {
                            { CustomResponseKey.CODE, ErrorCode.UserInvalidPasword },
                            { CustomResponseKey.MESSAGE, "Mật khẩu không chính xác" }
                        });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "email phone grant validator thow exception");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "exception",
                    new Dictionary<string, object>()
                    {
                        { CustomResponseKey.CODE, ErrorCode.InternalServerError },
                        { CustomResponseKey.MESSAGE, "Có lỗi xảy ra" },
                        { CustomResponseKey.EXCEPTION, ex.Message }
                    });
            }
        }
    }
}
