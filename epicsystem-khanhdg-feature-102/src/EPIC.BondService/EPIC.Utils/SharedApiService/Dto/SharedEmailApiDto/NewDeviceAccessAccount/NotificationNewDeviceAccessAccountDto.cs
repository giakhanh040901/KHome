using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.NewDeviceAccessAccount
{
    public class NotificationNewDeviceAccessAccountDto : SendEmailBaseDto
    {
        [JsonPropertyName("data")]
        public NewDeviceAccessAccountContent Data { get; set; }
    }

    public class NewDeviceAccessAccountContent
    {

    }
}
