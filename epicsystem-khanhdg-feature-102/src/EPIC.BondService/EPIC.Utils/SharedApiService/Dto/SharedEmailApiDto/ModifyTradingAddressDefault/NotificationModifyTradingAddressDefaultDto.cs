using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.ModifyTradingAddressDefault
{
    public class NotificationModifyTradingAddressDefaultDto : SendEmailBaseDto
    {
        [JsonPropertyName("data")]
        public ModifyTradingAddressContent Data { get; set; }
    }

    public class ModifyTradingAddressContent
    {
        [JsonPropertyName("CustomerName")]
        public string CustomerName { get; set; }

        [JsonPropertyName("Address")]
        public string Address { get; set; }
        public string DetailAddress { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
    }
}
