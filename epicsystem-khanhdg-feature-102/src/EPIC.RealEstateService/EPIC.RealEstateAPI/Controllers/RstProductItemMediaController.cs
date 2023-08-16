using EPIC.RealEstateDomain.Implements;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateEntities.Dto.RstProductItemMedia;
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
    [Route("api/real-estate/product-item-media")]
    [ApiController]
    public class RstProductItemMediaController : BaseController
    {
        private readonly IRstProductItemMediaServices _rstProductItemMediaServices;

        public RstProductItemMediaController(ILogger<RstProductItemMediaController> logger,
            IRstProductItemMediaServices rstProductItemMediaServices)
        {
            _logger = logger;
            _rstProductItemMediaServices = rstProductItemMediaServices;
        }

        /// <summary>
        /// Thêm hình ảnh sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateProjectListDetail_HinhAnh_ThemMoi)]
        public APIResponse Add([FromBody] CreateRstProductItemMediasDto input)
        {
            try
            {
                var result = _rstProductItemMediaServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật hình ảnh sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateProjectListDetail_HinhAnh_CapNhat)]
        public APIResponse Update([FromBody] UpdateRstProductItemMediaDto input)
        {
            try
            {
                var result = _rstProductItemMediaServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
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
        public APIResponse UpdateSortOrder(RstProductItemMediaSortDto input)
        {
            try
            {
                _rstProductItemMediaServices.UpdateSortOrder(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá hình ảnh sản phẩm 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.RealStateProjectListDetail_HinhAnh_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                _rstProductItemMediaServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái hình ảnh sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("change-status/{id}")]
        [PermissionFilter(Permissions.RealStateProjectListDetail_HinhAnh_DoiTrangThai)]
        public APIResponse ChangeStatus(int id, string status)
        {
            try
            {
                var result = _rstProductItemMediaServices.ChangeStatus(id, status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm hình ảnh sản phẩm theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _rstProductItemMediaServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách hình ảnh sản phẩm orderBy location
        /// </summary>
        /// <returns></returns>
        [HttpGet("find/{productItemId}")]
        public APIResponse Find(int productItemId, string location, string status)
        {
            try
            {
                var result = _rstProductItemMediaServices.Find(productItemId, location, status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
