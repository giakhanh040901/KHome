using EPIC.DataAccess.Base;
using EPIC.IdentityEntities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityRepositories
{
    public class RolePermissionEFRepository : BaseEFRepository<RolePermission>
    {
        public RolePermissionEFRepository(EpicSchemaDbContext dbContext) : base(dbContext, "SEQ_P_ROLE_PERMISSION")
        {
        }
    }
}
