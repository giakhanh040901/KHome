using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.EmailBond
{
    public class BondOrderRenewalsSuccessContent
    {
        [JsonPropertyName("CustomerName")]
        public string CustomerName { get; set; }

        [JsonPropertyName("ContractCode")]
        public string ContractCode { get; set; }

        [JsonPropertyName("PaymentFullDate")]
        public string PaymentFullDate { get; set; }

        /// <summary>
        /// Kỳ hạn
        /// </summary>
        [JsonPropertyName("Tenor")]
        public string Tenor { get; set; }
        [JsonPropertyName("TotalValue")]
        public string TotalValue { get; set; }
        public string BondName { get; set; }
        public string BondCode { get; set; }
        public string PolicyName { get; set; }
        public string Profit { get; set; }
    }
}
