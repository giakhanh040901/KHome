using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.EventNotification
{
    public class EventAdminEventFinishedContent : EventSuccessContent
    {
        /// <summary>
        /// Số lượng vé phát hành
        /// </summary>
        public string TicketQuantity { get; set; }
        /// <summary>
        /// Số lượng vé đăng ký thành công
        /// </summary>
        public string TicketRegisterSuccessQuantity { get; set;}
        /// <summary>
        /// Số lượng vé đã check In
        /// </summary>
        public string TicketCheckInQuantity { get; set;}
    }
}
