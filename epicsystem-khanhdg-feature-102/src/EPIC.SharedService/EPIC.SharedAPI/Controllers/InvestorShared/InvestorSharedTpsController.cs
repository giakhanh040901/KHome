using EPIC.Shared.Filter;
using EPIC.SharedDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.SharedAPI.Controllers.InvestorShared
{
    [WhiteListIpFilter(WhiteListIpTypes.ThongTinKhachDauTuTps)]
    [Route("api/shared/investor-shared/tps")]
    [ApiController]
    public class InvestorSharedTpsController : BaseController
    {
        private readonly IInvestorSharedTpsServices _investorSharedServices;

        public InvestorSharedTpsController(ILogger<InvestorSharedTpsController> logger, IInvestorSharedTpsServices investorSharedServices)
        {
            _logger = logger;
            _investorSharedServices = investorSharedServices;
        }

        /// <summary>
        /// Lấy thông tin sổ lệnh của nhà đầu tư theo tài khoản chứng khoán
        /// </summary>
        /// <param name="stockTradingAccount"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("order/{stockTradingAccount}")]
        public APIResponse ListOrderInvestorByStockTradingAccountTPS(string stockTradingAccount, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _investorSharedServices.ListOrderInvestorByStockTradingAccount(SecurityCompany.TPS, stockTradingAccount?.Trim(), startDate, endDate);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin giấy tờ của nhà đầu tư theo tài khoản chứng khoán
        /// </summary>
        /// <param name="stockTradingAccount"></param>
        /// <returns></returns>
        [HttpGet("identification/{stockTradingAccount}")]
        public APIResponse InvestorIdenByStockTradingAccountTPS(string stockTradingAccount)
        {
            try
            {
                var result = _investorSharedServices.InvestorIdenByStockTradingAccount(SecurityCompany.TPS, stockTradingAccount?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
