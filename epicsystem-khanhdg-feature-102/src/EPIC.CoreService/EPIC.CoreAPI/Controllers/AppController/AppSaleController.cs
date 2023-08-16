using EPIC.CoreDomain.Interfaces;
using EPIC.SIGN.CORE;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EPIC.CoreAPI.Controllers.AppController
{
    /// <summary>
    /// Quản lý về sale trên app
    /// </summary>
    [Authorize]
    [Route("api/core/sale")]
    [ApiController]
    public class AppSaleController : BaseController
    {
        private readonly ISaleServices _saleServices;
        private readonly ISaleExportCollapContractServices _saleExportCollapContractServices;

        public AppSaleController(ILogger<AppSaleController> logger, ISaleServices saleServices, ISaleExportCollapContractServices saleExportCollapContractServices)
        {
            _logger = logger;
            _saleServices = saleServices;
            _saleExportCollapContractServices = saleExportCollapContractServices;
        }

        /// <summary>
        /// tìm kiếm saler theo mã giới thiệu thuộc đlsc
        /// lỗi nếu: 1. không thấy mã gt đó là sale, 2. mã gt sale không thuộc đại lý, 3. mã gt sale không còn hoạt động
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="referralCode"></param>
        /// <returns></returns>
        [HttpGet("find-by-trading-provider")]
        public APIResponse FindByTradingProvider(int tradingProviderId, string referralCode)
        {
            try
            {
                var result = _saleServices.AppFindSaleByReferralCode(referralCode?.Trim(), tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        //danh sách sale trong phòng ban khi bạn là quản lý + danh sách sale là cộng tác viên của bạn

    }
}
