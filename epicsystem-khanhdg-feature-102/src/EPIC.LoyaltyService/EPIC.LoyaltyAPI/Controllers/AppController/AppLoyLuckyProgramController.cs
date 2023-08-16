using EPIC.Entities.DataEntities;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.Dto.LoyLuckyProgramInvestor;
using EPIC.LoyaltyEntities.Dto.LoyLuckyScenario;
using EPIC.LoyaltyEntities.Dto.LoyRank;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;

namespace EPIC.LoyaltyAPI.Controllers.AppController
{
    [Authorize]
    [Route("api/loyalty/app/lucky-program")]
    [ApiController]
    public class AppLoyLuckyProgramController : BaseController
    {
        private readonly ILoyLuckyProgramServices _loyLuckyProgramServices;
        public AppLoyLuckyProgramController(ILogger<AppLoyLuckyProgramController> logger,
            ILoyLuckyProgramServices loyLuckyProgramServices)
        {
            _logger = logger;
            _loyLuckyProgramServices = loyLuckyProgramServices;
        }

        /// <summary>
        /// Random giải thưởng
        /// </summary>
        /// <param name="luckyScenarioId">Id kịch bản của chương trình</param>
        /// <returns></returns>
        [HttpGet("random")]
        [ProducesResponseType(typeof(APIResponse<AppViewRankPointDto>), (int)HttpStatusCode.OK)]
        public APIResponse AppRandomPrizeByInvestor(int luckyScenarioId)
        {
            try
            {
                var result = _loyLuckyProgramServices.AppRandomPrizeByInvestor(luckyScenarioId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// APP - Danh sách chương trình được tham gia của nhà đầu tư theo Đại lý
        /// </summary>
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<AppLoyLuckyProgramByInvestorDto>>), (int)HttpStatusCode.OK)]
        public APIResponse AppGetAllLuckyProgram(int tradingProviderId)
        {
            try
            {
                var result = _loyLuckyProgramServices.AppGetAllLuckyProgram(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chương trình vòng quay may mắn
        /// </summary>
        [HttpGet("get-lucky-rotation-program")]
        [ProducesResponseType(typeof(APIResponse<AppLoyLuckyScenarioDto>), (int)HttpStatusCode.OK)]
        public APIResponse AppLoyLuckyScenarioRotationProgram(int tradingProviderId)
        {
            try
            {
                var result = _loyLuckyProgramServices.AppLoyLuckyScenarioRotationProgram(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
