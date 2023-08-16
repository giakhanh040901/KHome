using EPIC.CoreEntities.DataEntities;
using EPIC.DataAccess.Base;
using Microsoft.Extensions.Logging;

namespace EPIC.CoreRepositories
{
    public class CallCenterConfigEFRepository : BaseEFRepository<CallCenterConfig>
    {
        public CallCenterConfigEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, CallCenterConfig.SEQ)
        {
        }
    }
}
