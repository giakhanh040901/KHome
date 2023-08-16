using EPIC.Entities.Dto.Order;
using System;
using System.Collections.Generic;

namespace EPIC.Entities.Dto.BondShared
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
        /// Trái tức (thu nhập từ trái phiếu) (C)
        /// </summary>
        public decimal Coupon { get; set; }
        /// <summary>
        /// Tỉ lệ lợi tức
        /// </summary>
        public decimal InterestRate { get; set; }
        /// <summary>
        /// lợi nhuận, bỏ qua thuế
        /// Tổng số tiền nhận được A* D*B/365
        /// </summary>
        public decimal ActuallyProfit { get; set; }
        /// <summary>
        /// Số tiền kỳ cuối
        /// Số tiền Bên Bán nhận vào Ngày Thanh Toán (K) (K = F - tiền tạm ứng)
        /// </summary>
        public decimal FinalPeriod { get; set; }
        /// <summary>
        /// Số lượng trái phiếu
        /// </summary>
        public long Quantity { get; set; }
        /// <summary>
        /// Đơn giá
        /// </summary>
        public decimal UnitPrice { get; set; }
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
        public List<ProfitAppDto> CashFlow { get; set; }
        /// <summary>
        /// Tổng tiền nhận được không tính thuế (cả gốc)
        /// Tổng số tiền nhận được E (E = A + A * D * B/365)
        /// Tổng thu nhập cuối kỳ
        /// </summary>
        public decimal TotalReceiveValue { get; set; }
        /// <summary>
        /// Tổng Giá Bán (F) (F = E – C)
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// Tiền tạm ứng trong kỳ
        /// </summary>
        public decimal AdvancePayment { get; set; }

        public List<CouponInfoDto> CouponInfos { get; set; }
    }
}
