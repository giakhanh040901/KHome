using EPIC.DataAccess.Base;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstProductItemMaterialFileEFRepository : BaseEFRepository<RstProductItemMaterialFile>
    {
        public RstProductItemMaterialFileEFRepository(DbContext dbContext, ILogger logger, string seqName = null) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProductItemMaterialFile.SEQ}")
        {
        }
    }
}
