using EPIC.CoreEntities.DataEntities;
using EPIC.DataAccess.Base;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace EPIC.CoreRepositories
{
    public class InvestorRegisterLogEFRepository : BaseEFRepository<InvestorRegisterLog>
    {
        public InvestorRegisterLogEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{InvestorRegisterLog.SEQ}")
        {
        }

        public InvestorRegisterLog Add(InvestorRegisterLog input)
        {
            _logger.LogInformation($"{nameof(InvestorRegisterLogEFRepository)}->{nameof(Add)}: {JsonSerializer.Serialize(input)}");

            input.Id = (int)NextKey();
            return _dbSet.Add(input).Entity;
        }
    }
}
