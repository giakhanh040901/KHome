using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstProjectFile;
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
    [Route("api/real-estate/project-file")]
    [ApiController]
    public class RstProjectFileController : BaseController
    {
        private readonly IRstProjectFileServices _rstProjectFileServices;
        public RstProjectFileController(ILogger<RstProjectFileController> logger,
            IRstProjectFileServices rstProjectFileServices)
        {
            _logger = logger;
            _rstProjectFileServices = rstProjectFileServices;
        }

        /// <summary>
        /// Danh sách hồ sơ dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateProjectOverview_HoSo_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterRstProjectFileDto input)
        {
            try
            {
                var result = _rstProjectFileServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm hồ sơ dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateProjectOverview_HoSo_ThemMoi)]
        public APIResponse Add([FromBody] CreateRstProjectFilesDto input)
        {
            try
            {
                var result = _rstProjectFileServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật hồ sơ dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateProjectOverview_HoSo_CapNhat)]
        public APIResponse Update([FromBody] UpdateRstProjectFileDto input)
        {
            try
            {
                var result = _rstProjectFileServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá hồ sơ dự án
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.RealStateProjectOverview_HoSo_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                _rstProjectFileServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái hồ sơ dự án
        /// </summary>
        /// <returns></returns>
        [HttpPut("change-status/{id}")]
        [PermissionFilter(Permissions.RealStateProjectOverview_HoSo_DoiTrangThai)]
        public APIResponse ChangeStatus(int id, string status)
        {
            try
            {
                var result = _rstProjectFileServices.ChangeStatus(id, status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm hồ sơ dự án
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _rstProjectFileServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
