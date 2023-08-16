using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.DepositProvider;
using EPIC.Entities.Dto.Issuer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.BondInfo
{
    public class ProductBondInfoDto
    {
        public int ProductBondId { get; set; }

        public int IssuerId { get; set; }

        public int DepositProviderId { get; set; }

        public int BondTypeId { get; set; }

        public string BondCode { get; set; }

        public string BondName { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public DateTime? IssueDate { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal? ParValue { get; set; }

        public long? Quantity { get; set; }

        public decimal? TotalValue { get; set; }

        public int? BondPeriod { get; set; }

        public string BondPeriodUnit { get; set; }

        public decimal? InterestRate { get; set; }

        public int? InterestPeriod { get; set; }

        public string InterestPeriodUnit { get; set; }

        public string InterestType { get; set; }

        public int? InterestRateType { get; set; }

        public string InterestCouponType { get; set; }

        public string CouponBondType { get; set; }

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
        public DepositProviderDto DepositProvider { get; set; }

        public ViewIssuerDto Issuer { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
    }
}

