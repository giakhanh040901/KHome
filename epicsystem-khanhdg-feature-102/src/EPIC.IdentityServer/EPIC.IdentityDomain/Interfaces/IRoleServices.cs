using EPIC.DataAccess.Models;
using EPIC.IdentityEntities.Dto.PartnerPermission;
using EPIC.IdentityEntities.Dto.Roles;
using EPIC.IdentityEntities.Dto.TradingProviderPermission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityDomain.Interfaces
{
    public interface IRoleServices
    {
        
        RoleDto AddRoleTemplate(CreateRoleDto input);
        RoleDto UpdateRoleTemplate(UpdateRoleDto input);
        RoleDto FindByIdTemplate(int roleId);
        RoleDto DeleteRole(int roleId);

        /// <summary>
        /// Active / Deactive trạng thái role
        /// </summary>
        /// <param name="roleId"></param>
        void ChangeStatusRole(int roleId);

        /// <summary>
        /// Thông tin role và permission 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        RolePermissionInfoDto FindRoleById(int roleId);

        /// <summary>
        /// Cập nhật permission cho role
        /// </summary>
        void UpdatePermissionInRole(int roleId, CreateRolePermissionDto input);

        PagingResult<RoleDto> FindAll(FilterRoleDto input);
        #region Thêm Role cho đối tác
        PagingResult<RoleDto> FindAllPartner(FilterRoleDto input);

        #endregion

        #region Thêm role cho đại lý
        /// <summary>
        /// Lấy danh sách role theo Web 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<RoleDto> FindAllRoleInWebTrading(FilterRoleDto input);

        #endregion

        #region Thêm Role cho Epic thường
        PagingResult<RoleDto> FindAllByEpic(FilterRoleDto input);
        RoleDto AddRole(CreateRolePermissionDto input);
        #endregion
    }
}
