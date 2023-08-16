using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSellDetail
{
    public class CreateRstOpenSellDetailDto
    {
        public int OpenSellId { get; set; }
        public List<int> DistributionProductItemIds { get; set; }
    }
}
