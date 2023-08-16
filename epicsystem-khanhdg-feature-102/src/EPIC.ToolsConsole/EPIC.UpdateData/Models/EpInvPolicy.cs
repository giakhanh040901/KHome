using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvPolicy
    {
        public decimal Id { get; set; }
        public decimal? TradingProviderId { get; set; }
        public decimal? DistributionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal? Type { get; set; }
        public decimal? IncomeTax { get; set; }
        public decimal? MinMoney { get; set; }
        public string Status { get; set; }
        public string IsTransfer { get; set; }
        public decimal? TransferTax { get; set; }
        public decimal? Classify { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public string IsShowApp { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public decimal CalculateType { get; set; }
        public decimal? ExitFee { get; set; }
        public decimal? ExitFeeType { get; set; }
        public decimal? MinWithdraw { get; set; }
        public decimal? PolicyDisplayOrder { get; set; }
    }
}
