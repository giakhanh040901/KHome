using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.CallCenterConfig;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.CoreAPI.Controllers
{
    /// <summary>
    /// Cấu hình tài khoản trực call center
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/core/call-center-config")]
    [ApiController]
    public class CallCenterConfigController : BaseController
    {
        private readonly ICallCenterConfigService _callCenterConfigService;
        public CallCenterConfigController(ILogger<CallCenterConfigController> logger,
            ICallCenterConfigService callCenterConfigService)
        {
            _logger = logger;
            _callCenterConfigService = callCenterConfigService;
        }

        /// <summary>
        /// Danh sách phân trang cấu hình tài khoản trong call center (cấu hỉnh cho cả root và đại lý)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        public APIResponse FindAll([FromQuery] FilterCallCenterConfigDto input)
        {
            try
            {
                var result = _callCenterConfigService.FindAllConfig(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật danh sách tài khoản trong call center (cấu hỉnh cho cả root và đại lý)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public APIResponse Update([FromBody] UpdateCallCenterConfigDto input)
        {
            try
            {
                _callCenterConfigService.UpdateConfig(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
