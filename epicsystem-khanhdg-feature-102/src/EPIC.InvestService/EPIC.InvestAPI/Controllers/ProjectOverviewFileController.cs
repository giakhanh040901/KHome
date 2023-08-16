using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.ProjectOverViewFile;
using EPIC.Utils;
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
    [Route("api/invest/project-overview-file")]
    [ApiController]
    public class ProjectOverviewFileController : BaseController
    {
        private readonly IProjectOverviewFileServices _projectOverviewFiles;

        public ProjectOverviewFileController(ILogger<ProjectOverviewFileController> logger, IProjectOverviewFileServices projectOverviewFile)
        {
            _logger = logger;
            _projectOverviewFiles = projectOverviewFile;
        }

        /// <summary>
        /// Thêm mới file tổng quan dự án
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public APIResponse AddJuridicalFile([FromBody] CreateProjectOverviewFileDto body)
        {
            try
            {
                var result = _projectOverviewFiles.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);

            }
        }

        /// <summary>
        /// Danh sách file tổng quan dự án
        /// </summary>
        /// <param name="distributionId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Route("file/find")]
        [HttpGet]
        public APIResponse FindAll(int distributionId, int pageNumber, int? pageSize, int? status)
        {
            try
            {
                var result = _projectOverviewFiles.FindAll(distributionId, pageSize ?? 100, pageNumber, status);
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
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _projectOverviewFiles.FindById(id);
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
        public APIResponse Delete(int id)
        {
            try
            {
                var result = _projectOverviewFiles.Delete(id);
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
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        public APIResponse JuridicalFileUpdate([FromBody] UpdateProjectOverviewFileDto body)
        {
            try
            {
                var result = _projectOverviewFiles.Update(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
