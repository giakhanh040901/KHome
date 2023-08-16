using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstProjectPolicy;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicyTemp;
using EPIC.GarnerEntities.Dto.GarnerDistribution;
using System.Collections.Generic;
using System.Net;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/distribution-policy-temp")]
    [ApiController]
    public class RstDistributionPolicyTempController : BaseController
    {
        private readonly IRstDistributionPolicyTempServices _rstProjectDisPolicyServices;
        public RstDistributionPolicyTempController(ILogger<RstDistributionPolicyTempController> logger,
            IRstDistributionPolicyTempServices rstProjectDisPolicyServices)
        {
            _logger = logger;
            _rstProjectDisPolicyServices = rstProjectDisPolicyServices;
        }

        /// <summary>
        /// Tìm danh sách chính sách
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateCSPhanPhoi_DanhSach, Permissions.RealStateCSBanHang_DanhSach)]
        [ProducesResponseType(typeof(APIResponse<List<RstDistributionPolicyTempDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FilterRstDistributionPolicyTempDto input)
        {
            try
            {
                var result = _rstProjectDisPolicyServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm chính sách phân phối
        /// </summary>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateCSPhanPhoi_ThemMoi, Permissions.RealStateCSBanHang_ThemMoi)]
        public APIResponse Add([FromBody] CreateRstDistributionPolicyTempDto input)
        {
            try
            {
                var result = _rstProjectDisPolicyServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật chính sách phân phối
        /// </summary>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateCSPhanPhoi_CapNhat, Permissions.RealStateCSBanHang_CapNhat)]
        public APIResponse Update([FromBody] UpdateRstDistributionPolicyTempDto input)
        {
            try
            {
                var result = _rstProjectDisPolicyServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá chính sách
        /// </summary>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.RealStateCSPhanPhoi_Xoa, Permissions.RealStateCSBanHang_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                _rstProjectDisPolicyServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái chính sách chính sách phân phối
        /// </summary>
        [HttpPut("change-status/{id}")]
        [PermissionFilter(Permissions.RealStateCSPhanPhoi_DoiTrangThai, Permissions.RealStateCSBanHang_DoiTrangThai)]
        public APIResponse ChangeStatus(int id)
        {
            try
            {
                var result = _rstProjectDisPolicyServices.ChangeStatus(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm chính sách phân phối
        /// </summary>
        [HttpGet("find-by-id/{id}")]
        [ProducesResponseType(typeof(APIResponse<RstDistributionPolicyTempDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _rstProjectDisPolicyServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
