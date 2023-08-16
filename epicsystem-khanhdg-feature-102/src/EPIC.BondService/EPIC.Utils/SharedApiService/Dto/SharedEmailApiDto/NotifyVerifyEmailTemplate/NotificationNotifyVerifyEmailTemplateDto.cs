using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.NotifyVerifyEmailTemplate
{
    public class NotificationNotifyVerifyEmailTemplateDto : SendEmailBaseDto
    {
        [JsonPropertyName("data")]
        public NotifyVerifyEmailTemplateContent Data { get; set; }
    }

    public class NotifyVerifyEmailTemplateContent
    {
        [JsonPropertyName("CustomerName")]
        public string CustomerName { get; set; }

        [JsonPropertyName("LinkVerify")]
        public string LinkVerify { get; set; }

        [JsonPropertyName("EmailExpiredTime")]
        public string EmailExpiredTime { get; set; }
    }
}
