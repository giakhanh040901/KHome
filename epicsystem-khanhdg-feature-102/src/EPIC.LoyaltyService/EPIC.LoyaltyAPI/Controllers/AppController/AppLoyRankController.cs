using EPIC.Utils.Controllers;
using EPIC.Utils.Filter;
using EPIC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.Dto.LoyVoucher;
using System.Net;
using EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor;
using EPIC.DataAccess.Models;
using System.Threading.Tasks;
using EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint;
using System.Collections.Generic;
using EPIC.Utils.ConstantVariables.Loyalty;
using DocumentFormat.OpenXml.Office2010.Excel;
using Humanizer;
using EPIC.LoyaltyEntities.Dto.LoyRank;
using EPIC.WebAPIBase.FIlters;

namespace EPIC.LoyaltyAPI.Controllers
{
    [Authorize]
    [Route("api/loyalty/rank")]
    [ApiController]
    public class AppLoyRankController : BaseController
    {
        private readonly ILoyRankServices _loyRankServices;

        public AppLoyRankController(ILogger<AppLoyRankController> logger,
            ILoyRankServices loyRankServices)
        {
            _logger = logger;
            _loyRankServices = loyRankServices;
        }

        /// <summary>
        /// App Điểm và hạng hiện tại của khách
        /// </summary>
        /// <returns></returns>
        [HttpGet("investor")]
        [ProducesResponseType(typeof(APIResponse<AppViewRankPointDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindByInvestorId(int? tradingProviderId)
        {
            try
            {
                var result = _loyRankServices.FindByInvestorId(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

    }
}
