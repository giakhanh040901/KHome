using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstProjectUtility;
using EPIC.Utils.Controllers;
using EPIC.Utils.Filter;
using EPIC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System;
using EPIC.RealEstateEntities.Dto.RstProjectInformationShare;
using EPIC.RealEstateDomain.Implements;
using EPIC.WebAPIBase.FIlters;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/project-information-share")]
    [ApiController]
    public class RstProjectInformationShareController : BaseController
    {
        private readonly IRstProjectInformationShareServices _rstProjectShareServices;

        public RstProjectInformationShareController(ILogger<RstProjectInformationShareController> logger,
            IRstProjectInformationShareServices rstProjectShareServices)
        {
            _logger = logger;
            _rstProjectShareServices = rstProjectShareServices;
        }

        /// <summary>
        /// Thêm chia sẻ dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateProjectOverview_ChiaSeDuAn_ThemMoi)]
        public APIResponse Add([FromBody] CreateRstProjectInformationShareDto input)
        {
            try
            {
                var result = _rstProjectShareServices.AddProjectShare(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateProjectOverview_ChiaSeDuAn_CapNhat)]
        public APIResponse UpdateProjectShare([FromBody] UpdateRstProjectInformationShareDto input)
        {
            try
            {
                _rstProjectShareServices.UpdateProjectShare(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("change-status/{id}")]
        [PermissionFilter(Permissions.RealStateProjectOverview_ChiaSeDuAn_DoiTrangThai)]
        public APIResponse ChangeStatus(int id)
        {
            try
            {
                _rstProjectShareServices.ChangStatus(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.RealStateProjectOverview_ChiaSeDuAn_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                _rstProjectShareServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find-by-id/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _rstProjectShareServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateProjectOverview_ChiaSeDuAn_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterRstProjectInformationShareDto input)
        {
            try
            {
                var result = _rstProjectShareServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
