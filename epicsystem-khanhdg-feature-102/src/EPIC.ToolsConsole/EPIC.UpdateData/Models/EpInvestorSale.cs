using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvestorSale
    {
        public decimal Id { get; set; }
        public decimal? InvestorId { get; set; }
        public decimal? SaleId { get; set; }
        public string ReferralCode { get; set; }
        public string Deleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string IsDefault { get; set; }
    }
}
