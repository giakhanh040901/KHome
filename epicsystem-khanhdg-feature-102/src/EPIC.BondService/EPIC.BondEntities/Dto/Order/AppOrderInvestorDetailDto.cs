using EPIC.Entities.Dto.BondShared;
using System;
using System.Collections.Generic;

namespace EPIC.Entities.Dto.Order
{
    public class AppOrderInvestorDetailDto
    {
        public int Id { get; set; }

        public int? SecondaryId { get; set; }

        /// <summary>
        /// Tên của nhà đâu tư
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Mã trái phiếu
        /// </summary>
        public string BondCode { get; set; }

        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public string PolicyName { get; set; }

        /// <summary>
        /// Số tiền đầu tư
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Số lượng trái phiếu mua
        /// </summary>
        public long? Quantity { get; set; }

        /// <summary>
        /// Đơn giá mua
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// Số kỳ đáo hạn
        /// </summary>
        public int? PeriodQuantity { get; set; }

        /// <summary>
        /// Đơn vị đáo hạn (Năm Tháng Ngày)
        /// </summary>
        public string PeriodType { get; set; }

        /// <summary>
        /// Đơn vị kỳ trả lợi tức
        /// </summary>
        public string InterestPeriodType { get; set; }

        /// <summary>
        /// Lợi tức
        /// </summary>
        public decimal? Profit { get; set; }

        /// <summary>
        /// Lợi tức tính đến thời điểm hiện tại
        /// </summary>
        public decimal? ProfitNow { get; set; }

        /// <summary>
        /// Kiểu trả lãi
        /// </summary>
        public int? InterestType { get; set; }
        /// <summary>
        /// Chu kỳ nhận lợi tức
        /// </summary>
        public string InterestPeriod { get; set; }

        /// <summary>
        /// Ngày đầu tư
        /// </summary>
        public DateTime? PaymentFullDate { get; set; }
        /// <summary>
        /// Ngày đáo hạn
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Ngày đầu tư
        /// </summary>
        public DateTime? InvestDate { get; set; }
        /// <summary>
        /// Tổng lợi tức
        /// </summary>
        public decimal? AllActuallyProfit { get; set; }

        /// <summary>
        /// Tổng lợi tức
        /// </summary>
        public decimal? AllProfit { get; set; }

        /// <summary>
        /// Tổng thu nhập cuối kỳ
        /// </summary>
        public decimal? TotalIncome { get; set; }

        /// <summary>
        /// Trạng thái sổ lệnh
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Số tài khoản ngân hàng
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// Chủ tài khoản
        /// </summary>
        public string OwnerAccount { get; set; }

        /// <summary>
        /// Loại giấy tờ
        /// </summary>
        public string IdType { get; set; }

        /// <summary>
        /// Số giấy tờ
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string ContractAddress { get; set; }
        /// <summary>
        /// Mã giới thiệu nhân viên tư vấn
        /// </summary>
        public string SaleReferralCode { get; set; }

        /// <summary>
        /// Tên tư vấn viên
        /// </summary>
        public string SalerName { get; set; }

        /// <summary>
        /// Trạng thái giao nhận hợp đồng
        /// </summary>
        public int? DeliveryStatus { get; set; }

        /// <summary>
        /// Đơn giá hiện tại
        /// </summary>
        public decimal PriceNow { get; set; }

        /// <summary>
        /// Phương thức tất toán cuối kỳ
        /// </summary>
        public int? SettlementMethod { get; set; }

        /// <summary>
        /// Loại kỳ hạn sau khi tái tục
        /// </summary>
        public int? RenewalsPolicyDetailId { get; set; }

        /// <summary>
        /// Kỳ hạn
        /// </summary>
        public int PolicyDetailId { get; set; }

        /// <summary>
        /// Chính sách
        /// </summary>
        public int PolicyId { get; set; }

        public CashFlowDto AppCashFlow { get; set; }
        public AppPaymentInfoDto PaymentInfo { get; set; }
        public List<AppTransactionListDto> TransactionList { get; set; }
    }
}
