using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.ApprovePersonalPaper
{
    public class NotificationApprovePersonalPaperDto : SendEmailBaseDto
    {
        [JsonPropertyName("data")]
        public ApprovePersonalPaperContent Data { get; set; }
    }

    public class ApprovePersonalPaperContent
    {
        [JsonPropertyName("CustomerName")]
        public string CustomerName { get; set; }

        [JsonPropertyName("CustomerIdNo")]
        public string CustomerIdNo { get; set; }
    }
}
