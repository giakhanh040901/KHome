using EPIC.DataAccess.Base;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace EPIC.LoyaltyRepositories
{
    public class LoyConversionPointStatusLogEFRepository : BaseEFRepository<LoyConversionPointStatusLog>
    {
        public LoyConversionPointStatusLogEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_LOYALTY}.{LoyConversionPointStatusLog.SEQ}")
        {
        }

        /// <summary>
        /// Ghi log trạng thái
        /// </summary>
        public LoyConversionPointStatusLog Add(LoyConversionPointStatusLog input)
        {
            _logger.LogInformation($"{nameof(LoyConversionPointStatusLogEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");

            input.Id = (int)NextKey();
            input.CreatedDate = DateTime.Now;
            input.Deleted = YesNo.NO;
            _dbSet.Add(input);
            return input;
        }
    }
}
