using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.DataEntities
{
    public class GarnerExcelSaleInvestment
    {
        public int? TradingProviderId { get; set; }
        public long? OrderId { get; set; }
        /// <summary>
        /// Ngày giao dịch
        /// </summary>
        public DateTime? TranDate { get; set; }

        /// <summary>
        /// Nguồn đặt lệnh
        /// </summary>
        public int Source { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Id của business customer
        /// </summary>
        public int? BusinessCustomerId { get; set; }

        /// <summary>
        /// Id của investor
        /// </summary>
        public int? InvestorId { get; set; }

        /// <summary>
        /// Id ngân hàng của investor
        /// </summary>
        public int? InvestorBankAccId { get; set; }

        /// <summary>
        /// Id ngân hàng của BusinessCustomer
        /// </summary>
        public int? BusinessCustomerBankAccId { get; set; }

        /// <summary>
        /// Mã giới thiệu của sale
        /// </summary>
        public string SaleReferralCode { get; set; }

        /// <summary>
        /// Kiểu giao dịch
        /// </summary>
        public int? TranType { get; set; }

        public int? TranClassify { get; set; }
        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Mã chính sách
        /// </summary>
        public string PolicyCode { get; set; }

        /// <summary>
        /// Trạng thái của lệnh
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Ngày đầu tư
        /// </summary>
        public DateTime? InvestDate { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Loại tính toán
        /// </summary>
        public int? CalculateType { get; set; }

        /// <summary>
        /// Giá trị đầu tư theo hợp đồng
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Loại kỳ hạn
        /// </summary>
        public string PeriodType { get; set; }

        /// <summary>
        /// Số lượng kì hạn
        /// </summary>
        public int? PeriodQuantity { get; set; }

        /// <summary>
        /// Số tiền khách hàng đã chuyển
        /// </summary>
        public decimal CurrentInvestment { get; set; }

        /// <summary>
        /// Số tiền khách hàng giao dịch
        /// </summary>
        public decimal PaymentAmount { get; set; }
    }
}
