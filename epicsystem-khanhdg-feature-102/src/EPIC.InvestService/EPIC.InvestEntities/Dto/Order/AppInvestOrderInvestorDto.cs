using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Order
{
    public class AppInvestOrderInvestorDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Mã dự án
        /// </summary>
        public string InvCode { get; set; }
        /// <summary>
        /// Avatar lấy từ dự án
        /// </summary>
        public string IconProject { get; set; }

        /// <summary>
        /// Tổng tiền
        /// </summary>
        public decimal? TotalValue { get; set; }

        /// <summary>
        /// Tổng tiền
        /// </summary>
        public decimal? InitTotalValue { get; set; }

        /// <summary>
        /// Lợi tức
        /// </summary>
        public decimal? Profit { get; set; }

        /// <summary>
        /// Lãi suất
        /// </summary>
        public decimal? Interest { get; set; }

        /// <summary>
        /// lợi tức Là Năm - tháng - ngày
        /// </summary>
        public string InterestPeriodType { get; set; }

        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string PolicyName { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Thời gian còn lại
        /// </summary>
        public int? TimeRemaining { get; set; }

        /// <summary>
        /// Thời gian thanh toán
        /// </summary>
        public DateTime? PaymentFullDate { get; set; }

        /// <summary>
        /// Có tất toán trước hạn hay không?
        /// </summary>
        public bool IsPrepayment { get; set; }
    }
}
