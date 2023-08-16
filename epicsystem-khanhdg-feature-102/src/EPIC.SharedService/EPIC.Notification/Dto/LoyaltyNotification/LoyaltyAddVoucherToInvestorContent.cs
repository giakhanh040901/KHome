using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.LoyaltyNotification
{
    public class LoyaltyAddVoucherToInvestorContent
    {
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// Loại voucher
        /// </summary>
        public string VoucherType { get; set; }
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

        /// <summary>
        /// Ngày nhận
        /// </summary>
        public string FinishedDate { get; set; }
    }
}
