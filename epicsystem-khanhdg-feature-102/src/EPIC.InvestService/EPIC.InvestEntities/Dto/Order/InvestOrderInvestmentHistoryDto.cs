using EPIC.InvestEntities.Dto.Policy;
using System;

namespace EPIC.InvestEntities.Dto.Order
{
    public class InvestOrderInvestmentHistoryDto
    {
        public long Id { get; set; }
        public int? TradingProviderId { get; set; }
        public string CifCode { get; set; }
        public int? DepartmentId { get; set; }
        public int? ProjectId { get; set; }
        public int? DistributionId { get; set; }
        public int? PolicyId { get; set; }
        public int? PolicyDetailId { get; set; }
        public decimal? TotalValue { get; set; }

        /// <summary>
        /// Số tiền đầu tư ban đầu
        /// </summary>
        public decimal InitTotalValue { get; set; }
        public DateTime? BuyDate { get; set; }
        public int? Source { get; set; }
        public string ContractCode { get; set; }
        public DateTime? PaymentFullDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? SettlementDate { get; set; }

        /// <summary>
        /// Ngày đầu tư
        /// </summary>
        public DateTime? InvestDate { get; set; }
        public int? Status { get; set; }
        public string CustomerType { get; set; }

        /// <summary>
        /// Phương thức tất toán cuối kỳ
        /// </summary>
        public int? SettlementMethod { get; set; }
        /// <summary>
        /// Hình thức chi trả lợi tức/ đáo hạn
        /// </summary>
        public int? MethodInterest { get; set; }

        /// <summary>
        /// 1: Quản trị viên đặt; 2: Khách đặt; 3: Sale đặt
        /// </summary>
        public int? OrderSource { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Thông tin kỳ hạn
        /// </summary>
        public ViewPolicyDetailDto PolicyDetail { get; set; }

        /// <summary>
        /// Ngày đáo hạn
        /// </summary>
        public DateTime? DueDate { get; set; }
        /// <summary>
        /// Trạng thái của Lịch sử đầu tư: 1: Tất toán đúng hạn, 2: Tất toán trước hạn, 3: Tái tục gốc, 4: Tái tục gốc và lợi tức
        /// </summary>
        public int? InvestHistoryStatus { get; set; }

        /// <summary>
        /// Tên chính sách
        /// </summary>
        public string PolicyName { get; set; }

        /// <summary>
        /// Mã dự án
        /// </summary>
        public string InvCode { get; set; }

        /// <summary>
        /// Lợi tức của kỳ hạn
        /// </summary>
        public decimal? Profit { get; set; }
    }
}
