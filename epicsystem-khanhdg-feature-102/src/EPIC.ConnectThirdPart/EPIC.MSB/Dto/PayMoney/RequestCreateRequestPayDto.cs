using EPIC.MSB.Dto.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.PayMoney
{
    /// <summary>
    /// Tạo yêu cầu chi theo lô
    /// </summary>
    public class RequestCreateRequestPayDto
    {
        [JsonPropertyName("requestId")]
        public string RequestId { get; set; }

        [JsonPropertyName("mid")]
        public string MId { get; set; }

        [JsonPropertyName("tid")]
        public string TId { get; set; }

        [JsonPropertyName("secureHash")]
        public string SecureHash { get; set; }

        [JsonPropertyName("data")]
        public List<RequestPayItem> Data { get; set; }
    }

    /// <summary>
    /// Các lệnh chi cụ thể trong lô
    /// </summary>
    public class RequestPayItem
    {
        [JsonPropertyName("transId")]
        public string TransId { get; set; }

        [JsonPropertyName("transDate")]
        public string TransDate { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; }

        [JsonPropertyName("receiveAccount")]
        public string ReceiveAccount { get; set; }

        [JsonPropertyName("receiveName")]
        public string ReceiveName { get; set; }

        [JsonPropertyName("receiveBank")]
        public string ReceiveBank { get; set; }

        [JsonPropertyName("amount")]
        public string Amount { get; set; }
    }
}
