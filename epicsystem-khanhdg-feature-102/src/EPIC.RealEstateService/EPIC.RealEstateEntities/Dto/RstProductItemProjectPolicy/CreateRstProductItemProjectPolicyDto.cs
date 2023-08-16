using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItemProjectPolicy
{
    public class CreateRstProductItemProjectPolicyDto
    {
        public List<int> ProductItemProjectPolicies { get; set; }
        public int ProductItemId { get; set; }
    }
}
