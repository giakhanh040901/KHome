using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.Sale
{
    public class SaleBusinessTradingDto
    {
        public int SaleId { get; set; }
        public int BusinessCustomerId { get; set; }
        public int TradingProviderId { get; set; }
    }
}
