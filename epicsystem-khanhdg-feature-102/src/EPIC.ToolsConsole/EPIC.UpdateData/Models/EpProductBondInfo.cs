using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpProductBondInfo
    {
        public decimal ProductBondId { get; set; }
        public decimal? IssuerId { get; set; }
        public decimal? DepositProviderId { get; set; }
        public string BondCode { get; set; }
        public string BondName { get; set; }
        public string Content { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? ParValue { get; set; }
        public decimal? BondPeriod { get; set; }
        public string BondPeriodUnit { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? InterestPeriod { get; set; }
        public string InterestPeriodUnit { get; set; }
        public string InterestType { get; set; }
        public decimal? InterestRateType { get; set; }
        public string InterestCouponType { get; set; }
        public decimal? Quantity { get; set; }
        public string IsPaymentGuarantee { get; set; }
        public string AllowSbd { get; set; }
        public decimal? AllowSbdMonth { get; set; }
        public string IsCollateral { get; set; }
        public decimal? MaxInvestor { get; set; }
        public decimal? NumberClosePer { get; set; }
        public string GuaranteeOrganization { get; set; }
        public decimal? FeeRate { get; set; }
        public string NiemYet { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        public decimal? PartnerId { get; set; }
        public string CountType { get; set; }
        public string IsCheck { get; set; }
        public string IsCreated { get; set; }
        public string PolicyPaymentContent { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
