using EPIC.RealEstateDomain.Implements;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstProjectMedia;
using EPIC.RealEstateEntities.Dto.RstProjectMediaDetail;
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
    [Route("api/real-estate/project-media-detail")]
    [ApiController]
    public class RstProjectMediaDetailController : BaseController
    {
        private readonly IRstProjectMediaDetailServices _rstProjectMediaDetailServices;

        public RstProjectMediaDetailController(ILogger<RstProjectMediaDetailController> logger,
            IRstProjectMediaDetailServices rstProjectMediaDetailServices)
        {
            _logger = logger;
            _rstProjectMediaDetailServices = rstProjectMediaDetailServices;
        }

        /// <summary>
        /// Thêm nhóm hình ảnh dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateProjectOverview_NhomHinhAnh_ThemMoi)]
        public APIResponse Add([FromBody] CreateRstProjectMediaDetailsDto input)
        {
            try
            {
                var result = _rstProjectMediaDetailServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// thêm list project media detail vào project media
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add-list-detail")]
        public APIResponse Add([FromBody] AddRstProjectMediaDetailsDto input)
        {
            try
            {
                var result = _rstProjectMediaDetailServices.AddListMediaDetail(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật nhóm hình ảnh dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateProjectOverview_NhomHinhAnh_CapNhat)]
        public APIResponse Update([FromBody] UpdateRstProjectMediaDetailDto input)
        {
            try
            {
                var result = _rstProjectMediaDetailServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá nhóm hình ảnh dự án
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.RealStateProjectOverview_NhomHinhAnh_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                _rstProjectMediaDetailServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái nhóm hình ảnh dự án
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("change-status/{id}")]
        [PermissionFilter(Permissions.RealStateProjectOverview_NhomHinhAnh_DoiTrangThai)]
        public APIResponse ChangeStatus(int id, string status)
        {
            try
            {
                var result = _rstProjectMediaDetailServices.ChangeStatus(id, status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm nhóm hình ảnh dự án theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _rstProjectMediaDetailServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách nhóm hình ảnh dự án order by tên nhóm hình ảnh
        /// </summary>
        /// <returns></returns>
        [HttpGet("find/{projectId}")]
        public APIResponse Find(int projectId, string status)
        {
            try
            {
                var result = _rstProjectMediaDetailServices.Find(projectId, status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// update thứ tự nhóm hình ảnh dự án
        /// </summary>
        /// <returns></returns>
        [HttpPut("sort-order")]
        public APIResponse UpdateSortOrder(RstProjectMediaDetailSortDto input)
        {
            try
            {
                _rstProjectMediaDetailServices.UpdateSortOrder(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
