using EPIC.DataAccess.Base;
using EPIC.GarnerEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerRepositories
{
    public class GarnerWithdrawalDetailEFRepository : BaseEFRepository<GarnerWithdrawalDetail>
    {
        public GarnerWithdrawalDetailEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerWithdrawalDetail.SEQ}")
        {
        }

        public GarnerWithdrawalDetail Add(GarnerWithdrawalDetail input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (long)NextKey();
            return _dbSet.Add(input).Entity;
        }

        public List<GarnerWithdrawalDetail> FindByWithdrawalId(long withdrawalId)
        {
            return _dbSet.Where(d => d.WithdrawalId == withdrawalId).ToList();
        }
    }
}
