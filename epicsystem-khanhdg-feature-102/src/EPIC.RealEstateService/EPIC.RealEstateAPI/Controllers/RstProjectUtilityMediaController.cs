using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstProjectUtility;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System;
using EPIC.RealEstateDomain.Implements;
using EPIC.RealEstateEntities.Dto.RstProjectUtilityMedia;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/project-utility-media")]
    [ApiController]
    public class RstProjectUtilityMediaController : BaseController
    {
        private readonly IRstProjectUtilityMediaServices _rstProjectUtilityMediaServices;

        public RstProjectUtilityMediaController(ILogger<RstProjectUtilityMediaController> logger,
            IRstProjectUtilityMediaServices rstProjectUtilityMediaServices)
        {
            _logger = logger;
            _rstProjectUtilityMediaServices = rstProjectUtilityMediaServices;
        }

        /// <summary>
        /// Thêm hình ảnh minh họatiện ích 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateProjectOverview_TienIchMinhHoa_ThemMoi)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse AddProjectUtilityMedia(CreateRstProjectUtilityMediaDto input)
        {
            try
            {
                var result = _rstProjectUtilityMediaServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật hình ảnh minh họa tiện ích
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateProjectOverview_TienIchMinhHoa_CapNhat)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse UpdateProjectUtilityMedia(UpdateRstProjectUtilityMediaDto input)
        {
            try
            {
                var result = _rstProjectUtilityMediaServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa hình ảnh minh họa tiện ích
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.RealStateProjectOverview_TienIchMinhHoa_Xoa)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse DeleteProjectUtilityMedia(int id)
        {
            try
            {
                _rstProjectUtilityMediaServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
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
        [PermissionFilter(Permissions.RealStateProjectOverview_TienIchMinhHoa_DoiTrangThai)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse ChangeStatusProjectUtilityMedia(int id)
        {
            try
            {
                _rstProjectUtilityMediaServices.ChangeStatus(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get all hình ảnh tiện ích
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-utility-media")]
        [PermissionFilter(Permissions.RealStateProjectOverview_TienIchMinhHoa_DanhSach)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse GetAllProjectUtilityMedia(int projectId)
        {
            try
            {
                var result = _rstProjectUtilityMediaServices.GetAll(projectId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get ảnh tiện ích theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse GetById(int id)
        {
            try
            {
                var result = _rstProjectUtilityMediaServices.GetById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
