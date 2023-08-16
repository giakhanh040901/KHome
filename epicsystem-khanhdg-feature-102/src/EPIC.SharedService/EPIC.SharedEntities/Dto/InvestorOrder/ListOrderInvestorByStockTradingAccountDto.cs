using EPIC.BondEntities.Dto.BondOrder;
using EPIC.InvestEntities.Dto.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.SharedEntities.Dto.InvestorOrder
{
    public class ListOrderInvestorByStockTradingAccountDto
    { 
        public List<SCInvestOrderDto> InvestOrder { get; set; }
        public List<SCBondOrderDto> BondOrder { get; set; }
    }
}
