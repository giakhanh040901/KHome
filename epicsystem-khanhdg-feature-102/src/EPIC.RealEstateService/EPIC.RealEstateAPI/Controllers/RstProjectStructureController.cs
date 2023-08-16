using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstProjectStructure;
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
    [Route("api/real-estate/project-structure")]
    [ApiController]
    public class RstProjectStructureController : BaseController
    {
        private readonly IRstProjectStructureServices _rstProjectStructureServicesServices;
        public RstProjectStructureController(ILogger<RstProjectStructureController> logger,
            IRstProjectStructureServices rstProjectStructureServicesServices)
        {
            _logger = logger;
            _rstProjectStructureServicesServices = rstProjectStructureServicesServices;
        }

        /// <summary>
        /// Tìm danh sách cấu trúc cấu thành dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateProjectOverview_TienIchHeThong_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterRstProjectStructureDto input)
        {
            try
            {
                var result = _rstProjectStructureServicesServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm cấu trúc cấu thành dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")] 
        [PermissionFilter(Permissions.RealStateProjectOverview_CauTruc_Them)]
        public APIResponse Add([FromBody] CreateRstProjectStructureDto input)
        {
            try
            {
                var result = _rstProjectStructureServicesServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật cấu trúc cấu thành dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateProjectOverview_CauTruc_Sua)]
        public APIResponse Update([FromBody] UpdateRstProjectStructureDto input)
        {
            try
            {
                var result = _rstProjectStructureServicesServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá cấu trúc cấu thành dự án
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.RealStateProjectOverview_CauTruc_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                _rstProjectStructureServicesServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm cấu trúc cấu thành dự án
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _rstProjectStructureServicesServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm cấu trúc cấu thành dự án theo projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("find-by-project-id/{projectId}")]
        public APIResponse FindByProjectId(int projectId)
        {
            try
            {
                var result = _rstProjectStructureServicesServices.FindByProjectId(projectId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách cấu trúc là con trong các node
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("find-all-child/{projectId}")]
        public APIResponse FindAllChild(int projectId)
        {
            try
            {
                var result = _rstProjectStructureServicesServices.FindAllChild(projectId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách cấu trúc theo mức mật độ
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all/level/{level}")]
        public APIResponse FindAllByLevel(int level, int projectId)
        {
            try
            {
                var result = _rstProjectStructureServicesServices.FindAllByLevel(level, projectId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
