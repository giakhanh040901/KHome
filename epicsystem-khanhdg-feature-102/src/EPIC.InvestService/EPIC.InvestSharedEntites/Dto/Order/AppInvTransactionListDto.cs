using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestSharedEntites.Dto.Order
{
    public class AppInvTransactionListDto
    {
        /// <summary>
        /// Ngày giao dịch
        /// </summary>
        public DateTime? TranDate { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Loại giao dịch
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Số tiền
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }
    }
}
