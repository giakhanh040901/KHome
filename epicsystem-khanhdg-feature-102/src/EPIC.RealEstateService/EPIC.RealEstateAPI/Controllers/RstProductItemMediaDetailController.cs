using EPIC.RealEstateDomain.Implements;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstProductItemMediaDetail;
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

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/product-item-media-detail")]
    [ApiController]
    public class RstProductItemMediaDetailController : BaseController
    {
        private readonly IRstProductItemMediaDetailServices _rstProductItemMediaDetailServices;

        public RstProductItemMediaDetailController(ILogger<RstProductItemMediaDetailController> logger,
            IRstProductItemMediaDetailServices rstProductItemMediaDetailServices)
        {
            _logger = logger;
            _rstProductItemMediaDetailServices = rstProductItemMediaDetailServices;
        }

        /// <summary>
        /// Thêm nhóm hình ảnh sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateProjectListDetail_NhomHinhAnh_ThemMoi)]
        public APIResponse Add([FromBody] CreateRstProductItemMediaDetailsDto input)
        {
            try
            {
                var result = _rstProductItemMediaDetailServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật nhóm hình ảnh sảnp hẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateProjectListDetail_NhomHinhAnh_CapNhat)]
        public APIResponse Update([FromBody] UpdateRstProductItemMediaDetailDto input)
        {
            try
            {
                var result = _rstProductItemMediaDetailServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá nhóm hình ảnh sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.RealStateProjectListDetail_NhomHinhAnh_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                _rstProductItemMediaDetailServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// update thứ tự hình ảnh
        /// </summary>
        /// <returns></returns>
        [HttpPut("sort-order")]
        public APIResponse UpdateSortOrder(RstProductItemMediaDetailSortDto input)
        {
            try
            {
                _rstProductItemMediaDetailServices.UpdateSortOrder(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái nhóm hình ảnh sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("change-status/{id}")]
        [PermissionFilter(Permissions.RealStateProjectListDetail_NhomHinhAnh_DoiTrangThai)]
        public APIResponse ChangeStatus(int id, string status)
        {
            try
            {
                var result = _rstProductItemMediaDetailServices.ChangeStatus(id, status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm nhóm hình ảnh sản phẩm theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _rstProductItemMediaDetailServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách nhóm hình ảnh sản phẩm order by tên nhóm hình ảnh
        /// </summary>
        /// <returns></returns>
        [HttpGet("find/{productItemId}")]
        public APIResponse Find(int productItemId, string status)
        {
            try
            {
                var result = _rstProductItemMediaDetailServices.Find(productItemId, status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// thêm list product item media detail vào product item media
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add-list-detail")]
        public APIResponse Add([FromBody] AddRstProductItemMediaDetailsDto input)
        {
            try
            {
                var result = _rstProductItemMediaDetailServices.AddListMediaDetail(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
