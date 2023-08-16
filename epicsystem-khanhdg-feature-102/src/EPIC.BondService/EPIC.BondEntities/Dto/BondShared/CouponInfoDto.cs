using System;

namespace EPIC.Entities.Dto.BondShared
{
    /// <summary>
    /// Thông tin trái tức
    /// </summary>
    public class CouponInfoDto
    {
        /// <summary>
        /// Ngày trả
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Kỳ nhận
        /// </summary>
        public int ReceivePeriod { get; set; }

        /// <summary>
        /// Ngày chốt quyền
        /// </summary>
        public DateTime ClosePerDate { get; set; }
        
        /// <summary>
        /// Tỉ lệ trái tức
        /// </summary>
        public decimal CouponRate { get; set; }

        /// <summary>
        /// Thuế
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// trái tức
        /// </summary>
        public decimal Coupon { get; set; }

        /// <summary>
        /// trái tức sau thuế
        /// </summary>
        public decimal ActuallyCoupon { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }
    }
}
