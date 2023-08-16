using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ProjectTradingProvider
{
    public class CreateProjectTradingProviderDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public decimal? TotalInvestmentSub { get; set; }
    }
}
