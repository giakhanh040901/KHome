using EPIC.DataAccess.Models;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtConfigContractCode;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.InvConfigContractCode;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;

namespace EPIC.EventAPI.Controllers
{
    /// <summary>
    /// Controller Xử lý cấu trúc mã hợp đồng
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/event/config-contract-code")]
    [ApiController]
    public class EvtConfigContractCodeController : BaseController
    {
        private readonly IEvtConfigContractCodeServices _configContractCodeServices;

        public EvtConfigContractCodeController(ILogger<EvtConfigContractCodeController> logger, IEvtConfigContractCodeServices configContractCodeServices)
        {
            _logger = logger;
            _configContractCodeServices = configContractCodeServices;
        }

        /// <summary>
        /// Add cấu hình mã hợp đồng (Config contract Code)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(APIResponse<EvtConfigContractCodeDto>), (int)HttpStatusCode.OK)]
        public APIResponse AddConfigContractCode(CreateEvtConfigContractCodeDto input)
        {
            try
            {
                var result = _configContractCodeServices.AddConfigContractCode(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật cấu trúc mã hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse UpdateConfigContractCode(UpdateEvtConfigContractCodeDto input)
        {
            try
            {
                _configContractCodeServices.UpdateConfigContractCode(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find-all")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<EvtConfigContractCodeDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllConfigContractCode([FromQuery] FilterEvtConfigContractCodeDto input)
        {
            try
            {
                var result = _configContractCodeServices.GetAllConfigContractCode(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("get-all-status-active")]
        [ProducesResponseType(typeof(APIResponse<List<EvtConfigContractCodeDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllConfigContractCodeStatusActive()
        {
            try
            {
                var result = _configContractCodeServices.GetAllConfigContractCodeStatusActive();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("{configContractCodeId}")]
        [ProducesResponseType(typeof(APIResponse<EvtConfigContractCodeDto>), (int)HttpStatusCode.OK)]
        public APIResponse GetConfigContractCodeById(int configContractCodeId)
        {
            try
            {
                var result = _configContractCodeServices.GetConfigContractCodeById(configContractCodeId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổi trạng thái config theo id
        /// </summary>
        /// <param name="configContractCodeId"></param>
        /// <returns></returns>
        [HttpPut("update-status/{configContractCodeId}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse UpdateConfigContractCodeStatus(int configContractCodeId)
        {
            try
            {
                _configContractCodeServices.UpdateConfigContractCodeStatus(configContractCodeId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa theo id
        /// </summary>
        /// <param name="configContractCodeId"></param>
        /// <returns></returns>
        [HttpDelete("delete/{configContractCodeId}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse DeleteConfigContractCode(int configContractCodeId)
        {
            try
            {
                _configContractCodeServices.DeleteConfigContractCode(configContractCodeId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
