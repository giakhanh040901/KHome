using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstProjectPolicy;
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

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/project-policy")]
    [ApiController]
    public class RstProjectPolicyController : BaseController
    {
        private readonly IRstProjectPolicyServices _rstProjectPolicyServices;
        public RstProjectPolicyController(ILogger<RstProjectPolicyController> logger,
            IRstProjectPolicyServices rstProjectPolicyServices)
        {
            _logger = logger;
            _rstProjectPolicyServices = rstProjectPolicyServices;
        }

        /// <summary>
        /// Tìm danh sách chính sách ưu đãi chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateProjectOverview_ChinhSach_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterRstProjectPolicyDto input)
        {
            try
            {
                var result = _rstProjectPolicyServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm chính sách ưu đãi chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateProjectOverview_ChinhSach_ThemMoi)]
        public APIResponse Add([FromBody] CreateRstProjectPolicyDto input)
        {
            try
            {
                var result = _rstProjectPolicyServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật chính sách ưu đãi chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateProjectOverview_ChinhSach_CapNhat)]
        public APIResponse Update([FromBody] UpdateRstProjectPolicyDto input)
        {
            try
            {
                var result = _rstProjectPolicyServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá chính sách ưu đãi chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.RealStateProjectOverview_ChinhSach_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                _rstProjectPolicyServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái chính sách ưu đãi chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-status/{id}")]
        [PermissionFilter(Permissions.RealStateProjectOverview_ChinhSach_DoiTrangThai)]
        public APIResponse ChangeStatus(int id)
        {
            try
            {
                var result = _rstProjectPolicyServices.ChangeStatus(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm chính sách ưu đãi chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _rstProjectPolicyServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
