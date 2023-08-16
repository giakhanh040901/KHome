using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.LoyaltyNotification
{
    /// <summary>
    /// Thông báo tích điểm thành công 
    /// </summary>
    public class LoyAccumulatePointSuccessContent
    {
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// Điểm tích lũy
        /// </summary>
        public string Point { get; set; }
        /// <summary>
        /// Ngày kích hoạt
        /// </summary>
        public string ApplyDate { get; set; }
        /// <summary>
        /// Số điểm sau tích lũy
        /// </summary>
        public string TotalPoint { get; set; }
        /// <summary>
        /// Số điểm hiện tại
        /// </summary>
        public string CurrentPoint { get; set; }
    }
}
