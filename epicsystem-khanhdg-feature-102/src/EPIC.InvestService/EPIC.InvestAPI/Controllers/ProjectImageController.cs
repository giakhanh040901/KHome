using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.ProjectImage;
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
    [Route("api/invest/project-image")]
    [ApiController]
    public class ProjectImageController : BaseController
    {
        private IProjectImageServices _projectImageServices;

        public ProjectImageController(ILogger<ProjectImageController> logger, IProjectImageServices projectImageServices)
        {
            _logger = logger;
            _projectImageServices = projectImageServices;
        }

        /// <summary>
        /// Thêm mới hình ảnh của dự án đầu tư
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.InvestSPDT_HADT_ThemMoi)]
        public APIResponse Add([FromBody] CreateProjectImageDto body)
        {
            try
            {
                _projectImageServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);

            }
        }

        /// <summary>
        /// Lấy danh sách hình ảnh của dự án đầu tư
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("find/{projectId}")]
        [PermissionFilter(Permissions.InvestSPDT_HADT_DanhSach)]
        public APIResponse FindById(int projectId)
        {
            try
            {
                var result = _projectImageServices.FindByProjectId(projectId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa hình ảnh của dự án đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.InvestSPDT_HADT_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                var result = _projectImageServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
