using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.SendOTP
{
    public class NotificationSendOTPDto : SendEmailBaseDto
    {
        [JsonPropertyName("data")]
        public SendOTPContent Data { get; set; }
    }

    public class SendOTPContent
    {
        [JsonPropertyName("CustomerName")]
        public string CustomerName { get; set; }

        [JsonPropertyName("OTP")]
        public string OTP { get; set; }

        [JsonPropertyName("OtpExpiredTime")]
        public string OtpExpiredTime { get; set; }
    }
}
