using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.EventNotification
{
    /// <summary>
    /// Nội dung thông báo chung
    /// </summary>
    public class EventSuccessContent
    {
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// Tên sự kiện
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public string EndDate { get; set; }
    }
}
