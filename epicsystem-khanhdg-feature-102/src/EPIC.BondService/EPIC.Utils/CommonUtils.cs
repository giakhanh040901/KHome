using Shared = EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace EPIC.Utils
{
    public static class CommonUtils
    {
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes); // .NET 5 +
            }
        }

        public static string RandomNumber(int length = 6)
        {
            Random random = new Random();
            const string chars = "0123456789";

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GetCurrentRemoteIpAddress(IHttpContextAccessor httpContextAccessor)
        {
            string senderIpv4 = null;
            try
            {
                senderIpv4 = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedIps))
                {
                    senderIpv4 = forwardedIps.FirstOrDefault();
                }
            }
            catch
            {
            }
            return senderIpv4;
        }

        public static string GetCurrentXForwardedFor(IHttpContextAccessor httpContextAccessor)
        {
            string forwardedIpsStr = null;
            try
            {
                if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedIps))
                {
                    forwardedIpsStr = JsonSerializer.Serialize(forwardedIps.ToList());
                }
            }
            catch
            {
            }
            return forwardedIpsStr;
        }

        public static string GetCurrentUsername(IHttpContextAccessor httpContextAccessor)
        {
            var usr = httpContextAccessor.HttpContext?.User.FindFirst(Shared.ClaimTypes.Username);
            return usr != null ? usr.Value : "";
        }

        public static int GetCurrentUserId(IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            //nếu trong startup dùng JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //thì các claim type sẽ không bị ghi đè tên nên phải dùng trực tiếp "sub"
            var claim = claims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) ?? claims?.FindFirst("sub");
            if (claim == null)
            {
                throw new FaultException(new FaultReason($"Tài khoản không chứa claim \"{System.Security.Claims.ClaimTypes.NameIdentifier}\""),
                    new FaultCode(((int)ErrorCode.NotHaveClaim).ToString()), "");
            }
            int userId = int.Parse(claim.Value);
            return userId;
        }

        public static int GetCurrentInvestorId(IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var claim = claims?.FindFirst(Shared.ClaimTypes.InvestorId);
            if (claim == null)
            {
                throw new FaultException(new FaultReason($"Tài khoản không chứa claim \"{Shared.ClaimTypes.InvestorId}\""),
                    new FaultCode(((int)ErrorCode.NotHaveClaim).ToString()), "");
            }
            int investorId = int.Parse(claim?.Value);
            return investorId;
        }

        public static string GetCurrentUserType(IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var claim = claims?.FindFirst(Shared.ClaimTypes.UserType);
            if (claim == null)
            {
                throw new FaultException(new FaultReason($"Tài khoản không chứa claim \"{Shared.ClaimTypes.UserType}\""),
                    new FaultCode(((int)ErrorCode.NotHaveClaim).ToString()), "");
            }
            return claim?.Value;
        }

        /// <summary>
        /// Lấy ip khi login
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        public static string GetCurrentIpAddressInToken(IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var claim = claims?.FindFirst(Shared.ClaimTypes.IpAddressLogin);
            if (claim == null)
            {
                //throw new FaultException(new FaultReason($"Tài khoản không chứa claim \"{Shared.ClaimTypes.IpAddressLogin}\""),
                //    new FaultCode(((int)ErrorCode.NotHaveClaim).ToString()), "");
                return null;
            }
            return claim?.Value;
        }

        /// <summary>
        /// Lấy trading provider id hiện tại đang làm việc
        /// </summary>
        public static int GetCurrentTradingProviderId(IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var claim = claims?.FindFirst(Shared.ClaimTypes.TradingProviderId);
            if (claim == null)
            {
                throw new FaultException(new FaultReason($"Tài khoản không chứa claim \"{Shared.ClaimTypes.TradingProviderId}\""),
                    new FaultCode(((int)ErrorCode.NotHaveClaim).ToString()), "");
            }
            return int.Parse(claim?.Value);
        }

        /// <summary>
        /// Lấy partner id đang làm việc hiện tại
        /// </summary>
        public static int GetCurrentPartnerId(IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var claim = claims?.FindFirst(Shared.ClaimTypes.PartnerId);
            if (claim == null)
            {
                throw new FaultException(new FaultReason($"Tài khoản không chứa claim \"{Shared.ClaimTypes.PartnerId}\""),
                    new FaultCode(((int)ErrorCode.NotHaveClaim).ToString()), "");
            }
            return int.Parse(claim?.Value);
        }

        public static int GetCurrentSaleId(IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var claim = claims?.FindFirst(Shared.ClaimTypes.SaleId);
            if (claim == null)
            {
                throw new FaultException(new FaultReason($"Tài khoản không chứa claim \"{Shared.ClaimTypes.SaleId}\""),
                    new FaultCode(((int)ErrorCode.NotHaveClaim).ToString()), "");
            }
            return int.Parse(claim?.Value);
        }

        public static string GetCurrentClientId(IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var claim = claims.FindFirst(Shared.ClaimTypes.ClientId);
            if (claim == null)
            {
                throw new FaultException(new FaultReason($"Tài khoản không chứa claim \"{Shared.ClaimTypes.ClientId}\""),
                    new FaultCode(((int)ErrorCode.NotHaveClaim).ToString()), "");
            }
            return claim?.Value;
        }

        public static decimal GetCurrentTradingFilter(IHttpContextAccessor httpContextAccessor, string key)
        {
            var value = httpContextAccessor.HttpContext.Items[key];
            if (value == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy filter tradingProvider key"),
                    new FaultCode(((int)ErrorCode.NotHaveClaim).ToString()), "");
            }
            decimal tradingProviderId = decimal.Parse(value.ToString());
            return tradingProviderId;
        }

        public static TService GetService<TService>(IHttpContextAccessor httpContextAccessor) where TService : class
        {
            var service = httpContextAccessor.HttpContext.RequestServices.GetService(typeof(TService)) as TService;
            if (service == null)
            {
                throw new FaultException(new FaultReason($"Không lấy được service: {typeof(TService)}"),
                    new FaultCode(((int)ErrorCode.GetRequestService).ToString()), "");
            }
            return service;
        }
    }
}
