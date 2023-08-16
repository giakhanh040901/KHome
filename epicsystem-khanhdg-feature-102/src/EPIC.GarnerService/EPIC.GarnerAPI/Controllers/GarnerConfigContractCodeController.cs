using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCode;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.Shared.Filter;
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

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/config-contract-code")]
    [ApiController]
    public class GarnerConfigContractCodeController : BaseController
    {
        private readonly IGarnerDistributionServices _garnerDistributionServices;

        public GarnerConfigContractCodeController(ILogger<GarnerConfigContractCodeController> logger,
            IGarnerDistributionServices garnerDistributionServices)
        {
            _logger = logger;
            _garnerDistributionServices = garnerDistributionServices;
        }
        /// <summary>
        /// Add cấu hình mã hợp đồng (Config contract sCode)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        [PermissionFilter(Permissions.GarnerMauHD_ThemMoi)]
        public APIResponse AddConfigContractCode(CreateConfigContractCodeDto input)
        {
            try
            {
                _garnerDistributionServices.AddConfigContractCode(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("update")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        [PermissionFilter(Permissions.GarnerMauHD_CapNhat)]
        public APIResponse UpdateConfigContractCode(UpdateConfigContractCodeDto input)
        {
            try
            {
                _garnerDistributionServices.UpdateConfigContractCode(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        [PermissionFilter(Permissions.GarnerMauHD_DanhSach)]
        public APIResponse GetAllConfigContractCode([FromQuery] FilterConfigContractCodeDto input)
        {
            try
            {
                var result = _garnerDistributionServices.GetAllConfigContractCode(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("get-all-status-active")]
        [ProducesResponseType(typeof(APIResponse<List<GarnerConfigContractCodeDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllConfigContractCodeStatusActive()
        {
            try
            {
                var result = _garnerDistributionServices.GetAllConfigContractCodeStatusActive();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("{configContractCodeId}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse GetConfigContractCodeById(int configContractCodeId)
        {
            try
            {
                var result = _garnerDistributionServices.GetConfigContractCodeById(configContractCodeId);
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
                _garnerDistributionServices.UpdateConfigContractCodeStatus(configContractCodeId);
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
        [PermissionFilter(Permissions.GarnerMauHD_Xoa)]
        public APIResponse DeleteConfigContractCode(int configContractCodeId)
        {
            try
            {
                _garnerDistributionServices.DeleteConfigContractCode(configContractCodeId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
