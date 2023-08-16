using EPIC.Entities.Dto.TradingProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ProjectTradingProvider
{
    public class ProjectTradingProviderDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int PartnerId { get; set; }
        public int TradingProviderId { get; set; }
        public decimal? TotalInvestmentSub { get; set; }
        public string CreatedBy { get; set; }
        public TradingProviderDto TradingProvider { get; set; }
    }
}
