using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ExportReport
{
    public class BondInvestmentDto
    {
        public int Stt { get; set; }
        public DateTime? TranDate { get; set; }
        public string CustomerName { get; set; }
        public long OrderId { get; set; }
        public string CifCode { get; set; }
        public string Sex { get; set; }

        /// <summary>
        /// Mã giấy tờ
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// Loại giấy tờ
        /// </summary>
        public string IdType { get; set; }
        public int Classify { get; set; }
        public string ClassifyDisplay { get; set; }
        public string ContractCode { get; set; }
        public int TradingProviderId { get; set; }
        public string BankAccNo { get; set; }
        public string OwnerAccount { get; set; }
        public string CustomerBankName { get; set; }
        public string SaleReferralCode { get; set; }
        public string SaleName { get; set; }  //Lấy theo giấy tờ mặc định
        public string DepartmentName { get; set; }
        public string Area { get; set; }
        public string TradingType { get; set; }
        public decimal InterestRate { get; set; }
        public int InterestPeriod { get; set; }
        public string InterestPeriodUnit { get; set; }
        public string KyHan { get; set; }
        public int TranType { get; set; }
        public string TranTypeDisplay { get; set; }
        public string InvName { get; set; }
        public string BondCode { get; set; }
        public DateTime? PaymentFullDate { get; set; } //Ngày thanh toán đủ
        public DateTime? DueDate { get; set; } //Ngày thanh toán đủ
        public string PeriodTime { get; set; } //Thời hạn
        public int? StatusOrder { get; set; } //status của order
        public string StatusDisplay { get; set; }
        public int Quantity { get; set; }
        public int Source { get; set; }
        public string PeriodType { get; set; }
        public int PeriodQuantity { get; set; }
        public int BondSecondaryId { get; set; }
        public string Profit { get; set; } //Lấy trong chi tiết chính sách bao nhiêu %
        public decimal TotalValue { get; set; } //giá trị đầu tư hợp đồng
        public decimal UnitPrice { get; set; } // Đơn giá trên hợp đồng
        public Decimal BondValue { get; set; } //giá trị trái phiếu
        public Decimal InvestmentValueRemain { get; set; } //giá trị đầu tư còn lại
        public string CustomerType { get; set; }
        public decimal PaymentAmount { get;set; }

    }   

    public class SaleInfoExportExcel
    {
        public string Name { get; set; }
        public string DepartmentName { get; set; }
    }
}
