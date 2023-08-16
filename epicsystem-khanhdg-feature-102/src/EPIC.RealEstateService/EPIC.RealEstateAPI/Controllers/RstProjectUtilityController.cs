using EPIC.RealEstateDomain.Implements;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstConfigContractCode;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using EPIC.RealEstateEntities.DataEntities;
using System.Collections.Generic;
using EPIC.RealEstateEntities.Dto.RstProjectUtility;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/project-utility")]
    [ApiController]
    public class RstProjectUtilityController : BaseController
    {
        private readonly IRstProjectUtilityServices _rstProjectUtilityServices;

        public RstProjectUtilityController(ILogger<RstProjectUtilityController> logger,
            IRstProjectUtilityServices rstProjectUtilityServices)
        {
            _logger = logger;
            _rstProjectUtilityServices = rstProjectUtilityServices;
        }

        /// <summary>
        /// cập nhật tiện ích 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateProjectOverview_TienIchHeThong_QuanLy)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse UpdateProjectUtility([FromBody] CreateRstProjectUtilityDto input)
        {
            try
            {
                var result = _rstProjectUtilityServices.UpdateProjectUtility(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Bỏ chọn tiện ích
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse DeleteProjectUtility(int id)
        {
            try
            {
                _rstProjectUtilityServices.DeleteProjectUtility(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Find All danh sách tiện ích theo dự án
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateProjectOverview_TienIchHeThong_DanhSach)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse FindAllProjectUtility([FromQuery] FilterRstProjectUtilityDto input, int projectId)
        {
            try
            {
                var result = _rstProjectUtilityServices.FindAll(input, projectId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get all tiện ích
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-utility")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse GetAllProjectUtility()
        {
            try
            {
                var result = _rstProjectUtilityServices.GetAllUtility();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get All Nhóm tiện ích
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-group-utility")]
        [PermissionFilter(Permissions.RealStateProjectOverview_TienIchHeThong_DanhSach)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse GetAllProjectGroupUtility()
        {
            try
            {
                var result = _rstProjectUtilityServices.GetAllGroupUtility();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
