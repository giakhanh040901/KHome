using EPIC.DataAccess.Base;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;

namespace EPIC.LoyaltyRepositories
{
    public class LoyLuckyScenarioEFRepository : BaseEFRepository<LoyLuckyScenario>
    {
        public LoyLuckyScenarioEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_LOYALTY}.{LoyLuckyScenario.SEQ}")
        {
        }
    }
}
