using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.Notification
{
    /// <summary>
    /// Base notify chi hộ
    /// </summary>
    public class ReceiveNotificationPaymentBaseDto
    {
        [JsonPropertyName("transId")]
        public string TransId { get; set; }

        [JsonPropertyName("transDate")]
        public string TransDate { get; set; }

        [JsonPropertyName("tId")]
        public string TId { get; set; }

        [JsonPropertyName("mId")]
        public string MId { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; }

        [JsonPropertyName("senderName")]
        public string SenderName { get; set; }

        [JsonPropertyName("receiveName")]
        public string ReceiveName { get; set; }

        [JsonPropertyName("senderAccount")]
        public string SenderAccount { get; set; }

        [JsonPropertyName("receiveAccount")]
        public string ReceiveAccount { get; set; }

        [JsonPropertyName("receiveBank")]
        public string ReceiveBank { get; set; }

        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("fee")]
        public string Fee { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("napasTransId")]
        public string NapasTransId { get; set; }

        [JsonPropertyName("rrn")]
        public string Rrn { get; set; }
    }
}
