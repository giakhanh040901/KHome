using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.SignalR
{
    public static class EvtSignalRMethods
    {
        /// <summary>
        /// Update vé (giờ check inm check out)
        /// </summary>
        public const string UpdateOrderTickeDetail = "UpdateOrderTickeDetail";
        /// <summary>
        /// Update các thông số vé trong màn quản lý sự kiện
        /// </summary>
        public const string UpdateEventTicket = "UpdateEventTicket";
        /// <summary>
        /// Tắt thời gian chờ trên app khi phê duyệt thanh toán trên CMS
        /// </summary>
        public const string UpdateOrderExpiredTime = "UpdateOrderExpiredTime";
        /// <summary>
        /// Trạng thái lệnh khi active hợp đồng hợp lệ
        /// </summary>
        public const string UpdateOrderStatus = "UpdateOrderStatus";
        /// <summary>
        /// Trạng thái giao dịch khi active hợp đồng hợp lệ
        /// </summary>
        public const string UpdateOrderPaymentStatus = "UpdateOrderPaymentStatus";
    }
}
