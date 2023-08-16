using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.LoyaltyAPI.Controllers
{
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/loyalty/app/accumulate-point")]
    [ApiController]
    public class AppLoyHisAccumulatePointController : BaseController
    {
        private readonly IHisAccumulatePointServices _hisAccumulatePointServices;

        public AppLoyHisAccumulatePointController(ILogger<AppLoyHisAccumulatePointController> logger,
            IHisAccumulatePointServices hisAccumulatePointServices)
        {
            _logger = logger;
            _hisAccumulatePointServices = hisAccumulatePointServices;
        }

        /// <summary>
        /// Khách yêu cầu tiêu điểm
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("consume")]
        public APIResponse AppConsumePoint([FromBody] AppConsumePointDto dto)
        {
            try
            {
                _hisAccumulatePointServices.AppConsumePoint(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tab lịch sử điểm thưởng
        /// </summary>
        /// <returns></returns>
        [HttpGet("history")]
        [ProducesResponseType(typeof(APIResponse<List<AppViewAccumulatePointHistoryDto>>), (int)HttpStatusCode.OK)]
        public APIResponse AppConsumePoint(int? tradingProviderId)
        {
            try
            {
                var data = _hisAccumulatePointServices.AppGetAccumulatePointHistory(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

    }
}
