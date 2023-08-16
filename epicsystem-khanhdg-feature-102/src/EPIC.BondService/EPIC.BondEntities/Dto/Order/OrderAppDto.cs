using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Order
{
    public class OrderAppDto
    {
        public int OrderId { get; set; }
        public int TradingProviderId { get; set; }
        public string ContractCode { get; set; }
        public string BankName { get; set; }
        public string BankAccNo { get; set; }
        public decimal? TotalValue { get; set; }
        public string BankAccName { get; set; }
        public string PaymentNote { get; set; }
    }

    public class AppOrderInvestorDto
    {
        public int OrderId { get; set; }

        /// <summary>
        /// Mã lô trái phiếu
        /// </summary>
        public string BondCode { get; set; }
        /// <summary>
        /// Avatar của tổ chức phát hành
        /// </summary>
        public string IconIssuer { get; set; }

        /// <summary>
        /// Tổng tiền
        /// </summary>
        public decimal? TotalValue { get; set; }

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
    }
}
