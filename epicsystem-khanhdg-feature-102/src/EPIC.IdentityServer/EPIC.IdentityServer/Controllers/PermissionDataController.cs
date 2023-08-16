using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityEntities.Dto.Roles;
using EPIC.IdentityEntities.Dto.UsersPartner;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.Utils.RolePermissions;
using EPIC.Utils.RolePermissions.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace EPIC.IdentityServer.Controllers
{
    [Route("api/users/permission-data")]
    [ApiController]
    public class PermissionDataController : BaseController
    {
        private readonly IPermissionDataServices _permissionDataServices;

        public PermissionDataController(
            ILogger<PermissionDataController> logger,
            IPermissionDataServices permissionDataServices)
        {
            _logger = logger;
            _permissionDataServices = permissionDataServices;
        }

        [HttpGet("get-all")]
        public APIResponse GetAllPermissionToCheck(int permissionInWeb)
        {
            try
            {
                var result = PermissionConfig.Configs;
                var filter = result.Where(r => r.Value.PermissionInWeb == permissionInWeb).ToDictionary(o => o.Key, o => o.Value);
                return new APIResponse(Utils.StatusCode.Success, filter, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm role vào user
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add-role")]
        public APIResponse CreateUserRole(CreateUserRoleDto input)
        {
            try
            {
                var result = _permissionDataServices.AddRoleUser(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách tài khoản để phân quyền
        /// Status trạng thái: A: ACTIVE, D: DEACTIVE, X: IS_DELETED(đã xóa)
        /// </summary>
        /// <returns></returns>
        [HttpGet("user-by-manager")]
        public APIResponse FindAllUserByPartner([FromQuery] FilterUsersManagerDto input)
        {
            try
            {
                var result = _permissionDataServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách Role được gán vào User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("role-by-user/{userId}")]
        public APIResponse FindRoleByUserId(int userId)
        {
            try
            {
                var result = _permissionDataServices.FindRoleByUserId(userId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách role của Epic Root đang đăng nhập
        /// </summary>
        /// <returns></returns>
        [HttpGet("role-by-epic")]
        public APIResponse FindRoleByEpic()
        {
            try
            {
                var result = _permissionDataServices.FindRoleByUserType();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách role của đối tác tạo đang đăng nhập
        /// </summary>
        /// <returns></returns>
        [HttpGet("role-by-partner")]
        public APIResponse FindRoleByPartnerId()
        {
            try
            {
                var result = _permissionDataServices.FindRoleByUserType();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách role của đại lý sơ cấp tạo đang đăng nhập
        /// </summary>
        /// <returns></returns>
        [HttpGet("role-by-trading-provider")]
        public APIResponse FindRoleByTradingProviderId()
        {
            try
            {
                var result = _permissionDataServices.FindRoleByUserType();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
