using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    /// <summary>
    /// Loại hành động cập nhật
    /// </summary>
    public static class RstActionUpdateTypes
    {
        /// <summary>
        /// Phê duyệt trạng thái
        /// </summary>
        public const int APPROVE = 1;
        /// <summary>
        /// Cập nhật trạng thái khóa (PRODUCT_ITEM, OPEN_SELL_DETAIL, )
        /// </summary>
        public const int LOCK = 2;

        /// <summary>
        /// Cập nhật trạng thái khóa  DISTRIBUTION_PRODUCT_ITEM
        /// </summary>
        public const int STATUS_LOCK_STRING = 3;

        /// <summary>
        /// Cập nhật trạng thái dừng  OPEN_SELL
        /// </summary>
        public const int STATUS_STOP = 4;
        /// <summary>
        /// Cập nhật thời gian gia hạn lệnh
        /// </summary>
        public const int UPDATE_ORDER_EXP_TIME_DEPOSIT = 5;
    }
}
