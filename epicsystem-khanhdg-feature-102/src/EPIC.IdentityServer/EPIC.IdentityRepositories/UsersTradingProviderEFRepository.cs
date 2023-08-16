using EPIC.DataAccess.Base;
using EPIC.IdentityEntities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityRepositories
{
    public class UsersTradingProviderEFRepository : BaseEFRepository<UsersTradingProvider>
    {
        public UsersTradingProviderEFRepository(EpicSchemaDbContext dbContext) : base(dbContext, "SEQ_USERS_TRADING")
        {
        }
    }
}
