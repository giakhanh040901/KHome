using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.CollectMoney
{
    /// <summary>
    /// Response tạo virtual account
    /// </summary>
    public class ResponseDomainCreateVADto
    {
        [JsonPropertyName("moreInfo")]
        public string MoreInfo { get; set; }

        [JsonPropertyName("rows")]
        public List<ResponseCreateVARowDto> Rows { get; set; }
    }

    public class ResponseCreateVARowDto
    {
        [JsonPropertyName("accountNumber")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("serviceCode")]
        public string ServiceCode { get; set; }

        [JsonPropertyName("referenceNumber")]
        public string ReferenceNumber { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("beneficaryName")]
        public string BeneficaryName { get; set; }

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

        [JsonPropertyName("responseCode")]
        public string ResponseCode { get; set; }

        [JsonPropertyName("responseDesc")]
        public string ResponseDesc { get; set; }

        [JsonPropertyName("moreInfo")]
        public string MoreInfo { get; set; }
    }
}
