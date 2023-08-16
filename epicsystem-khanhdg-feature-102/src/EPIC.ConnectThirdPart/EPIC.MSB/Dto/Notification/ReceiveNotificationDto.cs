using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.Notification
{
    public class ReceiveNotificationDto
    {
        [JsonPropertyName("tranSeq")]
        public string TranSeq { get; set; }

        [JsonPropertyName("vaCode")]
        public string VaCode { get; set; }

        [JsonPropertyName("vaNumber")]
        public string VaNumber { get; set; }

        [JsonPropertyName("fromAccountName")]
        public string FromAccountName { get; set; }

        [JsonPropertyName("fromAccountNumber")]
        public string FromAccountNumber { get; set; }

        [JsonPropertyName("toAccountName")]
        public string ToAccountName { get; set; }

        [JsonPropertyName("toAccountNumber")]
        public string ToAccountNumber { get; set; }

        [JsonPropertyName("tranAmount")]
        public string TranAmount { get; set; }

        [JsonPropertyName("tranRemark")]
        public string TranRemark { get; set; }

        [JsonPropertyName("tranDate")]
        public string TranDate { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }
}
