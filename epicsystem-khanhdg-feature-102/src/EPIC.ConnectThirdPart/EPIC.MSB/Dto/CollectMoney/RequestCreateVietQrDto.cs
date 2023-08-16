using EPIC.MSB.Dto.Shared;
using System.Text.Json.Serialization;

namespace EPIC.MSB.Dto.CollectMoney
{
    public class RequestCreateVietQrDto : RequestMsbIdDto
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("account")]
        public string Account { get; set; }

        [JsonPropertyName("accountName")]
        public string AccountName { get; set; }

        [JsonPropertyName("remark")]
        public string Remark { get; set; }

        [JsonPropertyName("secureHash")]
        public string SecureHash { get; set; }
    }
}
