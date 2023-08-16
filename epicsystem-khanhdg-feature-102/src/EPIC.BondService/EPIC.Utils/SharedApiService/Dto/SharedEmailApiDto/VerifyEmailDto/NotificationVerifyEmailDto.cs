using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.VerifyEmailDto
{
    public class NotificationVerifyEmailDto : SendEmailBaseDto
    {
        [JsonPropertyName("data")]
        public VerifyEmailContent Data { get; set; }
    }

    public class VerifyEmailContent 
    {
        [JsonPropertyName("CustomerName")]
        public string CustomerName { get; set; }

        [JsonPropertyName("Email")]
        public string Email { get; set; }
    }
}
