using EPIC.Entities.Dto.User;
using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityEntities.Dto.Permissions;
using EPIC.IdentityEntities.Dto.Roles;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace EPIC.IdentityServer.Controllers
{
    [Authorize]
    [Route("api/users/epic")]
    [ApiController]
    public class UserEpicController : BaseController
    {
        private readonly IRoleServices _roleServices;
        private readonly IUserServices _userService;
        private readonly IPermissionServices _permissionServices;

        public UserEpicController(
            ILogger<UserEpicController> logger,
            IUserServices userService,
            IPermissionServices permissionServices,
            IRoleServices roleServices)
        {
            _logger = logger;
            _roleServices = roleServices;
            _userService = userService;
            _permissionServices = permissionServices;
        }


        #region Thêm role cho Epic thường
        /// <summary>
        /// Lấy danh sách role lọc theo web
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("roles/find-all")]
        public APIResponse FindAllByEpic([FromQuery] FilterRoleDto input)
        {
            try
            {
                var result = _roleServices.FindAllByEpic(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm role cho Epic thường
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("roles/add")]
        public APIResponse AddRoleEpic([FromBody] CreateRolePermissionDto input)
        {
            try
            {
                var result = _roleServices.AddRole(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiết role của Epic
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("roles/find/{roleId}")]
        public APIResponse FindRoleBEpic(int roleId)
        {
            try
            {
                var result = _roleServices.FindRoleById(roleId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cậu nhật role
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("roles/update-permission/{roleId}")]
        public APIResponse UpdatePermissionInRoleToEpic(int roleId, [FromBody] CreateRolePermissionDto input)
        {
            try
            {
                _roleServices.UpdatePermissionInRole(roleId, input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion
    }
}
