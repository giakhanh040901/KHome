using EPIC.CompanySharesEntities.Dto.Issuer;
using EPIC.Entities.Dto.BusinessCustomer;
using System;

namespace EPIC.CompanySharesEntities.Dto.CpsInfo
{
    public class CpsInfoDto
    {
        public int Id { get; set; }

        public int IssuerId { get; set; }

        public int DepositProviderId { get; set; }

        public int CpsTypeId { get; set; }

        public string CpsCode { get; set; }

        public string CpsName { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public DateTime? IssueDate { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal? ParValue { get; set; }

        public long? Quantity { get; set; }

        public decimal? TotalValue { get; set; }

        public int? CpsPeriod { get; set; }

        public string CpsPeriodUnit { get; set; }

        public decimal? InterestRate { get; set; }

        public int? InterestPeriod { get; set; }

        public string InterestPeriodUnit { get; set; }

        public string InterestType { get; set; }

        public int? InterestRateType { get; set; }

        public string InterestCouponType { get; set; }

        public string CouponCpsType { get; set; }

        public string IsPaymentGuarantee { get; set; }

        public string AllowSbd { get; set; }

        public int? AllowSbdMonth { get; set; }

        public int? Status { get; set; }
        public string PolicyPaymentContent { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string TradingProviderName { get; set; }

        public int? NumberClosePer { get; set; }

        public string IsCollateral { get; set; }

        public int? MaxInvestor { get; set; }

        public int? CountType { get; set; }

        public string NiemYet { get; set; }
        public string IsCheck { get; set; }
        public string IsCreated { get; set; }
        public long? SoLuongConLai { get; set; }
        public string Icon { get; set; }
        //public DepositProviderDto DepositProvider { get; set; }

        public ViewCpsIssuerDto Issuer { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
    }
}
