using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    /// <summary>
    /// Các hợp đồng còn giao dịch dang dở
    /// </summary>
    public class InvestorUnfinishedTransactionDto
    {
        /// <summary>
        /// Loại nhắc nhở
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Id lệnh để điều hướng
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Ngày tạo hợp đồng
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Số tiền đã thanh toán
        /// </summary>
        public decimal? TotalPayment { get; set; }
    }
}
