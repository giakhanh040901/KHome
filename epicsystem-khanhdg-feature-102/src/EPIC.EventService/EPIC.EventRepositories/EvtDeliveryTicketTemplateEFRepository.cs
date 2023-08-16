using EPIC.DataAccess.Base;
using EPIC.EventEntites.Entites;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventRepositories
{
    public class EvtDeliveryTicketTemplateEFRepository : BaseEFRepository<EvtDeliveryTicketTemplate>
    {
        public EvtDeliveryTicketTemplateEFRepository(DbContext dbContext, ILogger logger, string seqName = null) : base(dbContext, logger, $"{DbSchemas.EPIC_EVENT}.{EvtDeliveryTicketTemplate.SEQ}")
        {
        }
    }
}
