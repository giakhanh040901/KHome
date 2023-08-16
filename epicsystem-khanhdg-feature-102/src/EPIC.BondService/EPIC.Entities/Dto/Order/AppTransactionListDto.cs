using System;

namespace EPIC.Entities.Dto.Order
{
    /// <summary>
    /// Giao dịch trong lệnh
    /// </summary>
    public class AppTransactionListDto
    {
        /// <summary>
        /// Ngày giao dịch
        /// </summary>
        public DateTime? TranDate { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Desciption { get; set; }

        /// <summary>
        /// Loại giao dịch
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Số tiền
        /// </summary>
        public decimal? Amount { get; set; }
    }
}
