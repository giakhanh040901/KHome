using EPIC.InvestEntities.Dto.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.DataEntities
{
    public class InvestmentReport
    {
        public int OrderId { get; set; }
        public int Stt { get; set; }
        public int? InvestorId { get; set; } 
        public int? BusinessCustomerId { get; set; }
        public DateTime? InvestDate { get; set; }
        public DateTime? TranDate { get; set; }
        public int TradingProviderId { get; set; }
        public int? InvestorIdenId { get; set; }
        public int? InvestorBankAccId { get; set; }
        public int? BusinessCustomerBankAccId { get; set; }
        public int? ProjectId { get; set; }
        public int? PeriodQuantity { get; set; }
        public string PeriodType { get; set; }
        public string BusinessCustomerName { get; set; }
        public string InvestorName { get; set; }
        public string CifCode { get; set; }
        public string Sex { get; set; }
        public string ContractCode { get; set; }
        public string BusinessCustomerBankName { get; set; }
        public string InvestorBankName { get; set; }
        public string OwnerBankName { get; set; }
        public Decimal CurrentInvestment { get; set; }
        public string InvCode { get;set; }
        public string BankAccNo { get; set; }
        public string SaleReferralCode { get; set; }
        public string SaleName { get; set; } //Lấy theo giấy tờ mặc định
        public string DepartmentName { get; set; }
        public string Area { get; set; }
        public decimal InitTotalValue { get; set; }
        public int? Source { get; set; }
        public int? SaleOrderId { get; set; }
        public string InvName { get; set; }
        public string PolicyCode { get; set; }
        public int PolicyId { get; set; }
        public int PolicyDetailId { get; set; }
        public Boolean IsBlockage { get; set; }
        public DateTime? PaymentFullDate { get; set; } //Ngày thanh toán đủ
        public DateTime? EndDate { get; set; } //Ngày thanh toán đủ
        public string PeriodTime { get; set; } //Thời hạn
        public int? Status { get; set; } //status của order
        public double Profit { get; set; } //Lấy trong chi tiết chính sách bao nhiêu %
        public decimal TotalValue { get; set; } //giá trị đầu tư hợp đồng
        public Decimal PaymentAmnount { get; set; }
        public int Classify { get; set; }

        /// <summary>
        /// Loại tính toán GROSS hay NET
        /// </summary>
        public int? CalculateType { get; set; }

        /// <summary>
        /// Ngày tạo hợp đồng
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo hợp đồng
        /// </summary>
        public string CreatedBy { get; set; }

        public DateTime? ActiveDate { get; set; }

        /// <summary>
        /// Người duyệt hợp đồng sang trạng thái active
        /// </summary>
        public string ApproveBy { get; set; }

        /// <summary>
        /// Ngày duyệt hợp đồng sang trạng thái active
        /// </summary>
        public DateTime? ApproveDate { get; set; }

        /// <summary>
        /// Ngày gửi yêu cầu
        /// </summary>
        public DateTime? PendingDate { get; set; }

        /// <summary>
        /// Ngày giao hợp đồng
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Ngày nhận hợp đồng
        /// </summary>
        public DateTime? ReceivedDate { get; set; }

        /// <summary>
        /// Ngày hoàn thành hợp đồng
        /// </summary>
        public DateTime? FinishedDate { get; set; }
    }
}
