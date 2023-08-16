using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.LoyaltyNotification
{
    /// <summary>
    /// Thông báo đã gửi yêu cầu đổi điểm
    /// </summary>
    public class LoySendExchangeRequestAdminContent
    {
        /// <summary>
        /// Tên của khách
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// Số điện thoại của khách
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Tên voucher đổi
        /// </summary>
        public string VoucherName { get; set; }

        /// <summary>
        /// Điểm hiện tại của khách
        /// </summary>
        public string CurrentPoint { get; set; }

        /// <summary>
        /// Tổng điểm yêu cầu đổi
        /// </summary>
        public string Point { get; set; }
        /// <summary>
        /// Ngày tạo yêu cầu
        /// </summary>
        public string CreatedDate { get; set; }
    }
}
