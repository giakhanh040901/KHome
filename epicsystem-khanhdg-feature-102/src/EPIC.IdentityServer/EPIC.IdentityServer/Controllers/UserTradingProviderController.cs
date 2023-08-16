using EPIC.Entities.Dto.User;
using EPIC.IdentityDomain.Implements;
using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityEntities.Dto.Permissions;
using EPIC.IdentityEntities.Dto.Roles;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPIC.IdentityServer.Controllers
{
    [Authorize]
    [Route("api/users/trading-provider")]
    [ApiController]
    public class UserTradingProviderController : BaseController
    {
        private readonly IRoleServices _roleServices;
        private readonly IUserServices _userService;
        private readonly IPermissionServices _permissionServices;

        public UserTradingProviderController(
            ILogger<UserTradingProviderController> logger,
            IUserServices userService,
            IPermissionServices permissionServices,
            IRoleServices roleServices)
        {
            _logger = logger;
            _roleServices = roleServices;
            _userService = userService;
            _permissionServices = permissionServices;
        }
        #region Cấu hình Permission Tối đa cho ĐLSC
        /// <summary>
        /// Danh sách Permission của ĐLSC lọc theo Web
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="permissionInWeb"></param>
        /// <returns></returns>
        [HttpGet("max-permissions/web/find-all-list/{tradingProviderId}/{permissionInWeb}")]
        public APIResponse FindAllFullList(int tradingProviderId, int permissionInWeb)
        {
            try
            {
                var result = _permissionServices.FindAllListMaxPermissionInTrading(tradingProviderId, permissionInWeb, false);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm web cho ĐLCS
        /// </summary>
        /// <returns></returns>
        [HttpPost("max-permissions/web/add")]
        public APIResponse CreateMaxWebPermission(CreateMaxWebPermissionToTradingDto input)
        {
            try
            {
                var result = _permissionServices.CreateMaxWebPermissionToTrading(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách web của đại lý
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        [HttpGet("max-permissions/web/find-all-list/{tradingProviderId}")]
        public APIResponse FindAllFullList(int tradingProviderId)
        {
            try
            {
                var result = _permissionServices.FindAllListMaxPermissionInTrading(tradingProviderId, isGetWeb: true);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        /// <summary>
        /// Thêm permission tối đa cho web của đại lý
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        [HttpPut("max-permissions/permission-in-web/update/{tradingProviderId}")]
        public APIResponse UpdateListPermissionToTradingInWeb(UpdateMaxPermissionInWeb input, int tradingProviderId)
        {
            try
            {
                _permissionServices.UpdateListMaxPermissionToTradingInWeb(input, tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion
        /// <summary>
        /// Dlsc tạo user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [PermissionFilter(Permissions.InvestDaiLy_TKDN_ThemMoi)]

        public async Task<APIResponse> CreateUser([FromBody] CreateUserTradingProviderDto model)
        {
            try
            {
                var result = await _userService.CreateUserTradingProvider(model);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Authorize]
        [HttpGet("get-all")]
        [PermissionFilter(Permissions.InvestDaiLy_TKDN_DanhSach)]

        public APIResponse GetAllByTradingProvideId(int pageSize, int pageNumber, string keyword, int tradingProviderId)
        {
            try
            {
                var result = _userService.GetAllByTradingProvideId(pageSize, pageNumber, keyword, tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #region Thêm role cho Đại lý
        /// <summary>
        /// Lấy danh sách role lọc theo web
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("roles/find-all")]
        public APIResponse FindAllRoleInWebTrading([FromQuery] FilterRoleDto input)
        {
            try
            {
                var result = _roleServices.FindAllRoleInWebTrading(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm role cho đại lý
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("roles/add")]
        public APIResponse AddRoleTradingProvider([FromBody] CreateRolePermissionDto input)
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
        /// Xem chi tiết role của đại lý
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("roles/find/{roleId}")]
        public APIResponse FindRoleByTradingProviderId(int roleId)
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
