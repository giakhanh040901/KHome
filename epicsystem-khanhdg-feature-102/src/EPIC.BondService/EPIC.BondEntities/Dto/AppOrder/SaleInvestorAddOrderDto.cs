using EPIC.Entities.Dto.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.SaleInvestor
{
    public class SaleInvestorAddOrderDto : CheckOrderAppDto
    {
        public int InvestorId { get; set; }
    }
}
