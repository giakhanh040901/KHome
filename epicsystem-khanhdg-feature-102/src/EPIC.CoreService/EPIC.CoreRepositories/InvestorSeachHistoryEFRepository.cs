using EPIC.CoreEntities.DataEntities;
using EPIC.DataAccess.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class InvestorSeachHistoryEFRepository : BaseEFRepository<InvestorSearchHistory>
    {
        public InvestorSeachHistoryEFRepository(DbContext dbContext, ILogger logger, string seqName = null) : base(dbContext, logger, InvestorSearchHistory.SEQ)
        {
        }
    }
}
