using EPIC.MSB.Dto.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.PayMoney
{
    public class ResponseInquiryBatchDto : ResponseRequestPayBaseDto
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }
    }

    public class ResponseInquiryBatchDetailDto : ReceiveNotificationPaymentBaseDto
    {
    }
}
