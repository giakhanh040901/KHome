using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicy;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicyTemp;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/distribution-policy")]
    [ApiController]
    public class RstDistributionPolicyController : BaseController
    {
        private readonly IRstDistributionPolicyServices _rstProjectDisPolicyServices;
        public RstDistributionPolicyController(ILogger<RstDistributionPolicyController> logger,
            IRstDistributionPolicyServices rstProjectDisPolicyServices)
        {
            _logger = logger;
            _rstProjectDisPolicyServices = rstProjectDisPolicyServices;
        }

        /// <summary>
        /// Tìm danh sách chính sách
        /// </summary>
        /// <param name="input"></param>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStatePhanPhoi_ChinhSach_DanhSach)]
        [ProducesResponseType(typeof(APIResponse<List<RstDistributionPolicyDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FilterRstDistributionPolicyDto input, int distributionId)
        {
            try
            {
                var result = _rstProjectDisPolicyServices.FindAll(input, distributionId);
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
        [PermissionFilter(Permissions.RealStatePhanPhoi_ChinhSach_ThemMoi)]
        public APIResponse Add([FromBody] CreateRstDistributionPolicyDto input)
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
        public APIResponse Update([FromBody] UpdateRstDistributionPolicyDto input)
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
        [PermissionFilter(Permissions.RealStatePhanPhoi_ChinhSach_Xoa)]
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
        [PermissionFilter(Permissions.RealStatePhanPhoi_ChinhSach_DoiTrangThai)]
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
        /// Thay đổi trạng thái chính sách chính sách phân phối
        /// </summary>
        [HttpPut("active-distribution-policy/{id}")]
        public APIResponse ActiveDistributionPolicy(int id, int DistributionId)
        {
            try
            {
                _rstProjectDisPolicyServices.ActiveDistributionPolicy(id, DistributionId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
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
        [ProducesResponseType(typeof(APIResponse<RstDistributionPolicyDto>), (int)HttpStatusCode.OK)]
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

        /// <summary>
        /// lấy danh sách chính sách phân phối theo mở bán
        /// </summary>
        [HttpGet("get-all-policy")]
        [ProducesResponseType(typeof(APIResponse<RstDistributionPolicyDto>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllPolicy([FromQuery] FilterDistrobutionPolicyDto input)
        {
            try
            {
                var result = _rstProjectDisPolicyServices.GetAllPolicy(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
