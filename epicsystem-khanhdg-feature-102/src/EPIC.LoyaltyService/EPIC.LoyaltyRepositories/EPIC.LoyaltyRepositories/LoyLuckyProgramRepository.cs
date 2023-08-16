using EPIC.DataAccess.Base;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;

namespace EPIC.LoyaltyRepositories
{
    public class LoyLuckyProgramRepository : BaseEFRepository<LoyLuckyProgram>
    {
        public LoyLuckyProgramRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_LOYALTY}.{LoyLuckyProgram.SEQ}")
        {
        }
    }
}
