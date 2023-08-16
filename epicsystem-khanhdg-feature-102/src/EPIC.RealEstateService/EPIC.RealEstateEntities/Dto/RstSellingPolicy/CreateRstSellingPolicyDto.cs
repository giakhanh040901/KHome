using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstSellingPolicy
{
    public class CreateRstSellingPolicyDto
    {
        public List<int> SellingPolicies { get; set; }
        public int OpenSellId { get; set; }
    }
}
