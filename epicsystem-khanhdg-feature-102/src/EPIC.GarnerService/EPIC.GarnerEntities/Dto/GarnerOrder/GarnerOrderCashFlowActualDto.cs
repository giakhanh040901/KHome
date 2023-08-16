using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    /// <summary>
    /// Dòng tiền thực tế
    /// </summary>
    public class GarnerOrderCashFlowActualDto
    {
        public long? Id { get; set; }
        public string PeriodIndexName { get; set; }

        /// <summary>
        /// Số tiền đầu tư
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Rút vốn (Số tiền rút của hợp đồng)
        /// </summary>
        public decimal AmountMoney { get; set; }

        /// <summary>
        /// Số dư còn lại
        /// </summary>
        public decimal Surplus { get; set; }

        /// <summary>
        /// Số tiền tích lũy
        /// </summary>
        public decimal InitTotalValue { get; set; }

        /// <summary>
        /// Số tiền tích lũy hiện hữu
        /// </summary>
        public decimal ExistingAmount { get; set; }

        /// <summary>
        /// Ngày chi trả
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Số ngày đầu tư
        /// </summary>
        public int InvestDays { get; set; }

        /// <summary>
        /// Số tiền thực nhận
        /// </summary>
        public decimal AmountReceived { get; set; }

        /// <summary>
        /// Thuế TN
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// Lợi nhuận
        /// </summary>
        public decimal Profit { get; set; }

        /// <summary>
        /// lợi nhuận khấu trừ
        /// </summary>
        public decimal DeductibleProfit { get; set; }

        /// <summary>
        /// Số ngày
        /// </summary>
        public int NumberOfDays { get; set; }

        /// <summary>
        /// Trạng thái 1 khởi tạo, 2 duyệt, 3 hủy
        /// </summary>
        public int Status { get; set; }
    }
}
