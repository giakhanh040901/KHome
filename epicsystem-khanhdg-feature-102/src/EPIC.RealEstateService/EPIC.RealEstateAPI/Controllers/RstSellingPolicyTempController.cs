using EPIC.RealEstateDomain.Implements;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstSellingPolicy;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/selling-policy-temp")]
    [ApiController]
    public class RstSellingPolicyTempController : BaseController
    {
        private readonly IRstSellingPolicyTempServices _rstSellingPolicyServices;
        public RstSellingPolicyTempController(ILogger<RstSellingPolicyTempController> logger,
            IRstSellingPolicyTempServices rstSellingPolicyServices)
        {
            _logger = logger;
            _rstSellingPolicyServices = rstSellingPolicyServices;
        }

        /// <summary>
        /// Tìm danh sách chính sách bán hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateCSBanHang_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterRstSellingPolicyTempDto input)
        {
            try
            {
                var result = _rstSellingPolicyServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm chính sách bán hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateCSBanHang_ThemMoi)]
        public APIResponse Add([FromBody] CreateRstSellingPolicyTempDto input)
        {
            try
            {
                var result = _rstSellingPolicyServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật chính sách bán hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateCSBanHang_CapNhat)]
        public APIResponse Update([FromBody] UpdateRstSellingPolicyTempDto input)
        {
            try
            {
                var result = _rstSellingPolicyServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá chính sách bán hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.RealStateCSBanHang_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                _rstSellingPolicyServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái chính sách bán hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-status/{id}")]
        [PermissionFilter(Permissions.RealStateCSBanHang_DoiTrangThai)]
        public APIResponse ChangeStatus(int id)
        {
            try
            {
                var result = _rstSellingPolicyServices.ChangeStatus(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm chính sách bán hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _rstSellingPolicyServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
