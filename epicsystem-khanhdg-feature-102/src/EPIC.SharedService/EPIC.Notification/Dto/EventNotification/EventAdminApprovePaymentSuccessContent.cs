using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.EventNotification
{
    public class EventAdminApprovePaymentSuccessContent : EventSuccessContent
    {
        /// <summary>
        /// Số tiền chuyển
        /// </summary>
        public string PaymentAmount { get; set; }
        /// <summary>
        /// Tổng số tiền đã chuyển
        /// </summary>
        public string TotalPayment { get; set; }
        /// <summary>
        /// Số lượng vé
        /// </summary>
        public string TicketQuantity { get; set; }
        /// <summary>
        /// Mã đặt vé
        /// </summary>
        public string ContractCode { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string Phone { get; set; }
    }
}
