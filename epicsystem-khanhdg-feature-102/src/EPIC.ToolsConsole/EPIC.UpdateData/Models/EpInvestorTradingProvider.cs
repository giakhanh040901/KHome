using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvestorTradingProvider
    {
        public decimal Id { get; set; }
        public decimal? InvestorId { get; set; }
        public decimal? TradingProviderId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Deleted { get; set; }
    }
}
