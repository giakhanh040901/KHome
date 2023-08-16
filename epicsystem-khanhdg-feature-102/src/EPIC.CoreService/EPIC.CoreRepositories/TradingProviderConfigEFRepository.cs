using EPIC.CoreEntities.DataEntities;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class TradingProviderConfigEFRepository : BaseEFRepository<TradingProviderConfig>
    {
        private readonly EpicSchemaDbContext _epicDbContext;
        public TradingProviderConfigEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{TradingProviderConfig.SEQ}")
        {
            _epicDbContext = dbContext;
        }

        public TradingProviderConfig Add(TradingProviderConfig entity)
        {
            _logger.LogInformation($"{nameof(TradingProviderConfigEFRepository)}->{nameof(Add)}: {JsonSerializer.Serialize(entity)}");
            entity.Id = (int)NextKey();
            entity.CreatedDate = DateTime.Now;
            return _dbSet.Add(entity).Entity;
        }
    }
}
