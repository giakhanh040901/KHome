using EPIC.DataAccess.Models;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityEntities.Dto.Roles;
using EPIC.IdentityEntities.Dto.UsersPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityDomain.Interfaces
{
    public interface IPermissionDataServices
    {
        UserRole AddRoleUser(CreateUserRoleDto input);
        /// <summary>
        /// Lấy danh sách tài khoản để phân quyền
        /// ROOT ADMIN lấy danh sách ROOT PARTNER mà nó tạo ra
        /// ROOT PARTNER lấy danh sách PARTNER thường và ROOT TRADING (trong bảng quan hệ EP_TRADING_PROVIDER_PARTNER)
        /// PARTNER thường lấy danh sách ROOT TRADING (trong bảng quan hệ EP_TRADING_PROVIDER_PARTNER)
        /// ROOT TRADING lấy danh sách đại lý sơ cấp thường của nó
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<UsersPartnerDto> FindAll(FilterUsersManagerDto input);
        List<int> FindRoleByUserId(int userId);
        /// <summary>
        /// Lấy role theo user đăng nhập
        /// </summary>
        /// <returns></returns>
        List<RoleDto> FindRoleByUserType();
    }
}
