using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.GarnerNotification
{
    public class GarnerOrderDeliverySuccessContentDto
    {
        public string CustomerName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PolicyName { get; set; }
        public string PolicyCode { get; set; }
        public string DeliveryCode { get; set; }
        /// <summary>
        /// ngày đầu tư
        /// </summary>
        public string InvestDate { get; set; }
        /// <summary>
        ///  mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }
        /// <summary>
        /// ngày nhận hợp đồng
        /// </summary>
        public string ReceivedDate { get; set; }
        /// <summary>
        /// số tiền đầu tư ban đầu
        /// </summary>
        public string InitTotalValue { get; set; }
    }
}
