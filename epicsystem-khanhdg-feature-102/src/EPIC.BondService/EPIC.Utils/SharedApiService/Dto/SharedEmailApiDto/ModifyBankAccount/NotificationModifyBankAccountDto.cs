using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.ModifyBankAccount
{
    public class NotificationModifyBankAccountDto : SendEmailBaseDto
    {
        [JsonPropertyName("data")]
        public ModifyBankAccountContent Data { get; set; }
    }

    public class ModifyBankAccountContent
    {
        public string CustomerName { get; set; }
        public string BankName { get; set; }
        public string BankAccNo { get; set; }
    }
}
