using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.Entities.Dto.Order;
using EPIC.InvestSharedEntites.Dto.InvestShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestSharedEntites.Dto.Order
{
    public class AppInvestOrderInvestorDetailDto
    {
        public int Id { get; set; }

        public int? DistributionId { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Mã trái phiếu
        /// </summary>
        public string InvCode { get; set; }

        /// <summary>
        /// Hình ảnh Icon của dự án
        /// </summary>
        public string IconProject { get; set; }
        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public string PolicyName { get; set; }

        /// <summary>
        /// Kiểu chính sách sản phẩm (1: Cố định, 2: Linh hoạt, 3: Giới hạn)
        /// </summary>
        public int? PolicyType { get; set; }

        /// <summary>
        /// Số tiền đầu tư
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Số tiền đầu tư ban đầu
        /// </summary>
        public decimal InitTotalValue { get; set; }
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
        /// Lợi tức thực nhận tính đến ngày đáo hạn
        /// </summary>
        public decimal? AllProfit { get; set; }

        /// <summary>
        /// Chu kỳ nhận lợi tức, Kiểu trả lãi
        /// </summary>
        public int? InterestType { get; set; }

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
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

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
        public int? PolicyDetailId { get; set; }

        /// <summary>
        /// Chính sách
        /// </summary>
        public int? PolicyId { get; set; }

        /// <summary>
        /// Số tiền rút tối thiểu
        /// </summary>
        public decimal? MinWithdrawal { get; set; }
        public AppCashFlowDto AppCashFlow { get; set; }
        public AppPaymentInfoDto PaymentInfo { get; set; }
        public List<AppInvTransactionListDto> TransactionList { get; set; }
        public List<AppTradingBankAccountDto> TradingBankAccounts { get; set; }
    }

    public class AppCashFlowDto
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
        /// Tỉ lệ lợi tức(D)
        /// </summary>
        public decimal InterestRate { get; set; }
        /// <summary>
        /// Số tiền thực nhận, bao gồm lợi nhuận, bỏ qua thuế
        /// Tổng số tiền nhận được E (E = A + A* D*B/365)
        /// </summary>
        public decimal ActuallyProfit { get; set; }
        /// <summary>
        /// Số tiền kỳ cuối
        /// Số tiền Bên Bán nhận vào Ngày Thanh Toán (K) (K=F-tiền tạm ứng)
        /// </summary>
        public decimal FinalPeriod { get; set; }
        /// <summary>
        /// Dòng tiền trộn lẫn cả lợi tức và trái tức
        /// </summary>
        public List<ProfitDto> CashFlow { get; set; }
        /// <summary>
        /// Tổng Giá Bán (F) (F = E – C)
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
