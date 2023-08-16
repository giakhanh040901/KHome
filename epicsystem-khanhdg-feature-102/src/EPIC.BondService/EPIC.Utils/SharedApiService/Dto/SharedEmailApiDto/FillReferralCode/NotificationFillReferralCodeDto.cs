using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.FillReferralCode
{
    public class NotificationFillReferralCodeDto : SendEmailBaseDto
    {
        [JsonPropertyName("data")]
        public FillReferralCodeContent Data { get; set; }
    }

    public class FillReferralCodeContent
    {
        public string CustomerName { get; set; }
    }
}
