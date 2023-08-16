using EPIC.DataAccess.Base;
using EPIC.IdentityEntities.DataEntities;
using EPIC.Utils;
using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityRepositories
{
    public class UserRoleEFRepository : BaseEFRepository<UserRole>
    {
        public UserRoleEFRepository(EpicSchemaDbContext dbContext) : base(dbContext, "SEQ_P_USER_ROLE")
        {
        }

        /// <summary>
        /// đếm số user đang sử dụng role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public int TotalUse(int roleId)
        {
            var userRoleQuery = _dbSet.AsQueryable().Where(r => r.Deleted == YesNo.NO
                && r.RoleId == roleId);
            return userRoleQuery.Count();
        }
    }
}
