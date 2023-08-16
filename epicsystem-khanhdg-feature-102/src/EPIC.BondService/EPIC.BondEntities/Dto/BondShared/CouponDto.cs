using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.BondShared
{
    public class CouponDto
    {
        public int ProductBondId { get; set; }
        public DateTime IssueDate { get; set; }
        public decimal? ParValue { get; set; }
        public long? Quantity { get; set; }
        public int? BondPeriod { get; set; }
        public string BondPeriodUnit { get; set; }
        public decimal? InterestRate { get; set; }
        public string InterestPeriodUnit { get; set; }
        public int? InterestPeriod { get; set; }
        public int? InterestRateType { get; set; }
        public List<CouponRecurentDto> CouponRecurents { get; set; }
    }

    public class CouponRecurentDto
    {
        /// <summary>
        /// Ngày trả
        /// </summary>
        public DateTime? PayDate { get; set; }
        public decimal? Coupon { get; set; }
        /// <summary>
        /// Số ngày
        /// </summary>
        public int NumberOfDays { get; set; }
        /// <summary>
        /// Ngày chốt quyền
        /// </summary>
        public DateTime? ClosePerDate { get; set; }
    }
}
