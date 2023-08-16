using EPIC.MSB.Dto.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.CollectMoney
{
    public class RequestCreateVADto : RequestMsbBaseDto
    {
        [JsonPropertyName("rows")]
        public List<CreateVARowDto> Rows { get; set; }
    }

    public class CreateVARowDto
    {
        [JsonPropertyName("accountNumber")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("referenceNumber")]
        public string ReferenceNumber { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("payType")]
        public string PayType { get; set; }

        [JsonPropertyName("minAmount")]
        public string MinAmount { get; set; }

        [JsonPropertyName("maxAmount")]
        public string MaxAmount { get; set; }

        [JsonPropertyName("equalAmount")]
        public string EqualAmount { get; set; }

        [JsonPropertyName("detail1")]
        public string Detail1 { get; set; }

        [JsonPropertyName("detail2")]
        public string Detail2 { get; set; }

        [JsonPropertyName("detail3")]
        public string Detail3 { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("expiryDate")]
        public string ExpiryDate { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
