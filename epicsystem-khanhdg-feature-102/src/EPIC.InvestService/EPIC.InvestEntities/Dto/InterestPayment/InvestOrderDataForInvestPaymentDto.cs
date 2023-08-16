using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InterestPayment
{
    /// <summary>
    /// Lấy thông tin hợp đồng cho tính toán dự chi
    /// </summary>
    public class InvestOrderDataForInvestPaymentDto
    {
        public long Id { get; set; }
        public int? TradingProviderId { get; set; }
        public string CifCode { get; set; }
        public int? ProjectId { get; set; }
        public int? DistributionId { get; set; }
        public int? PolicyId { get; set; }

        /// <summary>
        /// Id Kỳ hạn
        /// </summary>
        public int? PolicyDetailId { get; set; }
        public decimal? TotalValue { get; set; }

        /// <summary>
        /// Số tiền đầu tư ban đầu
        /// </summary>
        public decimal InitTotalValue { get; set; }
        public string ContractCode { get; set; }

        /// <summary>
        /// Ngày đầu tư
        /// </summary>
        public DateTime? InvestDate { get; set; }

        /// <summary>
        /// Ngày đáo hạn
        /// </summary>
        public DateTime? DueDate { get; set; }
        public int? InvestorIdenId { get; set; }
        public int? Status { get; set; }

        /// <summary>
        /// Phương thức tất toán cuối kỳ
        /// </summary>
        public int? SettlementMethod { get; set; }

        /// <summary>
        /// Loại kỳ hạn sau khi tái tục
        /// </summary>
        public int? RenewalsPolicyDetailId { get; set; }
  
        public string CustomerName { get; set; }
    }
}
