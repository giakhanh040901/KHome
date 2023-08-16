using System;

namespace EPIC.InvestEntities.DataEntities
{
    /// <summary>
    /// Báo cáo các khoản đầu tư bán hộ
    /// </summary>
    public class SaleInvestmentExcel
    {
        public int OrderId { get; set; }
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Ngày giao dịch
        /// </summary>
        public DateTime TranDate { get; set; }

        /// <summary>
        /// Mã giấy tờ của sale
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// Loại giấy tờ
        /// </summary>
        public string IdType { get; set; }

        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Mã giới thiệu
        /// </summary>
        public string SaleRefferalCode { get; set; }

        /// <summary>
        /// Ngày đầu tư
        /// </summary>
        public DateTime? InvestDate { get; set; }

        /// <summary>
        /// Id đại lý sơ cấp
        /// </summary>
        public int TradingProviderId { get; set; }

        /// <summary>
        /// Id mã dự án
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// Số lượng kì hạn
        /// </summary>
        public int? PeriodQuantity { get; set; }

        /// <summary>
        /// Loại kì hạn
        /// </summary>
        public string PeriodType { get; set; }

        /// <summary>
        /// Tên khách hàng doanh nghiệp
        /// </summary>
        public string BusinessCustomerName { get; set; }

        /// <summary>
        /// Tên của khách hàng cá nhân
        /// </summary>
        public string InvestorName { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// Tên ngân hàng của khách hàng doanh nghiệp
        /// </summary>
        public string BusinessCustomerBankName { get; set; }

        /// <summary>
        /// Tên ngân hàng của khách hàng cá nhân
        /// </summary>
        public string InvestorBankName { get; set; }

        /// <summary>
        /// Tên chủ tài khoản
        /// </summary>
        public string OwnerBankName { get; set; }

        /// <summary>
        /// Tổng số tiền đầu tư hiện tại
        /// </summary>
        public Decimal CurrentInvestment { get; set; }

        /// <summary>
        /// Mã dự án
        /// </summary>
        public string InvCode { get; set; }

        /// <summary>
        /// Số tài khoản ngân hàng
        /// </summary>
        public string BankAccNo { get; set; }
        public string SaleReferralCode { get; set; }

        /// <summary>
        /// Mã giới thiệu cho sale bán hộ
        /// </summary>
        public string SaleReferralCodeSub { get; set; }
        /// <summary>
        /// Id phòng ban bán hộ
        /// </summary>
        public int? DepartmentIdSub { get; set; }
        public int DepartmentId { get; set; }

        /// <summary>
        /// Tên của saler
        /// </summary>
        public string SaleName { get; set; } //Lấy theo giấy tờ mặc định

        /// <summary>
        /// Tên của phòng ban
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Vùng
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Nguồn tạo của khách hàng
        /// </summary>
        public int? Source { get; set; }

        /// <summary>
        /// Id sale đặt lệnh cho khách
        /// </summary>
        public int? SaleOrderId { get; set; }

        /// <summary>
        /// Tên dự án
        /// </summary>
        public string InvName { get; set; }

        /// <summary>
        /// Mã chính sách
        /// </summary>
        public string PolicyCode { get; set; }
        public int PolicyId { get; set; }
        public int PolicyDetailId { get; set; }
        public Boolean IsBlockage { get; set; }
        public DateTime? PaymentFullDate { get; set; } //Ngày thanh toán đủ
        public DateTime? EndDate { get; set; } //Ngày thanh toán đủ
        public string PeriodTime { get; set; } //Thời hạn
        public int? Status { get; set; } //status của order
        public double Profit { get; set; } //Lấy trong chi tiết chính sách bao nhiêu %
        public string InitTotalValue { get; set; } //giá trị đầu tư hợp đồng
        public Decimal PaymentAmnount { get; set; }
        public int Classify { get; set; }

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
