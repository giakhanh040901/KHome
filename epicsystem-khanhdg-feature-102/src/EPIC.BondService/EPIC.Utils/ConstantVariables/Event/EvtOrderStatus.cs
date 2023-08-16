using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Event
{
    /// <summary>
    /// Trạng thái sổ lệnh
    /// </summary>
    public static class EvtOrderStatus
    {
        /// <summary>
        /// Trạng thái khởi tạo
        /// </summary>
        public const int KHOI_TAO = 1;
        /// <summary>
        /// Chờ thanh toán
        /// </summary>
        public const int CHO_THANH_TOAN = 2;
        /// <summary>
        /// Chờ xử lý
        /// </summary>
        public const int CHO_XU_LY = 3;
        /// <summary>
        /// Hợp lệ (Đã thanh toán)
        /// </summary>
        public const int HOP_LE = 4;
        /// <summary>
        /// Đã xóa (Hủy lệnh)
        /// </summary>
        public const int DA_XOA = 5;
    }
}
