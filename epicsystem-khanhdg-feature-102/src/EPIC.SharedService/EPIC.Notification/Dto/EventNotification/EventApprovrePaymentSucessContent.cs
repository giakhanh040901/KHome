using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.EventNotification
{
    public class EventApprovrePaymentSucessContent : EventSuccessContent
    {
        /// <summary>
        /// Số lượng vé
        /// </summary>
        public string TicketQuantity { get; set; }
        /// <summary>
        /// Số tiền
        /// </summary>
        public string TotalMoney { get; set; }
    }
}
