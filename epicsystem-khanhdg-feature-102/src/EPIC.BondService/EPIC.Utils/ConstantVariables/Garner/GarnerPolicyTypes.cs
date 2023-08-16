using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Garner
{
    /// <summary>
    /// Loại chính sách tích luỹ
    /// </summary>
    public static class GarnerPolicyTypes
    {
        /// <summary>
        /// Dạng không chọn kỳ hạn (tích luỹ theo thời gian)
        /// </summary>
        public const int KHONG_CHON_KY_HAN = 1;
        /// <summary>
        /// Dạng không chọn kỳ hạn (tích luỹ theo số tiền)
        /// </summary>
        public const int TICH_LUY_THEO_SO_TIEN = 2;
        /// <summary>
        /// Dạng không chọn kỳ hạn (tích luỹ theo cả thời gian và số tiền)
        /// </summary>
        public const int TICH_LUY_THEO_THOI_GIAN_VA_SO_TIEN = 3;
        /// <summary>
        /// Dạng chọn kỳ hạn
        /// </summary>
        public const int CHON_KY_HAN = 4;
    }
}
