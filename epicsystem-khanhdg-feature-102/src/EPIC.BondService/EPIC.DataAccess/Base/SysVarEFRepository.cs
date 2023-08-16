using EPIC.Entities.DataEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.DataAccess.Base
{
    public class SysVarEFRepository : BaseEFRepository<SysVar>
    {
        public SysVarEFRepository(DbContext dbContext) : base(dbContext)    
        {
        }

        public SysVar GetVarByName(string grName, string varName)
        {
            var result = EntityNoTracking.FirstOrDefault(sv => sv.GrName == grName && sv.VarName == varName);
            return result;
        }

        public int GetInvValueByName(string grName, string varName)
        {
            var result = GetVarByName(grName, varName);
            return Convert.ToInt32(result?.VarValue);
        }
    }
}
