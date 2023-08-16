using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.Inquiry
{
    public class RequestInquiryAccountDto
    {
        [JsonPropertyName("mid")]
        public string MId { get; set; }

        [JsonPropertyName("tid")]
        public string TId { get; set; }

        [JsonPropertyName("accountNo")]
        public string AccountNo { get; set; }

        [JsonPropertyName("bankCode")]
        public string BankCode { get; set; }

        [JsonPropertyName("secureHash")]
        public string SecureHash { get; set; }
    }
}
