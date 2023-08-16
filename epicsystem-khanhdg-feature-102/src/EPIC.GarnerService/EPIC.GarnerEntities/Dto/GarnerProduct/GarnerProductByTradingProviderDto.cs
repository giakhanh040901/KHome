using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerProduct
{
    public class GarnerProductByTradingProviderDto : GarnerProductDto
    {
        public decimal? TotalInvestmentSub { get; set; }
        public string IsProfitFromPartner { get; set; }
    }
}
