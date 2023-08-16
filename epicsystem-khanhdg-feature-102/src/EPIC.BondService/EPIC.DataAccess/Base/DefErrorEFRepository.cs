using EPIC.DataAccess.Base.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.DataAccess.Base
{
    public class DefErrorEFRepository : BaseEFRepository<DefError>
    {
        public DefErrorEFRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
