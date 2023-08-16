using EPIC.DataAccess.Base;
using EPIC.EventEntites.Entites;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventRepositories
{
    public class EvtEventBankAccountEFRepository : BaseEFRepository<EvtEventBankAccount>
    {
        public EvtEventBankAccountEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_EVENT}.{EvtEventBankAccount.SEQ}")
        {
        }
    }
}
