using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CompanySharesShared
{
    /// <summary>
    /// Thông tin cổ tức
    /// </summary>
    public class DividendInfoDto
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
        /// Tỉ lệ cổ tức
        /// </summary>
        public decimal CouponRate { get; set; }

        /// <summary>
        /// Thuế
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// cổ tức
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
