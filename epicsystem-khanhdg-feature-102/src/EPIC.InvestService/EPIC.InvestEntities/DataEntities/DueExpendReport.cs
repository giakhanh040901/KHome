using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.DataEntities
{
    public class DueExpendReport
    {

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Mã dự án
        /// </summary>
        public string InvCode { get; set; }

        /// <summary>
        /// Thuế thu nhập cá nhân
        /// </summary>
        public Decimal IncomeTax { get; set; }

        /// <summary>
        /// Id dự án
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Mã dự án
        /// </summary>
        public string ContractCode { get; set; }

        public int InvestorIdenId { get; set; }

        /// <summary>
        /// Tổng các khoản đầu tư
        /// </summary>
        public Decimal TotalInvestment { get; set; }

        /// <summary>
        /// tổng giá trị của bảng order
        /// </summary>
        public Decimal TotalValue { get; set; }

        /// <summary>
        /// chỉ số kì hạn
        /// </summary>
        public int PeriodIndex { get; set; }
        public int OrderId { get; set; }

        /// <summary>
        /// Ngày trả tiền của kì
        /// </summary>
        public DateTime? PayDate { get; set; }
        public int PolicyDetailId { get; set; }
        public int PolicyId { get; set; }
        public string IsLastPeriod { get; set; }
        public Decimal AmountMoney { get; set; }
        public int Status { get; set; }
        public string ProjectBankAccNo { get; set; }
        public int MaxPeriodIndex { get; set; }
        public int? TradingProviderId { get; set; }
        public int? BusinessCustomerId { get; set; }
        public DateTime? InvestDate { get; set; }
        public int PeriodQuantity { get; set; }
        public string PeriodType { get; set; }
        public decimal Profit { get; set; }
        public int TradingBankAccId { get; set; }
        public int InvestorBankAccId { get; set; }
        public int BusinessCustomerBankAccId { get; set; }
    }
}
