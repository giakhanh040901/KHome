using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerShared
{
    public class CashFlowDto
    {
        /// <summary>
        /// Số tiền đầu tư
        /// </summary>
        public decimal TotalValue { get; set; }
        /// <summary>
        /// Số ngày đầu tư
        /// </summary>
        public int NumberOfDays { get; set; }
        /// <summary>
        /// Tỉ lệ lợi tức
        /// </summary>
        public decimal InterestRate { get; set; }
        /// <summary>
        /// lợi nhuận thực nhận trừ đi thuế
        /// Tổng số tiền nhận được A * D * B/365 - thuế
        /// </summary>
        public decimal ActuallyProfit { get; set; }
        /// <summary>
        /// Lợi nhuận trước thuế A * D * B/365
        /// </summary>
        public decimal BeforeTaxProfit { get; set; }
        /// <summary>
        /// Số tiền kỳ cuối
        /// Số tiền Bên Bán nhận vào Ngày Thanh Toán (K) (K = F - tiền tạm ứng)
        /// </summary>
        public decimal FinalPeriod { get; set; }
        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Ngày đáo hạn
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Dòng tiền trộn lẫn cả lợi tức và trái tức
        /// </summary>
        public List<ProfitDto> CashFlow { get; set; }
        /// <summary>
        /// Tổng tiền nhận được cả gốc trừ đi thuế
        /// Tổng số tiền nhận được E (E = A + A * D * B/365)
        /// Tổng thu nhập cuối kỳ
        /// </summary>
        public decimal TotalReceiveValue { get; set; }
        /// <summary>
        /// Tổng Giá Bán (F) (F = E – C)
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// Thuế lợi tức cả kỳ
        /// </summary>
        public decimal TaxProfit { get; set; }

        /// <summary>
        /// Thuế thu nhập cá nhân
        /// </summary>
        public decimal? Tax { get; set; }
    }
}
