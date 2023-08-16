using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Event
{
    /// <summary>
    /// loại hành động (nếu có summary)
    /// </summary>
    public static class EvtActionUpdateTypes
    {
        /// <summary>
        /// Tạm dừng sự kiện
        /// </summary>
        public const int PAUSE_EVENT = 1;

        /// <summary>
        /// Hủy sự kiện
        /// </summary>
        public const int CANCEL_EVENT = 2;

        /// <summary>
        /// Cập nhật thời gian gia hạn lệnh
        /// </summary>
        public const int UPDATE_ORDER_EXPIRED_TIME = 3;

        /// <summary>
        /// cap nhat khoa order
        /// </summary>
        public const int UPDATE_ORDER_IS_LOCK = 4;
        /// <summary>
        /// cap nhat khoa order ticket
        /// </summary>
        public const int UPDATE_ORDER_TICKET_STATUS = 5;
    }
}
