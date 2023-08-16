using EPIC.Entities.Dto.BondInvestorAccount;
using EPIC.Entities.Dto.User;
using EPIC.IdentityDomain.Interfaces;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Shared.Filter;
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
    [Route("api/users")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserServices _userService;
        private readonly IPermissionServices _permissionServices;

        public UserController(ILogger<UserController> logger, IUserServices userService, IPermissionServices permissionServices)
        {
            _logger = logger;
            _userService = userService;
            _permissionServices = permissionServices;
        }

        #region tài khoản thường
        [HttpPost("create-user")]
        public APIResponse CreateUser([FromBody] CreateUserDto model)
        {
            try
            {
                _userService.Create(model);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Authorize]
        [HttpPut("update-user/{userId}")]
        public APIResponse UpdateUser([FromRoute] int userId, [FromBody] UpdateUserDto model)
        {
            try
            {
                var result = _userService.Update(userId, model);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Authorize]
        [HttpDelete("{userId}")]
        //[PermissionFilter(Permissions.CoreKHCN_Account_Delete)]
        public APIResponse DeleteUser(int userId)
        {
            try
            {
                _userService.DeleteByUserId(userId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Authorize]
        [HttpPut("active-user/{userId}")]
        public APIResponse ActiveUser([FromRoute] int userId, bool isActive)
        {
            try
            {
                var result = _userService.ActiveUser(userId, isActive);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        [Authorize]
        [HttpGet("find-by-id/{userId}")]
        public APIResponse FindById(int userId)
        {
            try
            {
                var result = _userService.FindById(userId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Authorize]
        [HttpGet("find-all-user")]
        public APIResponse FindAllUser(int pageSize, int pageNumber, string keyword)
        {
            try
            {
                var result = _userService.FindAll(pageSize, pageNumber, keyword);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("change-password")]
        public APIResponse ChangePasswordUser([FromBody] Entities.Dto.User.ChangePasswordDto input)
        {
            try
            {
                _userService.ChangePassword(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin của user đang đăng nhập để hiển thị
        /// </summary>
        /// <returns></returns>
        [HttpGet("my-info")]
        [Authorize]
        public APIResponse MyInfo()
        {
            try
            {
                var data = _userService.GetMyInfo();
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region fcm token
        /// <summary>
        /// Trả về list fcm token theo list user id
        /// </summary>
        /// <param name="listUserId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("get-fcm-token")]
        public APIResponse GetFcmToken([FromBody] List<decimal> listUserId)
        {
            try
            {
                var result = _userService.GetUsersFcmTokens(listUserId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Clear fcm token
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("clear-fcm-token")]
        public APIResponse ClearFcmToken([FromBody] ClearFcmTokenDto dto)
        {
            try
            {
                _userService.ClearFcmToken(dto.FcmToken);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        /// <summary>
        /// Lấy toàn bộ tài khoản loại của investor (dành cho app kèm theo list fcm_tokens)
        /// </summary>
        [HttpGet]
        [Route("find-users-fcm-tokens")]
        public APIResponse GetAllUserAndFcmTokens([FromQuery] FindBondInvestorAccountDto dto)
        {
            try
            {
                var investorAccount = _userService.GetByType(dto);
                return new APIResponse(Utils.StatusCode.Success, investorAccount, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách permission của tài khoản đang đăng nhập
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-permission")]
        public APIResponse GetPermission(int? permissionInWeb = null)
        {
            try
            {
                var result = _permissionServices.GetPermission(permissionInWeb);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Root đối tác tạo user đối tác thường, tạo root ĐLCS
        /// </summary>
        /// <returns></returns>
        [HttpPost("root-create-user")]
        public APIResponse RootCreateUser(CreateUserByRootDto input)
        {
            try
            {
                var result = _userService.RootCreateUser(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Root thay đổi mật khẩu cho các tài khoản cấp dưới
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("root-reset-password")]
        public APIResponse ResetPasswordByRoot(RootUpdatePasswordDto input)
        {
            try
            {
                _userService.ResetPasswordByRoot(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Active/Deactive cho các cho các tài khoản cấp dưới
        /// </summary>
        /// <returns></returns>
        [HttpPut("root-change-status")]
        public APIResponse ActiveUsersByRoot(int userId)
        {
            try
            {
                _userService.ActiveUsersByRoot(userId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Active/Deactive cho các cho các tài khoản cấp dưới
        /// </summary>
        /// <returns></returns>
        [HttpDelete("root-delete-user")]
        public APIResponse DeleteUsersByRoot(int userId)
        {
            try
            {
                _userService.DeleteUsersByRoot(userId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi mật khẩu của người dùng đang đăng nhập khi đang ở trong trạng thái mật khẩu tạm: IsTempPassword = Y
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("change-password-is-temp")]
        public APIResponse ChangePasswordTempByUser(UpdatePasswordUserDto input)
        {
            try
            {
                _userService.ChangePasswordTempByUser(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật tên hiển thị 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-display-name")]
        public APIResponse UpdateDisplayName(UpdateUserDto input)
        {
            try
            {
                _userService.UpdateDisplayNameUser(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");

            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật ảnh đại diện
        /// </summary>
        /// <param name="avatarImageUrl"></param>
        [HttpPut("update-avatar")]
        public APIResponse UpdateAvatar([FromBody] string avatarImageUrl)
        {
            try
            {
                _userService.UpdateAvatarUser(avatarImageUrl?.Trim());
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổ ra thông tin quyền riêng tư và cá nhân hoá
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-privacy-info")]
        public APIResponse GetPrivacyInfo()
        {
            try
            {
                var result = _userService.GetPrivacyInfo();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
