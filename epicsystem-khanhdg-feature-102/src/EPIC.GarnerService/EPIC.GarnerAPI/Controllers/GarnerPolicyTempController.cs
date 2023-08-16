using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerPolicyTemp;
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
    [Route("api/garner/policy-temp")]
    [ApiController]
    public class GarnerPolicyTempController : BaseController
    {
        private readonly IGarnerPolicyTempServices _garnerPolicyTempServices;

        public GarnerPolicyTempController(ILogger<GarnerPolicyTempController> logger,
            IGarnerPolicyTempServices garnerPolicyTempServices)
        {
            _logger = logger;
            _garnerPolicyTempServices = garnerPolicyTempServices;
        }

        /// <summary>
        /// lấy danh sách chính sách mẫu
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.GarnerCSM_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterPolicyTempDto input)
        {
            try
            {
                var result = _garnerPolicyTempServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm thông tin chính sách mẫu theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _garnerPolicyTempServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm chính sách mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.GarnerCSM_ThemMoi)]
        public APIResponse Add([FromBody] CreateGarnerPolicyTempDto input)
        {
            try
            {
                _garnerPolicyTempServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật chính sách mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.GarnerCSM_CapNhat)]
        public APIResponse Update([FromBody] UpdateGarnerPolicyTempDto input)
        {
            try
            {
                _garnerPolicyTempServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa chính sách mẫu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.GarnerCSM_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                _garnerPolicyTempServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// lấy danh sách chính sách mẫu
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all-no-permission")]
        [ProducesResponseType(typeof(APIResponse<List<GarnerPolicyTempDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllNoPermission(string status)
        {
            try
            {
                var result = _garnerPolicyTempServices.FindAllNoPermission(status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy kích hoạt chính sách
        /// </summary>
        /// <returns></returns>
        [HttpPut("change-status/{policyTempId}")]
        public APIResponse DeactivePolicyTemp(int policyTempId)
        {
            try
            {
                _garnerPolicyTempServices.ChangeStatusPolicyTemp(policyTempId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

    }
}
