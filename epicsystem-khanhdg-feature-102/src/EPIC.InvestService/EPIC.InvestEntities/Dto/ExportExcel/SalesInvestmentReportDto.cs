using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ExportExcel
{
    /// <summary>
    /// Báo cáo tổng hợp các khoản đầu tư bán hộ
    /// </summary>
    public class SalesInvestmentReportDto
    {

        /// <summary>
        /// Ngày giao dịch
        /// </summary>
        public DateTime? TranDate { get; set; }

        /// <summary>
        /// Ngày đầu tư
        /// </summary>
        public DateTime? InvestDate { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// Gross Net
        /// </summary>
        public string CalculateType { get; set; }

        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Nguồn đặt lệnh
        /// </summary>
        public string OrderSource { get; set; }

        /// <summary>
        /// Ngân hàng của tài khoản nhận tiền kh
        /// </summary>
        public string CustomerBankName { get; set; }

        /// <summary>
        /// Chủ tài khoản nhận tiền kh
        /// </summary>
        public string OwnerBankName { get; set; }

        /// <summary>
        /// Số tài khoản nhận tiền kh
        /// </summary>
        public string BankAccNo { get; set; }

        /// <summary>
        /// Mã giới thiệu của sale
        /// </summary>
        public string SaleReferralCode { get; set; }

        /// <summary>
        /// Mã giới thiệu sale bán hộ
        /// </summary>
        public string SaleReferralCodeSub { get; set; }

        /// <summary>
        /// Tên của người giới thiệu
        /// </summary>
        public string SaleName { get; set; }

        /// <summary>
        /// Tên của phòng ban
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Tên vùng
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Loại giao dịch
        /// </summary>
        public string TradingType { get; set; }

        /// <summary>
        /// Số giấy tờ có thể là CCCD, CMND, Hộ chiếu
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// Loại giấy tờ
        /// </summary>
        public string IdType { get; set; }

        /// <summary>
        /// Mã dự án
        /// </summary>
        public string InvCode { get; set; }

        /// <summary>
        /// Tên dự án
        /// </summary>
        public string InvName { get; set; }

        /// <summary>
        /// Mã chính sách
        /// </summary>
        public string PolicyCode { get; set; }

        /// <summary>
        /// Có phong toả hay không
        /// </summary>
        public string IsBlockage { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Thời gian của một kì
        /// </summary>
        public string PeriodTime { get; set; } //Thời hạn

        /// <summary>
        /// Trạng thái của order
        /// </summary>
        public int? Status { get; set; } //status của order

        /// <summary>
        /// Lợi tức
        /// </summary>
        public string Profit { get; set; } //Lấy trong chi tiết chính sách bao nhiêu %

        /// <summary>
        /// Tổng giá trị đầu tư
        /// </summary>
        public string InitTotalValue { get; set; } //giá trị đầu tư hợp đồng

        /// <summary>
        /// Tổng giá trị đầu tư hợp đồng
        /// </summary>
        public Decimal? PaymentAmnount { get; set; } //giá trị đầu tư hợp đồng

        /// <summary>
        /// Tổng giá trị đầu tư hiện tại
        /// </summary>
        public Decimal? CurrentInvestment { get; set; }

        /// <summary>
        /// Loại khách hàng
        /// </summary>
        public string CustomerType { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Địa chỉ thường trú
        /// </summary>
        public string PlaceOfResident { get; set; }

        /// <summary>
        /// Thời gian
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Ngày duyệt hợp đồng sang trạng thái active
        /// </summary>
        public DateTime? ApproveDate { get; set; }

        /// <summary>
        /// Người duyệt hợp đồng sang trạng thái active
        /// </summary>
        public string ApproveBy { get; set; }

        /// <summary>
        /// Ngày xử lý hợp đồng
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

        /// <summary>
        /// Số lượng ngày đầu tư
        /// </summary>
        public int? InvestDayNumber { get; set; }
        
        /// <summary>
        /// Lãi suất
        /// </summary>
        public string Interest { get; set; }

        /// <summary>
        /// Giá trị rút vốn, tất toán
        /// </summary>
        public decimal SettlementWithdrawalValue { get; set; }

        /// <summary>
        /// Số tiền đã chi
        /// </summary>
        public decimal AmountSpentValue { get; set; }

        /// <summary>
        /// Tiền lãi
        /// </summary>
        public decimal InterestMoney { get; set; }

        /// <summary>
        /// Phí rút vốn tất toán
        /// </summary>
        public decimal SettlementWithdrawalFee { get; set; }

        /// <summary>
        /// Phí chuyển nhượng
        /// </summary>
        public decimal TransferFee { get; set; }

        /// <summary>
        /// Thuế thu nhập cá nhân
        /// </summary>
        public decimal IncomeTax { get; set; }

        /// <summary>
        /// Lợi nhuận khấu trừ
        /// </summary>
        public decimal DeductibleProfit { get; set; }

        /// <summary>
        /// Số kh thực nhận
        /// </summary>
        public decimal ActualReceivedCustomerNumber { get; set; }
    }
}
