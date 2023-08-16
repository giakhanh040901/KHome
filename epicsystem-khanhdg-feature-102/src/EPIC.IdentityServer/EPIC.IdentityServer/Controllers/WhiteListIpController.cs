using EPIC.IdentityDomain.Implements;
using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityEntities.Dto.Roles;
using EPIC.IdentityEntities.Dto.WhiteListIp;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EPIC.IdentityServer.Controllers
{
    [Route("api/users/white-list-ip")]
    [ApiController]
    public class WhiteListIpController : BaseController
    {
        private readonly IWhiteListIpServices _whiteListIpServices;

        public WhiteListIpController(ILogger<RolesController> logger,
            IWhiteListIpServices whiteListIpServices)
        {
            _logger = logger;
            _whiteListIpServices = whiteListIpServices;
        }

        /// <summary>
        /// Thêm mới danh sách White List IP
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public APIResponse CreateWhiteListIP(CreateWhiteListDto input)
        {
            try
            {
                _whiteListIpServices.CreateWhiteListIP(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem danh sách White List IP
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("get-all")]
        public APIResponse GetAllWhiteListIp([FromQuery] FilterWhiteListIpDto input)
        {
            try
            {
                var result = _whiteListIpServices.FindAllWhiteListIp(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiết White List IP
        /// </summary>
        /// <param name="whiteListIpId"></param>
        /// <returns></returns>
        [HttpGet("get-by-id/{whiteListIpId}")]
        public APIResponse GetById(int whiteListIpId)
        {
            try
            {
                var result = _whiteListIpServices.GetById(whiteListIpId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật White List IP
        /// </summary>
        /// <param name="whiteListIpId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public APIResponse UpdateWhiteListIP(int whiteListIpId, UpdateWhiteListIpDto input)
        {
            try
            {
                _whiteListIpServices.UpdateWhiteListIP(whiteListIpId, input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa White List IP
        /// </summary>
        /// <param name="whiteListIpId"></param>
        /// <returns></returns>
        [HttpDelete("delete/{whiteListIpId}")]
        public APIResponse DeleteWhiteListIP(int whiteListIpId)
        {
            try
            {
                _whiteListIpServices.DeleteWhiteListIP(whiteListIpId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
