using EPIC.Entities.Dto.TradingProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CompanySharesInfoTradingProvider
{
    public class CpsInfoTradingProviderDto
    {
        public int Id { get; set; }
        public int CpsInfoId { get; set; }
        public int PartnerId { get; set; }
        public int TradingProviderId { get; set; }
        public decimal? TotalInvestmentSub { get; set; }
        public string CreatedBy { get; set; }
        public TradingProviderDto TradingProvider { get; set; }
    }
}
