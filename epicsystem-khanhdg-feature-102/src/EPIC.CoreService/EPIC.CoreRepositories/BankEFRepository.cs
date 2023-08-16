using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class BankEFRepository : BaseEFRepository<CoreBank>
    {
        public BankEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger)
        {
        }

        public CoreBank FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: id = {id}");
            return _dbSet.FirstOrDefault(b => b.BankId == id);
        }
    }
}
