using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.ResetPin
{
    public class NotificationResetPinDto : SendEmailBaseDto
    {
        [JsonPropertyName("data")]
        public ResetPinContentDto Data { get; set; }
    }

    public class ResetPinContentDto
    {
        [JsonPropertyName("CustomerName")]
        public string CustomerName { get; set; }

        [JsonPropertyName("PIN")]
        public string PinCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
