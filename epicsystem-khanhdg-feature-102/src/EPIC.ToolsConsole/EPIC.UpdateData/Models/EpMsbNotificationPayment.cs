using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpMsbNotificationPayment
    {
        public long Id { get; set; }
        public string TransId { get; set; }
        public string TransDate { get; set; }
        public string TId { get; set; }
        public string MId { get; set; }
        public string Note { get; set; }
        public string SenderName { get; set; }
        public string ReceiveName { get; set; }
        public string SenderAccount { get; set; }
        public string ReceiveAccount { get; set; }
        public string ReceiveBank { get; set; }
        public string Amount { get; set; }
        public string Fee { get; set; }
        public string SecureHash { get; set; }
        public string NapasTransId { get; set; }
        public string Status { get; set; }
        public string Ip { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? HandleStatus { get; set; }
        public string Exception { get; set; }
        public string Rrn { get; set; }
    }
}
