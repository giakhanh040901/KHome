using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.GarnerNotification
{
    public class GarnerInterestPaymentSuccessContentDto
    {
        /// <summary>
        /// Tên chính sách
        /// </summary>
        public string PolicyName { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Tên đại lý
        /// </summary>
        public string TradingProviderName { get; set; }

        /// <summary>
        /// Lợi nhuận
        /// </summary>
        public string AmountMoney { get; set; }

        /// <summary>
        /// Ngày chi trả/ Ngày giao dịch 
        /// </summary>
        public string PayDate { get; set; }
    }
}
