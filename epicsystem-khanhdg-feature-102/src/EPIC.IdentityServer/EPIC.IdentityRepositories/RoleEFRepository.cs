using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityEntities.Dto.Roles;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using System.Linq;

namespace EPIC.IdentityRepositories
{
    public class RoleEFRepository : BaseEFRepository<Role>
    {
        public RoleEFRepository(EpicSchemaDbContext dbContext) : base(dbContext, "SEQ_P_ROLE")
        {
        }

        /// <summary>
        /// Lấy danh sách role
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public PagingResult<Role> FindAll(FilterRoleDto input, int? partnerId = null, int? tradingProviderId = null, string userType = null)
        {
            PagingResult<Role> result = new();

            var roleQuery = (from role in _dbSet
                             let roleDefault = _epicSchemaDbContext.RoleDefaultRelationships.FirstOrDefault(rd => role.RoleType == RoleTypes.Default && rd.RoleId == role.Id && rd.Deleted == YesNo.NO
                                                && (userType == null || !UserTypes.ROOT_ADMIN_TYPES.Contains(userType)) && (partnerId == null || rd.PartnerId == partnerId) 
                                                && (tradingProviderId == null || rd.TradingProviderId == tradingProviderId))
                             where role.Deleted == YesNo.NO && role.PermissionInWeb == input.PermissionInWeb
                             && (input.Keyword == null || role.Name != null && role.Name.Contains(input.Keyword))
                             && (input.RoleType == null || input.RoleType == role.RoleType) && (input.Status == null || role.Status == input.Status)
                             && (partnerId == null || ((role.PartnerId == partnerId && role.RoleType == RoleTypes.Partner)
                                 || (role.RoleType == RoleTypes.Default && roleDefault != null)))
                             && (tradingProviderId == null || ((role.TradingProviderId == tradingProviderId && role.RoleType == RoleTypes.TradingProvider)
                                 || (role.RoleType == RoleTypes.Default && roleDefault != null)))
                             && (userType == null || (userType == UserTypes.ROOT_EPIC && (role.RoleType == RoleTypes.Epic || role.RoleType == RoleTypes.Default)))
                             select new Role
                             {
                                 Id = role.Id,
                                 Name = role.Name,
                                 PermissionInWeb = role.PermissionInWeb,
                                 RoleType = role.RoleType,
                                 TradingProviderId = role.TradingProviderId,
                                 PartnerId = role.PartnerId,
                                 CreatedBy = role.CreatedBy,
                                 CreatedDate = role.CreatedDate,
                                 ModifiedBy = role.ModifiedBy,
                                 ModifiedDate = role.ModifiedDate,
                                 Status = roleDefault == null ? role.Status : roleDefault.Status,
                             });

            roleQuery = roleQuery.OrderByDescending(r => r.Id);
            result.TotalItems = roleQuery.Count();
            if (input.PageSize != -1)
            {
                roleQuery = roleQuery.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = roleQuery;
            return result;
        }

        public void AddRoleDefault(int? partnerId, int? tradingProviderId)
        {
            /// Lấy danh sách role mặc định
            var roleDefaults = _dbSet.Where(r => r.RoleType == RoleTypes.Default && r.Status == Status.ACTIVE && r.Deleted == YesNo.NO);
            foreach (var item in roleDefaults)
            {
                if (_epicSchemaDbContext.RoleDefaultRelationships.Any(r => r.RoleId == item.Id && (partnerId == null || partnerId == r.PartnerId)
                        && (tradingProviderId == null || tradingProviderId == r.TradingProviderId)))
                {
                    continue;
                }    
                _epicSchemaDbContext.RoleDefaultRelationships.Add(new RoleDefaultRelationship
                {
                    Id = (int)NextKey(RoleDefaultRelationship.SEQ),
                    PartnerId = partnerId,
                    TradingProviderId = tradingProviderId,
                    RoleId = item.Id,
                    Status = item.Status,
                    Deleted = YesNo.NO
                });
            }
            _dbContext.SaveChanges();
        }
    }
}
