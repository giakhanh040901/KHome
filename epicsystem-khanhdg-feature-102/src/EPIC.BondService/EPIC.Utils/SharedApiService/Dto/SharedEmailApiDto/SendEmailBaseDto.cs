using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto
{
    public class SendEmailBaseDto
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("receiver")]
        public Receiver Receiver { get; set; }
    }

    public class SendEmailDto<T>
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("receiver")]
        public Receiver Receiver { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }

        [JsonPropertyName("attachments")]
        public List<string> Attachments { get; set; }
    }

    public class Receiver
    {
        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("email")]
        public EmailNotifi Email { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("fcm_tokens")]
        public List<string> FcmTokens { get; set; }
    }

    public class EmailNotifi
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }
    }

    public class ParamsChooseTemplate
    {
        public int? TradingProviderId { get; set; }
    }
}
