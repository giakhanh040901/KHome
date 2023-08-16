using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetailTemp;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/policy-detail-temp")]
    [ApiController]
    public class GarnerPolicyDetailTempController : BaseController
    {
        private readonly IGarnerPolicyDetailTempServices _garnerPolicyDetailTempServices;
        public GarnerPolicyDetailTempController(ILogger<GarnerPolicyDetailTempController> logger,
            IGarnerPolicyDetailTempServices garnerPolicyDetailTempServices)
        {
            _logger = logger;
            _garnerPolicyDetailTempServices = garnerPolicyDetailTempServices;
        }


        /// <summary>
        /// Tìm thông tin kỳ hạn mẫu theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _garnerPolicyDetailTempServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm kỳ hạn mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.GarnerCSM_KyHan_ThemMoi)]
        public APIResponse Add([FromBody] CreateGarnerPolicyDetailTempDto input)
        {
            try
            {
                _garnerPolicyDetailTempServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật kỳ hạn mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.GarnerCSM_KyHan_CapNhat)]
        public APIResponse Update([FromBody] UpdateGarnerPolicyDetailTempDto input)
        {
            try
            {
                _garnerPolicyDetailTempServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa kỳ hạn mẫu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.GarnerCSM_KyHan_Xoa)]

        public APIResponse Delete(int id)
        {
            try
            {
                _garnerPolicyDetailTempServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách kỳ hạn mẫu theo chính sách mẫu
        /// </summary>
        /// <param name="policyTempId"></param>
        /// <returns></returns>
        [HttpGet("find-by-policy-temp")]
        public APIResponse FindByPolicyTempId (int policyTempId)
        {
            try
            {
                var result = _garnerPolicyDetailTempServices.FindAll(policyTempId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
