using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.EmailInvest
{
    public class InvestOrderSuccessDelivery
    {
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }
        public string TradingProviderName { get; set; }
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
        ///  tên sản phẩm đầu tư
        /// </summary>
        public string InvName { get; set; }
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
