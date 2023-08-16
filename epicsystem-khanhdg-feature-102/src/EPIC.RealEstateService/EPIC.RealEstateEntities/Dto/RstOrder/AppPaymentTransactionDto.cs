using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrder
{
    /// <summary>
    /// Tiến độ giao dịch của hợp đồng
    /// </summary>
    public class AppPaymentTransactionDto
    {
        /// <summary>
        /// Số tiền
        /// </summary>
        public decimal AmountMoney { get; set; }

        /// <summary>
        /// Trạng thái giao dịch: 1: Chờ giao dịch: 2: Giao dịch thành công
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 1: Đã cọc, 2... :Thanh toán lần thứ 2,...
        /// </summary>
        public int TransactionIndex { get; set; }

        /// <summary>
        /// Thời gian giao dịch thành công
        /// </summary>
        public DateTime? TransactionDate { get; set; }
    }
}
