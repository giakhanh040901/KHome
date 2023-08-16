using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Loyalty
{
    /// <summary>
    /// Status của Voucher - Investor
    /// </summary>
    public static class LoyVoucherInvestorStatus
    {
        /// <summary>
        /// Khởi tạo
        /// </summary>
        public const int KHOI_TAO = 0;
        /// <summary>
        /// Kích hoạt
        /// </summary>
        public const int KICH_HOAT = 1;
        /// <summary>
        /// Hủy kích hoạt
        /// </summary>
        public const int HUY_KICH_HOAT = 2;
        /// <summary>
        /// Đã xóa
        /// </summary>
        public const int DA_XOA = 3;
        /// <summary>
        /// Hết hạn
        /// </summary>
        public const int HET_HAN = 4;
    }
}
