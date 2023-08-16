using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Notification.ResetPassword
{
    public class NotificationResetPasswordDto : SendEmailBaseDto
    {     
        [JsonPropertyName("data")]
        public ResetPasswordContentDto Data { get; set; }
    }

    public class ResetPasswordContentDto
    {
        [JsonPropertyName("CustomerName")]
        public string CustomerName { get; set; }

        [JsonPropertyName("PASSWORD")]
        public string Password { get; set; }
    }

}
