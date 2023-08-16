using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstGenContractCode
{
    public class RstGenContractCodeDefaultDto
    {
        public DataEntities.RstOrder Order { get; set; }
        public DataEntities.RstProject Project { get; set; }
        public DataEntities.RstProductItem ProductItem { get; set; }
        public DataEntities.RstDistributionPolicy DistributionPolicy { get; set; }
    }
}
