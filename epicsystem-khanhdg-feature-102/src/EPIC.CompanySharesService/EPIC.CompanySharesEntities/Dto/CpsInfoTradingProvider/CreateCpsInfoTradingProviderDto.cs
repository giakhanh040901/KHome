using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CompanySharesInfoTradingProvider
{
    public class CreateCpsInfoTradingProviderDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public decimal? TotalInvestmentSub { get; set; }
    }
}
