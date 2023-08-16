using EPIC.Utils.ConstantVariables.Identity;
using EPIC.WebAPIBase;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Shared.Filter
{
    public class HangfireFilter : IDashboardAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        public HangfireFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new JsonWebKey(File.ReadAllText(_configuration.GetValue<string>(StartUpProductBase.Jwk))),
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out SecurityToken validatedToken);
                return principal.Claims.Any(c => c.Type == Utils.ConstantVariables.Shared.ClaimTypes.SysAdmin);
            }
            catch
            {
                return false;
            }
        }

        public bool Authorize(DashboardContext context)
        {
            var cookieToken = context.GetHttpContext().Request.Cookies.FirstOrDefault(c => c.Key == CookiesKeys.SysToken);
            if (cookieToken.Value != null)
            {
                if (ValidateToken(cookieToken.Value))
                {
                    return true;
                }
            }
            context.GetHttpContext().Response.Redirect("/api/users/view/login");
            return true;
        }
    }
}
