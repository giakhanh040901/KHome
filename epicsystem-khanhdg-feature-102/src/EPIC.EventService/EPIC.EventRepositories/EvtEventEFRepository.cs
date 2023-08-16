using EPIC.DataAccess.Base;
using EPIC.EventEntites.Entites;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using EPIC.RealEstateEntities.DataEntities;

namespace EPIC.EventRepositories
{
    public class EvtEventEFRepository : BaseEFRepository<EvtEvent>
    {
        public EvtEventEFRepository(DbContext dbContext, ILogger logger, string seqName = null) : base(dbContext, logger, $"{DbSchemas.EPIC_EVENT}.{EvtEvent.SEQ}")
        {
        }
    }
}
