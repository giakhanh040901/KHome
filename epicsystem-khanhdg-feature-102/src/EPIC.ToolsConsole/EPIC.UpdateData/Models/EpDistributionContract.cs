using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpDistributionContract
    {
        public decimal DistributionContractId { get; set; }
        public decimal? PartnerId { get; set; }
        public decimal? TradingProviderId { get; set; }
        public decimal? BondPrimaryId { get; set; }
        public string ContractCode { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? TotalValue { get; set; }
        public DateTime? DateBuy { get; set; }
        public decimal? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public DateTime? DateContract { get; set; }
    }
}
