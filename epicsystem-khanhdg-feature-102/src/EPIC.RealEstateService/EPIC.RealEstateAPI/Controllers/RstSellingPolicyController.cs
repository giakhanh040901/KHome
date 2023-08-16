using EPIC.RealEstateEntities.Dto.RstProductItemProjectPolicy;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using EPIC.RealEstateDomain.Interfaces;
using Microsoft.Extensions.Logging;
using EPIC.RealEstateEntities.Dto.RstSellingPolicy;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/selling-policy")]
    [ApiController]
    public class RstSellingPolicyController : BaseController
    {
        private readonly IRstSellingPolicyServices _rstSellingPolicyServices;
        public RstSellingPolicyController(ILogger<RstSellingPolicyController> logger,
            IRstSellingPolicyServices rstSellingPolicyServices)
        {
            _logger = logger;
            _rstSellingPolicyServices = rstSellingPolicyServices;
        }
        /// <summary>
        /// Thêm chính sách mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("add-policy")]
        [PermissionFilter(Permissions.RealStateMoBan_ChinhSach)]
        public APIResponse AddSellingPolicy([FromBody] CreateRstSellingPolicyDto input)
        {
            try
            {
                _rstSellingPolicyServices.AddSellingPolicy(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get danh sách chính sách được chọn
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all-policy")]
        [PermissionFilter(Permissions.RealStateMoBan_ChinhSach_DanhSach)]
        public APIResponse FindAllSellingPolicy([FromQuery] FilterRstSellingPolicyDto input)
        {
            try
            {
                var result = _rstSellingPolicyServices.FindAllSellingPolicy(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        ///Thay đổi trạng thái chính sách chính sách
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-status/{id}")]
        [PermissionFilter(Permissions.RealStateMoBan_ChinhSach_DoiTrangThai)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse ChangeStatusSellingPolicy(int id)
        {
            try
            {
                _rstSellingPolicyServices.ChangeStatusSellingPolicy(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
