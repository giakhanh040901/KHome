using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.ModifyPersonalPaper
{
    public class NotificationModifyPersonalPaperDto : SendEmailBaseDto
    {
        [JsonPropertyName("data")]
        public ModifyPersonalPaperContent Data { get; set; }
    }

    public class ModifyPersonalPaperContent
    {
        [JsonPropertyName("CustomerName")]
        public string CustomerName { get; set; }

        [JsonPropertyName("UserName")]
        public string UserName { get; set; }
    }
}
