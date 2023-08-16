using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstProjectMedia;
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
using System.Collections.Generic;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/project-media")]
    [ApiController]
    public class RstProjectMediaController : BaseController
    {
        private readonly IRstProjectMediaServices _rstProjectMediaServices;

        public RstProjectMediaController(ILogger<RstProjectMediaController> logger,
            IRstProjectMediaServices rstProjectMediaServices)
        {
            _logger = logger;
            _rstProjectMediaServices = rstProjectMediaServices;
        }

        /// <summary>
        /// Tìm danh sách hình ảnh dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateProjectOverview_HinhAnh)]
        public APIResponse FindAll([FromQuery] FilterRstProjectMediaDto input)
        {
            try
            {
                var result = _rstProjectMediaServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm hình ảnh dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateProjectOverview_HinhAnh_ThemMoi)]
        public APIResponse Add([FromBody] CreateRstProjectMediasDto input)
        {
            try
            {
                var result = _rstProjectMediaServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật hình ảnh dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateProjectOverview_HinhAnh_CapNhat)]
        public APIResponse Update([FromBody] UpdateRstProjectMediaDto input)
        {
            try
            {
                var result = _rstProjectMediaServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá hình ảnh dự án
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.RealStateProjectOverview_HinhAnh_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                _rstProjectMediaServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái hình ảnh dự án
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("change-status/{id}")]
        [PermissionFilter(Permissions.RealStateProjectOverview_HinhAnh_DoiTrangThai)]
        public APIResponse ChangeStatus(int id, string status)
        {
            try
            {
                var result = _rstProjectMediaServices.ChangeStatus(id, status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// update thứ tự hình ảnh dự án
        /// </summary>
        /// <returns></returns>
        [HttpPut("sort-order")]
        public APIResponse UpdateSortOrder(RstProjectMediaSortDto input)
        {
            try
            {
                _rstProjectMediaServices.UpdateSortOrder(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm hình ảnh dự án theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _rstProjectMediaServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách dự án orderBy Location
        /// </summary>
        /// <returns></returns>
        [HttpGet("find/{projectId}")]
        public APIResponse Find(int projectId, string location, string status)
        {
            try
            {
                var result = _rstProjectMediaServices.Find(projectId, location, status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
