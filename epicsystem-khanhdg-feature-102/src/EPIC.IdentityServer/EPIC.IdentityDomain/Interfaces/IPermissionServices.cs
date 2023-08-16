using EPIC.IdentityEntities.Dto.PartnerPermission;
using EPIC.IdentityEntities.Dto.Permissions;
using EPIC.IdentityEntities.Dto.TradingProviderPermission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityDomain.Interfaces
{
    public interface IPermissionServices
    {
        /// <summary>
        /// Cấu hình Web tối đa cho Đối tác
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PartnerPermissionDto CreateMaxWebPermission(CreateMaxWebPermissionDto input);
        /// <summary>
        /// Lấy danh sách Permission tối đa của theo web của Partner
        /// </summary>
        /// <param name="partnerId"></param>
        /// <param name="permissionInWeb"></param>
        /// <param name="isGetWeb"></param>
        /// <returns></returns>
        List<PartnerPermissionDto> FindAllListMaxPermission(int partnerId, int permissionInWeb = 0, bool isGetWeb = false);
        void DeleteMaxWebPermission(int partnerId, string permissionWebKey);
        /// <summary>
        /// Update Permission tối đa trong web cho Đối tác
        /// </summary>
        /// <param name="permissionInWeb"></param>
        /// <param name="permissionKeys"></param>
        /// <param name="partnerId"></param>
        void UpdateListMaxPermissionInWeb(UpdateMaxPermissionInWeb input, int partnerId);
        /// <summary>
        /// Cấu hình Web tối đa cho đại lý sơ cấp
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        TradingProviderPermissionDto CreateMaxWebPermissionToTrading(CreateMaxWebPermissionToTradingDto input);
        /// <summary>
        /// Cấu hình permission tối đa cho web của đại lý sơ cấp
        /// </summary>
        /// <param name="tradingProviderId"></param>
        void UpdateListMaxPermissionToTradingInWeb(UpdateMaxPermissionInWeb input, int tradingProviderId);
        List<TradingProviderPermissionDto> FindAllListMaxPermissionInTrading(int tradingProviderId, int permissionInWeb = 0, bool isGetWeb = false);
        List<string> GetPermission(int? permissionInWeb = null);
    }
}
