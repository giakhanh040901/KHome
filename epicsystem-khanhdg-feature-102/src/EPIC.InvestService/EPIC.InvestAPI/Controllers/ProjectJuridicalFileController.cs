using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.Project;
using EPIC.InvestEntities.Dto.ProjectJuridicalFile;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/project-juridical-file")]
    [ApiController]
    public class ProjectJuridicalFileController : BaseController
    {
        private IProjectJuridicalFileServices _juridicalFileServices;

        public ProjectJuridicalFileController(ILogger<ProjectJuridicalFileController> logger, IProjectJuridicalFileServices juridicalFileServices)
        {
            _logger = logger;
            _juridicalFileServices = juridicalFileServices;
        }

        /// <summary>
        /// Thêm mới hồ sơ pháp lý của dự án đầu tư
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.InvestSPDT_HSPL_ThemMoi)]
        public APIResponse AddJuridicalFile([FromBody] CreateProjectJuridicalFileDto body)
        {
            try
            {
                var result = _juridicalFileServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);

            }
        }

        /// <summary>
        /// Danh sách hồ sơ pháp lý của dự án đầu tư
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [Route("file/find")]
        [HttpGet]
        [PermissionFilter(Permissions.InvestSPDT_HSPL_DanhSach)]
        public APIResponse FindAll(int projectId, int pageNumber, int? pageSize, string keyword)
        {
            try
            {
                var result = _juridicalFileServices.FindAll(projectId, pageSize ?? 100, pageNumber, keyword?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết hồ sơ pháp lý của dự án đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        [PermissionFilter(Permissions.InvestSPDT_HSPL_DanhSach)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _juridicalFileServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa hồ sơ pháp lý của dự án đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.InvestSPDT_HSPL_DeleteFile)]
        public APIResponse Delete(int id)
        {
            try
            {
                var result = _juridicalFileServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật hồ sơ pháp lý của dự án đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        public APIResponse JuridicalFileUpdate(int id, [FromBody] UpdateProjectJuridicalFileDto body)
        {
            try
            {
                var result = _juridicalFileServices.Update(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
