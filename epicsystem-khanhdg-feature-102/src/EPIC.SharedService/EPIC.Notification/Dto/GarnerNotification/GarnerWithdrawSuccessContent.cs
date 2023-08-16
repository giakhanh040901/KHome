using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.GarnerNotification
{
    public class GarnerWithdrawSuccessContent
    {
        public string PolicyName { get; set; }
        public string CustomerName { get; set; }
        public string ContractCode { get; set; }
        public string TradingProviderName { get; set; }
        public string WithdrawAmount { get; set; }
        public string AmountRemain { get; set; }
        public string TranDate { get; set; }
        public string TranNote { get; set; }
    }
}
