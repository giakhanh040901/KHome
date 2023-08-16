using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.EmailInvest
{
    /// <summary>
    /// Đến hạn thanh toán 
    /// </summary>
    public class InvestNoticePaymentDueDateContent
    {
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }
        public string InvName { get; set; }

        /// <summary>
        /// Ngày đáo hạn
        /// </summary>
        public string DueDate { get; set; }
    }
}
