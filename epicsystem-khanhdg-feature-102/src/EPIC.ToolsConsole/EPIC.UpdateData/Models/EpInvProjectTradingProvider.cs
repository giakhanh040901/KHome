using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvProjectTradingProvider
    {
        public decimal Id { get; set; }
        public decimal ProjectId { get; set; }
        public decimal PartnerId { get; set; }
        public decimal TradingProviderId { get; set; }
        public decimal? TotalInvestmentSub { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
    }
}
