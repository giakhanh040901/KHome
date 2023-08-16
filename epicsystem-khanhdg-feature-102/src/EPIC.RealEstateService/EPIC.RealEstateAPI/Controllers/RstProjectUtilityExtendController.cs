using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstProjectUtilityMedia;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using EPIC.RealEstateEntities.Dto.RstProjectUtilityExtend;
using EPIC.RealEstateDomain.Implements;
using System.Collections.Generic;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;

namespace EPIC.RealEstateAPI.Controllers
{

    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/project-utility-extend")]
    [ApiController]
    public class RstProjectUtilityExtendController : BaseController
    {
        private readonly IRstProjectUtilityExtendServices _rstProjectUtilityExtendServices;

        public RstProjectUtilityExtendController(ILogger<RstProjectUtilityExtendController> logger,
            IRstProjectUtilityExtendServices rstProjectUtilityExtendServices)
        {
            _logger = logger;
            _rstProjectUtilityExtendServices = rstProjectUtilityExtendServices;
        }

        /// <summary>
        /// Thêm tiện ích khác
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateProjectOverview_TienIchKhac_ThemMoi)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse AddProjectUtilityExtend([FromBody] CreateRstProjectUtilityExtendDto input)
        {
            try
            {
                var result = _rstProjectUtilityExtendServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật tiện ích khác
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateProjectOverview_TienIchKhac_CapNhat)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse UpdateProjectUtilityExtend([FromBody] UpdateRstProjectUtilityExtendDto input)
        {
            try
            {
                _rstProjectUtilityExtendServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa hình tiện ích khác
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.RealStateProjectOverview_TienIchKhac_Xoa)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse DeleteProjectUtilityExtend(int id)
        {
            try
            {
                _rstProjectUtilityExtendServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get all tiện ích khác
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-utility-extend")]
        [PermissionFilter(Permissions.RealStateProjectOverview_TienIchKhac_DanhSach)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse GetAllProjectUtilityExtend(int projectId)
        {
            try
            {
                var result = _rstProjectUtilityExtendServices.GetAll(projectId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get tiện ích khác theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse GetById(int id)
        {
            try
            {
                var result = _rstProjectUtilityExtendServices.GetById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        ///Thay đổi trạng thái
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-status/{id}")]
        [PermissionFilter(Permissions.RealStateProjectOverview_TienIchKhac_DoiTrangThai)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse ChangeStatusProjectUtilityExtend(int id)
        {
            try
            {
                _rstProjectUtilityExtendServices.ChangeStatus(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
