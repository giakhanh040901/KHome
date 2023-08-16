using EPIC.Entities.DataEntities;
using EPIC.IdentityServer.Models;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Security;
using IdentityServer4;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPIC.IdentityServer.Controllers
{
    [Route("api/users/view")]
    public class UserViewController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IdentityServerTools _identityServerTools;
        private readonly IHostEnvironment _env;

        public UserViewController(
            IConfiguration configuration, 
            IdentityServerTools identityServerTools,
            IHostEnvironment env)
        {
            _configuration = configuration;
            _identityServerTools = identityServerTools;
            _env = env;
        }

        [HttpGet("verify-email-success")]
        public IActionResult VerifyEmailSuccess()
        {
            var test = Request.Headers["key"];
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("post-login")]
        public async Task<IActionResult> PostLoginAsync(PostLoginDto input)
        {
            bool login = false;
            if (_env.EnvironmentName == EnvironmentNames.Production)
            {
                login = input.Username == "sys" 
                    && CryptographyUtils.ComputeSha256Hash(input.Password) == "0242636F0173EC0FE594CDF83A36BEE9BD4C0A4182833A52381B9826030EE3BC";
            }
            else
            {
                login = input.Username == "sys" && input.Password == "sys@123#*";
            }

            if (login)
            {
                ViewBag.Message = "Đăng nhập thành công";
                var claims = new List<Claim>
                {
                    new Claim(Utils.ConstantVariables.Shared.ClaimTypes.SysAdmin, "true")
                };
                int lifeTime = _configuration.GetValue<int>("IdentityServer:Default:AccessTokenLifetime");
                string accessToken = await _identityServerTools.IssueJwtAsync(lifeTime, claims);
                HttpContext.Response.Cookies.Append(CookiesKeys.SysToken, accessToken, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddSeconds(lifeTime)
                });
                return View("Login");
            }
            ViewBag.Message = "Tài khoản hoặc mật khẩu không chính xác";
            return View("Login");
        }
    }
}
