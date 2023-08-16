using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.EmailInvest
{
    public class InvestPaymentSuccessContent
    {
        public string CustomerName { get; set; }
        public string ContractCode { get; set; }
        public string PaymentAmount { get; set; }
        public string TranDate { get; set; }
        public string TranNote { get; set; }
        public string TradingProviderName { get; set; }
        public string InvCode { get; set; }
        public string InvName { get; set; }
    }
}
