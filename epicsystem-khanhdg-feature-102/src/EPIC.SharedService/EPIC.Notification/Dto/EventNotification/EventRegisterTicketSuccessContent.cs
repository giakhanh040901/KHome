using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.EventNotification
{
    /// <summary>
    /// Nội dung gửi thông báo khi đăng ký vé tham gia sự kiện thành công
    /// </summary>
    public class EventRegisterTicketSuccessContent : EventSuccessContent
    {
        /// <summary>
        /// Số lượng vé
        /// </summary>
        public string TicketQuantity { get; set; }
    }
}
