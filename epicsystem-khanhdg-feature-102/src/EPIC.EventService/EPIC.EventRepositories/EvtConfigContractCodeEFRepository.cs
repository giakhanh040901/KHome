using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.EventEntites.Dto.EvtConfigContractCode;
using EPIC.EventEntites.Entites;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.EventRepositories
{
    public class EvtConfigContractCodeEFRepository : BaseEFRepository<EvtConfigContractCode>
    {
        public EvtConfigContractCodeEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_EVENT}.{EvtConfigContractCode.SEQ}")
        {
        }
    }
}
