using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.InvestApprove;
using EPIC.InvestEntities.Dto.InvestProject;
using EPIC.InvestEntities.Dto.Project;
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
using System.Net;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/project")]
    [ApiController]
    public class ProjectController : BaseController
    {
        private readonly IProjectServices _projectService;

        public ProjectController(ILogger<ProjectController> logger, IProjectServices projectService)
        {
            _logger = logger;
            _projectService = projectService;
        }

        /// <summary>
        /// Lấy danh sách dự án đầu tư theo đối tác
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find")]
        //[PermissionFilter(Permissions.InvestSPDT_DanhSach)]
        public APIResponse GetAll([FromQuery] FilterInvestProjectDto input)
        {
            try
            {
                var result = _projectService.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách dự án đầu tư tìm chính xác
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("find-all-trading-provider")]
        [PermissionFilter(Permissions.InvestPPDT_ThemMoi)]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ProjectDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllList(int pageNumber, int? pageSize, string keyword, int? status)
        {
            try
            {
                var result = _projectService.FindAllTradingProvider(pageSize ?? 100, pageNumber, keyword?.Trim(), status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiết dự án đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        [PermissionFilter(Permissions.InvestSPDT_ThongTinChung)]
        public APIResponse GetById(int id)
        {
            try
            {
                var result = _projectService.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo yêu cầu duyệt dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("request")]
        [PermissionFilter(Permissions.InvestSPDT_TrinhDuyet)]
        public APIResponse AddRequest([FromBody] CreateInvestRequestDto input)
        {
            try
            {
                _projectService.Request(input);
                return new APIResponse(Utils.StatusCode.Success, input, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("approve")]
        [PermissionFilter(Permissions.InvestPDYCTT_PheDuyetOrHuy)]
        public APIResponse Approve(InvestApproveDto input)
        {
            try
            {
                _projectService.Approve(input);
                return new APIResponse(Utils.StatusCode.Success, input, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Epic xác minh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("check")]
        [PermissionFilter(Permissions.InvestSPDT_EpicXacMinh)]
        public APIResponse Check([FromBody] InvestCheckDto input)
        {
            try
            {
                _projectService.Check(input);
                return new APIResponse(Utils.StatusCode.Success, input, 200, "Xác minh thành công");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy duyệt dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("cancel")]
        [PermissionFilter(Permissions.InvestPDYCTT_PheDuyetOrHuy)]
        public APIResponse Cancel([FromBody] InvestCancelDto input)
        {
            try
            {
                _projectService.Cancel(input);
                return new APIResponse(Utils.StatusCode.Success, input, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đóng-Mở
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("close-open")]
        [PermissionFilter(Permissions.InvestSPDT_Dong)]
        public APIResponse CloseOpen(int id)
        {
            try
            {
                _projectService.CloseOpen(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm mới dự án đầu tư theo đối tác
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.InvestSPDT_ThemMoi)]
        public APIResponse Add([FromBody] CreateProjectDto body)
        {
            try
            {
                var result = _projectService.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật dự án đầu tư theo đối tác
        /// </summary>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.InvestSPDT_CapNhat)]
        public APIResponse Update([FromBody] UpdateProjectDto body)
        {
            try
            {
                _projectService.Update(body);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa dự án đầu tư theo đối tác
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.InvestSPDT_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                var result = _projectService.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
