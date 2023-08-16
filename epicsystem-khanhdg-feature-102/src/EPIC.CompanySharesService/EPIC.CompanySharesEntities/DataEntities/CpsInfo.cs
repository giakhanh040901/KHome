using EPIC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.DataEntities
{
    public class CpsInfo : IFullAudited
    {
        public int Id { get; set; }
        public int? IssuerId { get; set; }
        public string CpsCode { get; set; }
        public string CpsName { get; set; }
        public string Content { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? ParValue { get; set; }
        public int? Period { get; set; }
        public string PeriodUnit { get; set; }
        public decimal? InterestRate { get; set; }
        public int? InterestPeriod { get; set; }
        public string InterestPeriodUnit { get; set; }
        public int? InterestRateType { get; set; }
        public long? Quantity { get; set; }
        public string IsPaymentGuarantee { get; set; }
        public string IsAllowSbd { get; set; }
        public int? AllowSbdDay { get; set; }
        public string IsCollateral { get; set; }
        public int? MaxInvestor { get; set; }
        public int? NumberClosePer { get; set; }
        public string GuranteeOrganization { get; set; }
        public decimal? FeeRate { get; set; }
        public string IsListing { get; set; }
        public int? Status { get; set; }
        public string Deleted { get; set; }
        public int? PartnerId { get; set; }
        public int CountType { get; set; }
        public string IsCheck { get; set; }
        public string IsCreated { get; set; }
        public string PolicyPaymentContent { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public int? TotalInvestment { get; set; }
        public string HasTotalInvestmentSub { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
