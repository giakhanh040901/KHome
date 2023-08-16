using EPIC.DataAccess.Base;
using EPIC.IdentityEntities.DataEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityRepositories
{
    public class WhiteListIpDetailEFRepository : BaseEFRepository<WhiteListIpDetail>
    {
        public WhiteListIpDetailEFRepository(DbContext dbContext) : base(dbContext, "SEQ_WHITE_LIST_IP_DETAIL")
        {
        }
    }
}