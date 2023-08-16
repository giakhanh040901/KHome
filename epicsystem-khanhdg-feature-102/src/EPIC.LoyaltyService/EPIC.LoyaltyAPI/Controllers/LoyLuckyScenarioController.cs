using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.Dto.LoyLuckyProgram;
using EPIC.LoyaltyEntities.Dto.LoyLuckyRotationInterface;
using EPIC.LoyaltyEntities.Dto.LoyLuckyScenario;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;

namespace EPIC.LoyaltyAPI.Controllers
{
    [Route("api/loyalty/lucky-scenario")]
    [ApiController]
    public class LoyLuckyScenarioController : BaseController
    {
        private readonly ILoyLuckyScenarioServices _loyLuckyScenarioServices;

        public LoyLuckyScenarioController(ILogger<LoyLuckyScenarioController> logger,
            ILoyLuckyScenarioServices loyLuckyScenarioServices)
        {
            _logger = logger;
            _loyLuckyScenarioServices = loyLuckyScenarioServices;
        }

        /// <summary>
        /// Thêm mới kịch bản
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public APIResponse Add([FromForm] CreateLoyLuckyScenarioDto input)
        {
            try
            {
                _loyLuckyScenarioServices.AddLuckyScenario(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật kịch bản
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public APIResponse UpdateLuckyScenario([FromForm] UpdateLoyLuckyScenarioDto input)
        {
            try
            {
                _loyLuckyScenarioServices.UpdateLuckyScenario(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái
        /// </summary>
        /// <param name="luckyScenarioId"></param>
        /// <returns></returns>
        [HttpPut("change-status/{luckyScenarioId}")]
        public APIResponse ChangeStatusLuckyScenario(int luckyScenarioId)
        {
            try
            {
                _loyLuckyScenarioServices.ChangeStatusLuckyScenario(luckyScenarioId);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }

        }
        /// <summary>
        /// Xóa kịch bản
        /// </summary>
        /// <param name="luckyScenarioId"></param>
        /// <returns></returns>
        [HttpPut("delete/{luckyScenarioId}")]
        public APIResponse DeleteLuckyScenario(int luckyScenarioId)
        {
            try
            {
                _loyLuckyScenarioServices.DeleteLuckyScenario(luckyScenarioId);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật vòng quay của kịch bản
        /// </summary>
        /// <returns></returns>
        [HttpPut("rotation-interface/update")]
        public APIResponse UpdateLuckyRotationInterface([FromForm] CreateLoyLuckyRotationInterfaceDto input, int luckyScenarioId)
        {
            try
            {
                _loyLuckyScenarioServices.UpdateLuckyRotationInterface(input, luckyScenarioId);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }

        }

        /// <summary>
        /// Xem danh sách kịch bản
        /// </summary>
        /// <param name="luckyProgramId"></param>
        /// <returns></returns>
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(APIResponse<List<ViewLoyLuckyScenarioDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllLuckyScenario(int luckyProgramId)
        {
            try
            {
                var result = _loyLuckyScenarioServices.GetAllLuckyScenario(luckyProgramId);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiết kịch bản
        /// </summary>
        /// <param name="luckyScenarioId"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{luckyScenarioId}")]
        [ProducesResponseType(typeof(APIResponse<LoyLuckyScenarioDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindById(int luckyScenarioId)
        {
            try
            {
                var result = _loyLuckyScenarioServices.FindById(luckyScenarioId);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
