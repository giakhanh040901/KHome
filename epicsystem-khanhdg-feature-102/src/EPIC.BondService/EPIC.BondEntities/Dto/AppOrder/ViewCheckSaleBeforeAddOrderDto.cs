using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.AppOrder
{
    public class ViewCheckSaleBeforeAddOrderDto
    {
        public int SaleId { get; set; }
        public int? InvestorId { get; set; }
        public int? BusinessCustomerId { get; set; }
        public string Fullname { get; set; }
    }
}
