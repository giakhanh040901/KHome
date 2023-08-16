using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerExportExcel
{
    public class GarnerListActualPaymentDto
    {
        /// <summary>
        /// Ngày giao dịch
        /// </summary>
        public DateTime? TranDate { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Số giấy tờ
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// Loại giấy tờ
        /// </summary>
        public string IdType { get; set; }

        /// <summary>
        /// Ngày chi
        /// </summary>
        public DateTime? PayDate { get; set; }

        /// <summary>
        /// Địa chỉ thường trú
        /// </summary>
        public string PermanentAddress { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string Cifcode { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Tài khoản nhận tiền KH
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// Tài khoản nhận tiền của khách hàng
        /// </summary>
        public string OwnerBankAccount { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Mã giới thiệu
        /// </summary>
        public string ReferralCode { get; set; }

        /// <summary>
        /// Tên người giới thiệu
        /// </summary>
        public string SalerName { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string SaleDepartmentName { get; set; }

        /// <summary>
        /// Kiểu giao dịch
        /// </summary>
        public string TranType { get; set; }

        /// <summary>
        /// Mã chính sách
        /// </summary>
        public string PolicyCode { get; set; }

        /// <summary>
        /// Loại hình kỳ hạn
        /// </summary>
        public string PeriodType { get; set; }

        /// <summary>
        /// Phong tỏa
        /// </summary>
        public string IsBlockage { get; set; }

        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        public DateTime? InvestDate { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Thời hạn
        /// </summary>
        public string PeriodIndex { get; set; }

        /// <summary>
        /// Tình trạng
        /// </summary>
        public string StatusDisplay { get; set; }

        /// <summary>
        /// Loại chi
        /// </summary>
        public string PaymentType { get; set; }

        /// <summary>
        /// Gross Net
        /// </summary>
        public string CalculateTypeDisplay { get; set; }

        /// <summary>
        /// Số tiền đầu tư của lệnh
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Số tiền chuyển
        /// </summary>
        public string TransferAmount { get; set; }

        /// <summary>
        /// Giá trị đầu tư hiện tại
        /// </summary>
        public decimal CurrentInvestment { get; set; }

        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Thời hạn
        /// </summary>
        public string PeriodTime { get; set; }

        /// <summary>
        /// Số tiền chuyển của giao dịch hiện tại
        /// </summary>
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// Tiền lãi
        /// </summary>
        public decimal InterestAmount { get; set; }

        /// <summary>
        /// Tiền gốc
        /// </summary>
        public decimal PrincipalAmount { get; set; }

        /// <summary>
        /// Giá trị rút vốn tất toán
        /// </summary>
        public decimal WithdrawalAmount { get; set; }

        /// <summary>
        /// Thuế TNCN
        /// </summary>
        public decimal IncomeTax { get; set; }

        /// <summary>
        /// Lợi nhuận khấu trừ
        /// </summary>
        public decimal DeductibleProfit { get; set; }

        /// <summary>
        /// Lợi tức
        /// </summary>
        public decimal Profit { get; set; }

        /// <summary>
        /// kiểu giao dịch : online/offline
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// giá trị đầu tư theo hợp đồng
        /// </summary>
        public decimal InitTotalValue { get; set; }
    }
}
