using EPIC.DataAccess.Base;
using EPIC.InvestEntities.DataEntities;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class InvestHistoryUpdateEFRepository : BaseEFRepository<InvestHistoryUpdate>
    {
        public InvestHistoryUpdateEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{InvestHistoryUpdate.SEQ}")
        {
        }
        public void Add(InvestHistoryUpdate input, string username)
        {
            _logger.LogInformation($"{nameof(InvestHistoryUpdateEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}");
            input.Id = (int)NextKey();
            input.CreatedBy = username;
            _dbSet.Add(input);
        }
    }
}
