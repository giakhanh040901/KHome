using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Entities.Dto.TradingProvider;

namespace EPIC.GarnerEntities.Dto.GarnerProductTradingProvider
{
    public class GarnerProductTradingProviderDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int TradingProviderId { get; set; }
        public string HasTotalInvestmentSub { get; set; }
        public string IsProfitFromPartner { get; set; }
        public decimal? TotalInvestmentSub { get; set; }
        public long? Quantity { get; set; }
        public DateTime DistributionDate { get; set; }
        public TradingProviderDto TradingProvider { get; set; }
    }
}
