using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CpsInfo
{
    public class SCInfoDto
    {
        public string CpsCode { get; set; }

        public string CpsName { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public DateTime? IssueDate { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal? ParValue { get; set; }

        public long? Quantity { get; set; }

        public decimal? TotalValue { get; set; }

        public int? Period { get; set; }

        public string PeriodUnit { get; set; }

        public decimal? InterestRate { get; set; }

        public int? InterestPeriod { get; set; }

        public string InterestPeriodUnit { get; set; }

        public int? InterestRateType { get; set; }

        public string DividendType { get; set; }

        public string IsPaymentGuarantee { get; set; }

        public string IsAllowSbd { get; set; }

        public int? AllowSbdDay { get; set; }

        public int? Status { get; set; }
        public string PolicyPaymentContent { get; set; }

        public int? NumberClosePer { get; set; }

        public string IsCollateral { get; set; }

        public int? MaxInvestor { get; set; }

        public int? CountType { get; set; }

        public string IsListing { get; set; }
        public string IsCheck { get; set; }
        public string IsCreated { get; set; }
        public string Icon { get; set; }
    }
}
