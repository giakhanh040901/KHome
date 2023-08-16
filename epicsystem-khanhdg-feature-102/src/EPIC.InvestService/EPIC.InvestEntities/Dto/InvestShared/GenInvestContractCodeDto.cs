using EPIC.InvestEntities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvestShared
{
    public class GenInvestContractCodeDto
    {
        public InvOrder Order { get; set; }
        public DataEntities.Policy Policy { get; set; }
    }
}
