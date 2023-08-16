using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.LoyaltyNotification
{
    /// <summary>
    /// Thông báo tạo yêu cầu đổi ưu đãi thành công
    /// </summary>
    public class LoySendRequestConversionPointContent
    {
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// Tên voucher (Tên hiển thị)
        /// </summary>
        public string VoucherName { get; set; }
    }
}
