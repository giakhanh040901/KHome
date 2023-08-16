using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.CreateCustomerInformation
{
    public class NotificationCreateCustomerInformationDto : SendEmailBaseDto
    {
        [JsonPropertyName("data")]
        public CreateCustomerInformationContent Data { get; set; }
    }

    public class CreateCustomerInformationContent
    {

    }
}
