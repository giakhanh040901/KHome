using EPIC.CoreEntities.DataEntities;
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
    public class PartnerEFRepository : BaseEFRepository<Partner>
    {
        public PartnerEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, Partner.SEQ)
        {
        }
    }
}
