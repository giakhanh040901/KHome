using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.EventNotification
{
    /// <summary>
    /// Nội dung thông báo nhận vé bản cứng và thông báo yêu cầu nhận hóa đơn
    /// </summary>
    public class EventAdminNotifyReceiveHardTicketSuccessContent : EventSuccessContent
    {
        /// <summary>
        /// Sđt khách hàng
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Mã yêu cầu
        /// </summary>
        public string ContractCode { get; set; }
        /// <summary>
        /// Số lượng vé
        /// </summary>
        public string TicketQuantity { get; set; }
        /// <summary>
        /// Ngày yêu cầu
        /// </summary>
        public string CreatedDate { get; set; }
    }
}
