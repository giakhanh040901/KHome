using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    public class ActualExpendReport
    {
        public int OrderId { get; set; }

        //Mã khách hàng
        public string CifCode { get; set; }

        /// <summary>
        /// Mã dự án đầu tư
        /// </summary>
        public string InvCode { get; set; }

        /// <summary>
        /// Tổng giá trị đầu tư
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Giá trị đầu tư đến hiện tại
        /// </summary>
        public Decimal CurrentInvestment { get; set; }

        /// <summary>
        /// Trạng thái của order
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Kiểu chi
        /// </summary>
        public int ExpendType { get; set; }

        /// <summary>
        /// Kiểu loại hình
        /// </summary>
        public int? SettlementMethod { get; set; }

        /// <summary>
        /// Id của khách hàng cá nhân
        /// </summary>
        public int? InvestorId { get; set; }

        /// <summary>
        /// Id của khách hàng doanh nghiệp
        /// </summary>
        public int? BusinessCustomerId { get; set; }

        /// <summary>
        /// Giá trị rút vốn hoặc tất toán 
        /// </summary>
        public Decimal? AmountMoney { get; set; }

        /// <summary>
        /// Số tiền chi 
        /// </summary>
        public Decimal ExpendMoney { get; set; }

        /// <summary>
        /// Giá trị đầu tư ban đầu
        /// </summary>
        public Decimal InitTotalValue { get; set; }

        /// <summary>
        /// Số tiền lãi
        /// </summary>
        public Decimal InterestMoney { get; set; }

        /// <summary>
        /// Phí chuyển nhượng
        /// </summary>
        public Decimal TransferFee { get; set; }

        /// <summary>
        /// Thuế thu nhập cá nhân   
        /// </summary>
        public Decimal IncomeTax { get; set; }

        /// <summary>
        /// Lợi nhuận khấu trừ
        /// </summary>
        public Decimal DeductibleProfit { get; set; }

        /// <summary>
        /// Ngày chi
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Tổng giá trị đầu tư
        /// </summary>
        public Decimal TotalInvestment { get; set; }

        /// <summary>
        /// Id của chính sách
        /// </summary>
        public int PolicyId { get; set; }

        /// <summary>
        /// Id của kỳ hạn
        /// </summary>
        public int PolicyDetailId { get; set; }

        /// <summary>
        /// Ngày rút vốn tất toán
        /// </summary>
        public DateTime WithdrawalDate {get;set;} 

        /// <summary>
        /// Kỳ số bao nhiêu
        /// </summary>
        public int? PeriodIndex { get; set; }

        /// <summary>
        /// Có phải là kỳ cuối không
        /// </summary>
        public string IsLastPeriod { get; set; }

        /// <summary>
        /// Ngày bắt đầu đầu tư
        /// </summary>
        public DateTime? InvestDate { get; set; }

        /// <summary>
        /// Id dự án
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Mã dự án
        /// </summary>
        public string ContractCode { get; set; }

        public DateTime? PaymentFullDate { get; set; }
        
    }
}
