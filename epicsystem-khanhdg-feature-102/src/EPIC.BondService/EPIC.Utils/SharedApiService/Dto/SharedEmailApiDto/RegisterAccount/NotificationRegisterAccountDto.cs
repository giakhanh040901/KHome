using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.RegisterAccount
{
    public class NotificationRegisterAccountDto : SendEmailBaseDto
    {
        [JsonPropertyName("data")]
        public RegisterAccountContent Data { get; set; }
    }

    public class RegisterAccountContent
    {
        [JsonPropertyName("CustomerName")]
        public string CustomerName { get; set; }

        [JsonPropertyName("UserName")]
        public string UserName { get; set; }

        [JsonPropertyName("CifCode")]
        public string CifCode { get; set; }

        [JsonPropertyName("CreatedDate")]
        public string CreatedDate { get; set; }

        [JsonPropertyName("Phone")]
        public string Phone { get; set; }
        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [JsonPropertyName("OtpType")]
        public string OtpType { get; set; }
    }
}
