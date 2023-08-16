using EPIC.DataAccess.Base;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityEntities.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityRepositories
{
    public class PartnerPermissionEFRepository : BaseEFRepository<PartnerPermission>
    {
        public PartnerPermissionEFRepository(EpicSchemaDbContext dbContext) : base(dbContext, "SEQ_P_PARTNER_PERMISSION")
        {
        }
    }
}
