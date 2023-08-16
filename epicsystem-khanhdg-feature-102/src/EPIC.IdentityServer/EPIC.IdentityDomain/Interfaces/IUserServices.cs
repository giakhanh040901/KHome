using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BondInvestorAccount;
using EPIC.Entities.Dto.User;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityEntities.Dto.UsersChat;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPIC.IdentityDomain.Interfaces
{
    public interface IUserServices
    {
        Users FindById(int userId);
        bool ValidatePassword(string username, string password);
        void ChangePassword(ChangePasswordDto input);
        Users GetByUserName(string username);
        UserDto Create(CreateUserDto model);
        UserDto CreateUserPartner(CreateUserPartnerDto model);
        Task<UserDto> CreateUserTradingProvider(CreateUserTradingProviderDto model);
        int Delete(int userId);
        int Update(int id, UpdateUserDto model);
        int ActiveUser(decimal userId, bool isActive);
        PagingResult<Users> FindAll(int pageSize, int pageNumber, string keyword);
        PagingResult<Users> GetAllByPartnerId(int pageSize, int pageNumber, string keyword, int partnerId);
        PagingResult<Users> GetAllByTradingProvideId(int pageSize, int pageNumber, string keyword, int tradingProviderId);
        List<Claim> GetClaims(int userId);
        Users FindByInvestorId(int investorId);
        List<ViewUserFcmToken> GetUsersFcmTokens(List<decimal> listUserId);
        void ClearFcmToken(string fcmToken);
        PagingResult<UserIsInvestorForAppDto> GetByType(FindBondInvestorAccountDto dto);
        MyInfoDto GetMyInfo();
        void Login(decimal userId);

        UserDto RootCreateUser(CreateUserByRootDto model);
        /// <summary>
        /// Root thay đổi mật khẩu cho các tài khoản cấp dưới
        /// </summary>
        /// <param name="input"></param>
        void ResetPasswordByRoot(RootUpdatePasswordDto input);

        /// <summary>
        /// Active/Deactive cho các cấp ở dưới UserData đang đăng nhập
        /// </summary>
        void ActiveUsersByRoot(int userId);

        /// <summary>
        /// Xóa tài khoản cấp dưới
        /// </summary>
        void DeleteUsersByRoot(int userId);

        /// <summary>
        /// Thay đổi mật khẩu của người dùng đang đăng nhập khi đang ở trong trạng thái mật khẩu tạm: IsTempPassword = Y
        /// </summary>
        /// <param name="input"></param>
        public void ChangePasswordTempByUser(UpdatePasswordUserDto input);

        /// <summary>
        /// Lưu thông tin user chat từ app
        /// </summary>
        /// <param name="dto"></param>
        void SaveUserChatRoomInfo(CreateUsersChatInfoDto dto);

        /// <summary>
        /// Lấy thông tin user chat từ app
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PagingResult<ViewUsersChatInfoDto> GetUserChatRoomInfo(FindUserChatRoomDto dto);

        /// <summary>
        /// Xoá mềm user theo user id
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteByUserId(int userId);

        /// <summary>
        /// Update tên hiển thị 
        /// </summary>
        /// <param name="input"></param>
        void UpdateDisplayNameUser(UpdateUserDto input);

        /// <summary>
        /// Đổ ra thông tin quyền riêng tư và cá nhân hoá
        /// </summary>
        /// <returns></returns>
        PrivacyInfoDto GetPrivacyInfo();

        /// <summary>
        /// Cập nhật ảnh đại diện
        /// </summary>
        /// <param name="avatarImageUrl"></param>
        void UpdateAvatarUser(string avatarImageUrl);
    }
}
