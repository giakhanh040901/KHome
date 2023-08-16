using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstConfigContractCode;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/config-contract-code")]
    [ApiController]
    public class RstConfigContractCodeController : BaseController
    {
        private readonly IRstConfigContractCodeServices _rstConfigContractCodeServices;

        public RstConfigContractCodeController(ILogger<RstConfigContractCodeController> logger,
            IRstConfigContractCodeServices rstConfigContractCodeServices)
        {
            _logger = logger;
            _rstConfigContractCodeServices = rstConfigContractCodeServices;
        }

        /// <summary>
        /// Add cấu hình mã hợp đồng (Config contract sCode)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateCTMaHDCoc_ThemMoi, Permissions.RealStateCTMaHDGiaoDich_ThemMoi)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse AddConfigContractCode(CreateRstConfigContractCodeDto input)
        {
            try
            {
                _rstConfigContractCodeServices.AddConfigContractCode(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateCTMaHDCoc_CapNhat, Permissions.RealStateCTMaHDGiaoDich_CapNhat)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse UpdateConfigContractCode(UpdateRstConfigContractCodeDto input)
        {
            try
            {
                _rstConfigContractCodeServices.UpdateConfigContractCode(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("get-all")]
        [PermissionFilter(Permissions.RealStateCTMaHDCoc_DanhSach, Permissions.RealStateCTMaHDGiaoDich_DanhSach)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse GetAllConfigContractCode([FromQuery] FilterRstConfigContractCodeDto input)
        {
            try
            {
                var result = _rstConfigContractCodeServices.GetAllConfigContractCode(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("get-all-config")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse GetAllConfig([FromQuery] FilterRstConfigContractCodeDto input)
        {
            try
            {
                var result = _rstConfigContractCodeServices.GetAllConfig(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("get-all-status-active")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse GetAllConfigContractCodeStatusActive()
        {
            try
            {
                var result = _rstConfigContractCodeServices.GetAllConfigContractCodeStatusActive();
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
                var result = _rstConfigContractCodeServices.GetConfigContractCodeById(configContractCodeId);
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
        [PermissionFilter(Permissions.RealStateCTMaHDCoc_DoiTrangThai, Permissions.RealStateCTMaHDGiaoDich_DoiTrangThai)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse UpdateConfigContractCodeStatus(int configContractCodeId)
        {
            try
            {
                _rstConfigContractCodeServices.UpdateConfigContractCodeStatus(configContractCodeId);
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
        [PermissionFilter(Permissions.RealStateCTMaHDCoc_Xoa, Permissions.RealStateCTMaHDGiaoDich_Xoa)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse DeleteConfigContractCode(int configContractCodeId)
        {
            try
            {
                _rstConfigContractCodeServices.DeleteConfigContractCode(configContractCodeId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
