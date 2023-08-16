using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvestorStockTemp
    {
        public decimal Id { get; set; }
        public decimal? InvestorId { get; set; }
        public decimal? InvestorGroupId { get; set; }
        public decimal? SecurityCompany { get; set; }
        public string StockTradingAccount { get; set; }
        public string ReferId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        public string IsDefault { get; set; }
    }
}
