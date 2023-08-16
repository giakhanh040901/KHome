using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.LoyaltyNotification
{
    /// <summary>
    /// Khách nhận voucher thành công từ yêu cầu đổi điểm
    /// </summary>
    public class LoyInvestorReceivedVoucherContent
    {
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// Tên voucher
        /// </summary>
        public string VoucherName { get; set; }
        /// <summary>
        /// Ngày có hiệu lực
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        public string EndDate { get; set; }
    }
}
