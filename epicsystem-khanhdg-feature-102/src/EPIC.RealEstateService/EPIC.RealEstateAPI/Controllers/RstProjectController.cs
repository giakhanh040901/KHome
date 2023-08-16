using EPIC.GarnerEntities.Dto.GarnerDistribution;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstApprove;
using EPIC.RealEstateEntities.Dto.RstProject;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/project")]
    [ApiController]
    public class RstProjectController : BaseController
    {
        private readonly IRstProjectServices _projectServices;

        public RstProjectController(ILogger<RstProjectController> logger, IRstProjectServices projectServices)
        {
            _logger = logger;
            _projectServices = projectServices;
        }

        /// <summary>
        /// Thêm dự án
        /// </summary>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateProjectOverview_ThemMoi)]
        [ProducesResponseType(typeof(APIResponse<RstProjectDto>), (int)HttpStatusCode.OK)]
        public APIResponse Add([FromBody] RstCreateProjectDto input)
        {
            try
            {
                var result = _projectServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật dự án
        /// </summary>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateProjectOverview_ThongTinChung_CapNhat)]
        [ProducesResponseType(typeof(APIResponse<RstProjectDto>), (int)HttpStatusCode.OK)]
        public APIResponse Update([FromBody] RstUpdateProjectDto input)
        {
            try
            {
                var result = _projectServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa dự án khi đang ở trạng thái khởi tạo
        /// </summary>
        [HttpPut("delete/{projectId}")]
        public APIResponse Delete(int projectId)
        {
            try
            {
                _projectServices.Delete(projectId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật nội dung tổng quan
        /// </summary>
        [HttpPut("update-overview-content")]
        public APIResponse UpdateProjectOverviewContent([FromBody] RstUpdateProjectOverviewContentDto input)
        {
            try
            {
                _projectServices.UpdateProjectOverviewContent(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// lấy danh sách dự án
        /// </summary>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateProjectOverview_DanhSach, Permissions.RealStateMenuProjectList_DanhSach, Permissions.RealStatePhanPhoi_DanhSach)]
        [ProducesResponseType(typeof(APIResponse<List<ViewRstProjectDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FilterRstProjectDto input)
        {
            try
            {
                var result = _projectServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// lấy danh sách dự án được phân phối cho đại lý
        /// </summary>
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(APIResponse<List<RstProjectDto>>), (int)HttpStatusCode.OK)]
        public APIResponse ProjectGetAll([FromQuery] List<int> tradingProviderIds)
        {
            try
            {
                var result = _projectServices.ProjectGetAll(tradingProviderIds);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// xem chi tiết dự án
        /// </summary>
        [HttpGet("find-by-id/{id}")]
        [ProducesResponseType(typeof(APIResponse<RstProjectDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _projectServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Yêu cầu trình duyệt dự án
        /// </summary>
        [HttpPut("request")]
        [PermissionFilter(Permissions.RealStateProjectOverview_TrinhDuyet)]
        public APIResponse ProjectRequest([FromBody] RstRequestDto input)
        {
            try
            {
                _projectServices.Request(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt dự án
        /// </summary>
        [HttpPut("approve")]
        [PermissionFilter(Permissions.RealStateProjectOverview_PheDuyet)]
        public APIResponse ProjectApprove([FromBody] RstApproveDto input)
        {
            try
            {
                _projectServices.Approve(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy duyệt dự án
        /// </summary>
        [HttpPut("cancel")]
        [PermissionFilter(Permissions.RealStateProjectOverview_PheDuyet)]
        public APIResponse ProjectCancel([FromBody] RstCancelDto input)
        {
            try
            {
                _projectServices.Cancel(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
