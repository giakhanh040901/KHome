using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstOpenSellFile;
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
    [Route("api/real-estate/open-sell-file")]
    [ApiController]
    public class RstOpenSellFileController : BaseController
    {
        private readonly IRstOpenSellFileServices _rstOpenSellFileServices;

        public RstOpenSellFileController(ILogger<RstOpenSellFileController> logger,
            IRstOpenSellFileServices rstOpenSellFileServices)
        {
            _logger = logger;
            _rstOpenSellFileServices = rstOpenSellFileServices;
        }

        /// <summary>
        /// Danh sách hồ sơ mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateMoBan_HoSo_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterRstOpenSellFileDto input)
        {
            try
            {
                var result = _rstOpenSellFileServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm hồ sơ mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateMoBan_HoSo_ThemMoi)]
        public APIResponse Add([FromBody] CreateRstOpenSellFilesDto input)
        {
            try
            {
                var result = _rstOpenSellFileServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật hồ sơ mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateMoBan_HoSo_ChinhSua)]
        public APIResponse Update([FromBody] UpdateRstOpenSellFileDto input)
        {
            try
            {
                var result = _rstOpenSellFileServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá hồ sơ mở bán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.RealStateMoBan_HoSo_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                _rstOpenSellFileServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái hồ sơ mở bán
        /// </summary>
        /// <returns></returns>
        [HttpPut("change-status/{id}")]
        [PermissionFilter(Permissions.RealStateMoBan_HoSo_DoiTrangThai)]
        public APIResponse ChangeStatus(int id, string status)
        {
            try
            {
                var result = _rstOpenSellFileServices.ChangeStatus(id, status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm hồ sơ mở bán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _rstOpenSellFileServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}