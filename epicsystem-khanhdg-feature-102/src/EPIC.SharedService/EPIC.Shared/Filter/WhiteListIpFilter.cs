using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.IdentityRepositories;
using EPIC.Shared.Middlewares;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NetTools;
using System;
using System.Linq;

namespace EPIC.Shared.Filter
{
    /// <summary>
    /// Filter white list ip, những nghiệp vụ nào dành cho đại lý sẽ bắt buộc lấy trading provider id trong token
    /// phụ thuộc vào nghiệp vụ trong hằng số <see cref="WhiteListIpTypes"/><br/>
    /// </summary>
    public class WhiteListIpFilter : Attribute, IAuthorizationFilter
    {
        private ILogger _logger;
        private IWebHostEnvironment _env;
        private IHttpContextAccessor _httpContextAccessor;
        private EpicSchemaDbContext _dbContext;
        private WhiteListIpEFRepository _whiteListIpEFRepository;
        private WhiteListIpDetailEFRepository _whiteListIpDetailEFRepository;
        private readonly int _type;

        /// <summary>
        /// Khởi tạo filter<br/>
        /// <paramref name="type"/> loại filter lọc theo nghiệp vụ nào <see cref="WhiteListIpTypes"/><br/>
        /// </summary>
        public WhiteListIpFilter(int type)
        {
            _type = type;
        }

        private void GetServices(AuthorizationFilterContext context)
        {
            _logger = context.HttpContext.RequestServices.GetService(typeof(ILogger<CheckLoginOtherDeviceMiddleware>)) as ILogger;
            _httpContextAccessor = context.HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            _env = context.HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment)) as IWebHostEnvironment;
            _dbContext = context.HttpContext.RequestServices.GetService(typeof(EpicSchemaDbContext)) as EpicSchemaDbContext;
            _whiteListIpEFRepository = new WhiteListIpEFRepository(_dbContext);
            _whiteListIpDetailEFRepository = new WhiteListIpDetailEFRepository(_dbContext);
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            GetServices(context);
            //môi trường dev thì bỏ qua
            if (_env.EnvironmentName == EnvironmentNames.Development 
                || _env.EnvironmentName == EnvironmentNames.DevelopmentWSL)
            {
                return;
            }

            var requestIp = CommonUtils.GetCurrentRemoteIpAddress(_httpContextAccessor);
            try
            {
                bool isGrant = false;
                if (WhiteListIpTypes.MustPassTradingId.Contains(_type)) //những white list ip cho trading
                {
                    int? tradingProviderId = null;
                    tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContextAccessor);
                    //var tradingFind = _dbContext.TradingProviders.FirstOrDefault(t => t.TradingProviderId == tradingProviderId);
                    //if (tradingFind.IsIpPayment == YesNo.NO)
                    //{
                    //    return;
                    //}
                    // Filter theo tradingId
                    var whiteListIpFind = _whiteListIpEFRepository.Entity
                        .FirstOrDefault(r => r.Type == _type && r.TradingProviderId == tradingProviderId && r.Deleted == YesNo.NO);
                    if (whiteListIpFind == null)
                    {
                        return;
                    }

                    var childList = _whiteListIpDetailEFRepository.Entity
                            .Where(c => c.WhiteListIpId == whiteListIpFind.Id && c.Deleted == YesNo.NO).ToList();

                    foreach (var child in childList)
                    {
                        var startIp = child.IpAddressStart;
                        var endIp = child.IpAddressEnd;
                        var rangeIp = IPAddressRange.Parse($"{startIp}-{endIp}");
                        isGrant = rangeIp.Contains(IPAddressRange.Parse(requestIp));
                        if (isGrant)
                        {
                            var whiteListIpDetail = _whiteListIpDetailEFRepository.Entity
                                .FirstOrDefault(t => t.Id == child.Id && t.Deleted == YesNo.NO);
                            _httpContextAccessor.HttpContext?.Items?
                                .Add(WhiteListIpTypes.TRADING_KEY, whiteListIpDetail.TradingProviderId);
                            return;
                        }
                    }
                }
                else // những white list ip root
                {
                    // tìm cha theo type
                    var whiteListIpQuery = _whiteListIpEFRepository.Entity
                        .Where(r => r.Type == _type && r.Deleted == YesNo.NO).ToList();
                    foreach (var whiteListIp in whiteListIpQuery)
                    {
                        if (whiteListIp == null)
                        {
                            throw new Exception($"Không có cấu hình white list ip: type = {_type}");
                        }
                        var childList = _whiteListIpDetailEFRepository.Entity.AsQueryable()
                            .Where(c => c.WhiteListIpId == whiteListIp.Id && c.Deleted == YesNo.NO).ToList();

                        foreach (var child in childList)
                        {
                            var startIp = child.IpAddressStart;
                            var endIp = child.IpAddressEnd;
                            var rangeIp = IPAddressRange.Parse($"{startIp}-{endIp}");
                            isGrant = rangeIp.Contains(IPAddressRange.Parse(requestIp));
                            if (isGrant)
                            {
                                var whiteListIpDetail = _whiteListIpDetailEFRepository.Entity
                                    .FirstOrDefault(t => t.Id == child.Id && t.Deleted == YesNo.NO);
                                return;
                            }
                        }
                    }
                }

                if (!isGrant)
                {
                    context.Result = new UnauthorizedObjectResult(new { message = $"Ip '{requestIp}' không có trong dải white list ip" });
                }
            }
            catch (Exception ex)
            {
                string message = $"Ip '{requestIp}' không có quyền truy cập do lỗi cấu hình";
                _logger.LogError(ex, message);
                context.Result = new UnauthorizedObjectResult(new { message });
            }
        }
    }
}
