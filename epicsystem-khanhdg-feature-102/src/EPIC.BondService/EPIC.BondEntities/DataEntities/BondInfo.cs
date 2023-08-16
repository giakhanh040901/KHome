using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondInfo : IFullAudited
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public int IssuerId { get; set; }
        public int DepositProviderId { get; set; }
        public int BondTypeId { get; set; }
        public string BondCode { get; set; }
        public string BondName { get; set; }
        public long Quantity { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal ParValue { get; set; }
        public decimal TotalValue { get; set; }
        public int BondPeriod { get; set; }
        [Required]
        public string BondPeriodUnit { get; set; }
        public decimal InterestRate { get; set; }
        public int? InterestPeriod { get; set; }
        public string InterestPeriodUnit { get; set; }
        /// <summary>
        /// Kiểu trả trái tức (1: Định kỳ, 2: Cuối kỳ)
        /// </summary>
        public int InterestRateType { get; set; }

        [NotMapped]
        [Obsolete("bỏ")]
        public string InterestType { get; set; }

        [NotMapped]
        [Obsolete("bỏ")]
        public string InterestCouponType { get; set; }

        [NotMapped]
        [Obsolete("bỏ")]
        public string CouponBondType { get; set; }

        public string IsPaymentGuarantee { get; set; }
        public string AllowSbd { get; set; }
        public int? AllowSbdMonth { get; set; }

        [Required]
        public string IsCollateral { get; set; }
        public int? MaxInvestor { get; set; }
        public int NumberClosePer { get; set; }
        public int CountType { get; set; }
        public string NiemYet { get; set; }
        public decimal FeeRate { get; set; }
        public int Status { get; set; }
        public string PolicyPaymentContent { get; set; }
        public string IsCheck { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
    }
}
