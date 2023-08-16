using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Pvcb.Dto.CollectMoney
{
    public class GetThongTinThanhToanDto
    {
        [JsonPropertyName("nameOfBeneficiary")]
        public string NameOfBeneficiary { get; set; }

        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
