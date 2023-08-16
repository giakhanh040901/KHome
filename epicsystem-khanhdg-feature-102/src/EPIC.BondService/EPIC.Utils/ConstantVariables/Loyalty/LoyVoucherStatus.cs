using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Loyalty
{
    public static class LoyVoucherStatus
    {
        /// <summary>
        /// Khởi tạo
        /// </summary>
        public const int KHOI_TAO = 1;
        /// <summary>
        /// Kích hoạt
        /// </summary>
        public const int KICH_HOAT = 2;
        /// <summary>
        /// Hủy kích hoạt
        /// </summary>
        public const int HUY_KICH_HOAT = 3;
        /// <summary>
        /// Đã xóa
        /// </summary>
        public const int DA_XOA = 4;
        /// <summary>
        /// Hết hạn
        /// </summary>
        public const int HET_HAN = 5;
    }
}
