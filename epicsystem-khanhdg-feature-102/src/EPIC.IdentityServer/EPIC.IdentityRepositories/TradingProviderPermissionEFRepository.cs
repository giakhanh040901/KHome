using EPIC.DataAccess.Base;
using EPIC.IdentityEntities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityRepositories
{
    public class TradingProviderPermissionEFRepository : BaseEFRepository<TradingProviderPermission>
    {
        public TradingProviderPermissionEFRepository(EpicSchemaDbContext dbContext) : base(dbContext, "SEQ_P_TRADING_PERMISSION")
        {
        }
    }
}
