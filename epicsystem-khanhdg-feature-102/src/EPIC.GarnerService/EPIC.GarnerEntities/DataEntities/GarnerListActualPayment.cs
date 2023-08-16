using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.DataEntities
{
    public class GarnerListActualPayment
    {

        /// <summary>
        /// Ngày giao dịch
        /// </summary>
        public DateTime? TranDate { get; set; }
        
        /// <summary>
        /// Id ngân hàng của tài khoản cá nhân
        /// </summary>
        public int? InvestorBankAccId { get; set; }

        /// <summary>
        /// Id ngân hàng của tài khoản doanh nghiệp
        /// </summary>
        public int? BusinessCustomerBankAccId { get; set; }

        /// <summary>
        /// Mã lệnh
        /// </summary>
        public long OrderId { get; set; }
        public int? TradingProviderId { get; set; }

        /// <summary>
        /// Ngày chi trả
        /// </summary>
        public DateTime? Paydate { get; set; }

        /// <summary>
        /// Loại chi
        /// </summary>
        public int? InterestPaymentType { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }
        public int? BusinessCustomerId { get; set; }
        
        public int? InvestorId { get; set; }
        /// <summary>
        /// Mã chính sách
        /// </summary>
        public string PolicyCode { get; set; }

        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Tổng giá trị đầu tư
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Nguồn đặt lệnh
        /// </summary>
        public int? Source { get; set; }

        /// <summary>
        /// Tổng giá trị đầu tư hiện tại
        /// </summary>
        public decimal CurrentTotalInvestment { get; set; }

        /// <summary>
        /// Loại chi
        /// </summary>
        public int? PaymentType { get; set; }
        
        /// <summary>
        /// Giá trị rút vốn tất toán
        /// </summary>
        public decimal WithdrawalAmount { get; set; }
        /// <summary>
        /// Tiền gốc
        /// </summary>
        public decimal PrincipalMoney { get; set; }
        /// <summary>
        /// Giá trị đầu tư đến hiện tại
        /// </summary>
        public Decimal CurrentInvestment { get; set; }

        /// <summary>
        /// Kiểu chi
        /// </summary>
        public int ExpendType { get; set; }

        /// <summary>
        /// Giá trị rút vốn hoặc tất toán 
        /// </summary>
        public Decimal? AmountMoney { get; set; }

        /// <summary>
        /// Số tiền chi 
        /// </summary>
        public Decimal ExpendMoney { get; set; }

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
        public DateTime WithdrawalDate { get; set; }

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
        /// Ngày thanh toán đủ
        /// </summary>
        public DateTime? PaymentFullDate { get; set; }
        /// <summary>
        /// Tiền lãi
        /// </summary>
        public decimal InterestAmount { get; set; }

        /// <summary>
        /// Tiền gốc
        /// </summary>
        public decimal PrincipalAmount { get; set; }
    }
}
