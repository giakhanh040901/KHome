using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.NotifyOTPEmailTemplate
{
    public class NotificationNotifyOTPEmailTemplateDto : SendEmailBaseDto
    {
        [JsonPropertyName("data")]
        public NotifyOTPEmailTemplateContent Data { get; set; }
    }

    public class NotifyOTPEmailTemplateContent
    {

    }
}
