using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.PaymentSuccess
{
    public class BondPaymentSuccessContent
    {
        public string CustomerName { get; set; }
        public string ContractCode { get; set; }
        public string PaymentAmount { get; set; }
        public string TradingProviderName { get; set; }
        public string TranDate { get; set; }
        public string TranNote { get; set; }
        public string BondCode { get; set; }
        public string BondName { get; set; }
    }
}
