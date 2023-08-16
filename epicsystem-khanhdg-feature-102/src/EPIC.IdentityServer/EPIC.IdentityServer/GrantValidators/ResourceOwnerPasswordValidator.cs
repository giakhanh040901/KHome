using EPIC.BondDomain.Interfaces;
using EPIC.CoreDomain.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.IdentityDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace EPIC.IdentityServer.GrantValidators
{
    /// <summary>
    /// Kiểm tra tài khoản mật khẩu
    /// </summary>
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IEventService _events;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserServices _userService;
        private readonly IInvestorServices _investorServices;
        private EpicSchemaDbContext _dbContext;

        public ResourceOwnerPasswordValidator(
            IEventService events,
            IConfiguration configuration,
            ILogger<ResourceOwnerPasswordValidator> logger,
            IHttpContextAccessor httpContextAccessor,
            IUserServices userService,
            IInvestorServices investorServices,
            EpicSchemaDbContext dbContext)
        {
            _events = events;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _investorServices = investorServices;
            _dbContext = dbContext;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            _logger.LogInformation("ipaddress request password grant: {}", ipAddress);

            context.UserName = context.UserName?.Trim();
            context.Password = context.Password?.Trim();

            var user = _userService.GetByUserName(context.UserName);
            if (user != null)
            {
                if (user.Status == UserStatus.DEACTIVE)
                {
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "user deactive", interactive: false));
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "user deactive",
                        new Dictionary<string, object>()
                        {
                                { CustomResponseKey.CODE, ErrorCode.UserDeactive },
                                { CustomResponseKey.MESSAGE, "Tài khoản bị khóa" }
                        });
                    return;
                }

                if (user.UserType == UserTypes.INVESTOR)
                {
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "user is investor", interactive: false));
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "user is investor",
                        new Dictionary<string, object>()
                        {
                                { CustomResponseKey.CODE, ErrorCode.UserInvestorCanNotLoginCMS },
                                { CustomResponseKey.MESSAGE, "Tài khoản không có quyền đăng nhập trên CMS" }
                        });
                    return;
                }

                bool result = _userService.ValidatePassword(context.UserName, context.Password);
                if (result)
                {
                    // Cập nhật thời gian đăng nhập cuối cùng
                    _userService.Login(user?.UserId ?? 0);
                    // Lấy đối tượng HttpContext từ Dependency Injection (DI)
                    var contextInfo = _httpContextAccessor.HttpContext;

                    // Lấy thông tin trình duyệt
                    var header = contextInfo.Request.Headers;
                    string operatingSystem = "";
                    string browser = "";
                    var platFormCheck = header.ContainsKey("Sec-Ch-Ua-Platform");
                    if (platFormCheck)
                    {
                        var operatingSystemInfo = contextInfo.Request.Headers["Sec-Ch-Ua-Platform"].ToString();
                        operatingSystem = operatingSystemInfo.Length > 0 ? operatingSystemInfo.Replace("\"", "") : null;
                    }
                    var browserInfoCheck = header.ContainsKey("Sec-Ch-Ua");
                    if (browserInfoCheck)
                    {
                        var browserInfo = contextInfo.Request.Headers["Sec-Ch-Ua"].ToString();
                        var browserInfoString = browserInfo.Length > 0 ? browserInfo.Replace("\"", "").Split(',') : null;
                        browser = browserInfoString.Length > 0 ? browserInfoString[browserInfoString.Length - 1].Trim() : null;
                    }
                    var userFind = _dbContext.Users.FirstOrDefault(x => x.UserId == user.UserId);
                    if (userFind != null)
                    {
                        userFind.LastDevice = $"Hệ điều hành: {operatingSystem} - Trình duyệt: {browser}";
                    }
                    _dbContext.SaveChanges();

                    string sub = user.UserId.ToString();
                    _logger.LogInformation("Credentials validated for username: {username}", context.UserName);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, sub, context.UserName, interactive: false));

                    context.Result = new GrantValidationResult(sub, AuthenticationMethods.Password);
                    return;
                }
                else
                {
                    _logger.LogInformation("Authentication failed for username: {username}, reason: invalid credentials", context.UserName);
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid credentials", interactive: false));
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid password",
                        new Dictionary<string, object>()
                        {
                            { CustomResponseKey.CODE, ErrorCode.UserInvalidPasword },
                            { CustomResponseKey.MESSAGE, "Mật khẩu không chính xác" }
                        });
                    return;
                }
            }
            else
            {
                _logger.LogInformation("No user found matching username: {username}", context.UserName);
                await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username", interactive: false));
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid username",
                    new Dictionary<string, object>()
                    {
                            { CustomResponseKey.CODE, ErrorCode.NotFound },
                            { CustomResponseKey.MESSAGE, "Tài khoản không tồn tại" }
                    });
                    return;
            }
        }
    }
}
