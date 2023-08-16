using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityEntities.Dto.Roles;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.IdentityServer.Controllers
{
    [Authorize]
    [Route("api/users/roles")]
    [ApiController]
    public class RolesController : BaseController 
    {
        private readonly IRoleServices _roleServices;

        public RolesController(ILogger<RolesController> logger,
            IRoleServices roleServices)
        {
            _logger = logger;
            _roleServices = roleServices;
        }

        /// <summary>
        /// Tìm danh sách role
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        public APIResponse FindAll([FromQuery] FilterRoleDto input)
        {
            try
            {
                var result = _roleServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm role mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public APIResponse AddRole(CreateRoleDto input)
        {
            try
            {
                var result = _roleServices.AddRoleTemplate(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiết role mẫu 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("find/{roleId}")]
        public APIResponse FindByIdTemplate(int roleId)
        {
            try
            {
                var result = _roleServices.FindByIdTemplate(roleId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        

        /// <summary>
        /// cập nhật role mẫu (tên)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public APIResponse UpdateRoleTemplate(UpdateRoleDto input)
        {
            try
            {
                var result = _roleServices.UpdateRoleTemplate(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        //update danh sách permission của role mẫu

        //update danh sách permission tối đa của đối tác (thêm xoá, dựa vào danh sách tích chọn)

        /// <summary>
        /// Active/Deactive trạng thái của role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPut("status")]
        public APIResponse ChangeStatusRole(int roleId)
        {
            try
            {
                _roleServices.ChangeStatusRole(roleId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa role mẫu
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpDelete("delete/{roleId}")]
        public APIResponse DeleteRoleTemplate(int roleId)
        {
            try
            {
                var result = _roleServices.DeleteRole(roleId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
