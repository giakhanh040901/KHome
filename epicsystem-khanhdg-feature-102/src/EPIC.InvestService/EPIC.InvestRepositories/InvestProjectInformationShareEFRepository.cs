﻿using EPIC.DataAccess.Base;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class InvestProjectInformationShareEFRepository : BaseEFRepository<InvestProjectInformationShare>
    {
        public InvestProjectInformationShareEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{InvestProjectInformationShare.SEQ}")
        {
        }
    }
}
