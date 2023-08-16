using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.GarnerNotification
{
    public class GarnerOrderSuccessContent
    {
        public string CustomerName { get; set; }
        public string TotalValue { get; set; }
        public string ContractCode { get; set; }
        public string TranDate { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PolicyName { get; set; }
        public string PolicyCode { get; set; }
        public string Profit { get; set; }
        public string TradingProviderName { get; set; }
    }
}
