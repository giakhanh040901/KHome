using EPIC.Entities.Dto.User;
using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityEntities.Dto.Permissions;
using EPIC.IdentityEntities.Dto.Roles;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EPIC.IdentityServer.Controllers
{
    [Authorize]
    [Route("api/users/partner")]
    [ApiController]
    public class UserPartnerController : BaseController
    {
        private readonly IUserServices _userService;
        private readonly IRoleServices _roleServices;
        private readonly IPermissionServices _permissionServices;

        public UserPartnerController(
            ILogger<UserPartnerController> logger,
            IUserServices userService,
            IRoleServices roleServices,
            IPermissionServices permissionServices)
        {
            _logger = logger;
            _userService = userService;
            _roleServices = roleServices;
            _permissionServices=permissionServices;
        }

        #region permission tối đa
        /// <summary>
        /// Danh sách web của đối tác
        /// </summary>
        /// <returns></returns>
        [HttpGet("max-permissions/web/find-all-list/{partnerId}")]
        public APIResponse FindAllFullList(int partnerId)
        {
            try
            {
                var result = _permissionServices.FindAllListMaxPermission(partnerId, isGetWeb: true);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm web cho đối tác
        /// </summary>
        /// <returns></returns>
        [HttpPost("max-permissions/web/add")]
        public APIResponse CreateMaxWebPermission(CreateMaxWebPermissionDto input)
        {
            try
            {
                var result = _permissionServices.CreateMaxWebPermission(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá web của đối tác
        /// </summary>
        /// <param name="partnerId"></param>
        /// <param name="permissionWebKey"></param>
        /// <returns></returns>
        [HttpDelete("max-permissions/web/delete")]
        public APIResponse DeleteMaxWebPermission(int partnerId, string permissionWebKey)
        {
            try
            {
                _permissionServices.DeleteMaxWebPermission(partnerId, permissionWebKey);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách permission trong web
        /// </summary>
        /// <returns></returns>
        [HttpGet("max-permissions/web/find-all-list/{partnerId}/{permissionInWeb}")]
        public APIResponse FindAllFullList(int partnerId, int permissionInWeb)
        {
            try
            {
                var result = _permissionServices.FindAllListMaxPermission(partnerId, permissionInWeb, false);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cấu hình permission tối đa cho web của đối tác
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        [HttpPut("max-permissions/permission-in-web/update/{partnerId}")]
        public APIResponse UpdateListPermissionInWeb(UpdateMaxPermissionInWeb input, int partnerId)
        {
            try
            {
                _permissionServices.UpdateListMaxPermissionInWeb(input, partnerId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region users
        /// <summary>
        /// Thêm user cho partner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public APIResponse CreateUser([FromBody] CreateUserPartnerDto model)
        {
            try
            {
                var result = _userService.CreateUserPartner(model);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// all user của partner
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        [HttpGet("get-all")]
        public APIResponse GetAllByPartnerId(int pageSize, int pageNumber, string keyword, int partnerId)
        {
            try
            {
                var result = _userService.GetAllByPartnerId(pageSize, pageNumber, keyword, partnerId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region roles
        /// <summary>
        /// danh sách role của đối tác
        /// </summary>
        /// <returns></returns>
        [HttpGet("roles/find-all")]
        public APIResponse FindAllPartner([FromQuery] FilterRoleDto input)
        {
            try
            {
                var result = _roleServices.FindAllPartner(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm role cho partner
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("roles/add")]
        public APIResponse AddRolePartner([FromBody] CreateRolePermissionDto input)
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
        /// Xem chi tiết role của đối tác
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("roles/find/{roleId}")]
        public APIResponse FindByIdByPartner(int roleId)
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
        #endregion

        #region permission trong role
        //xem danh sách permission trong role

        //update danh sách permission trong role (thêm, xoá)
        [HttpPut("roles/update-permission/{roleId}")]
        public APIResponse UpdatePermissionInRole(int roleId, [FromBody] CreateRolePermissionDto input)
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
