using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ExportExcel
{
    public class InvestmentAccountDto
    {
        public int Stt { get; set; }

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
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

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
        public string PeriodTime { get; set; }

        /// <summary>
        /// Trạng thái của lệnh
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Lợi tức
        /// </summary>
        public string Profit { get; set; }

        /// <summary>
        /// Tổng giá trị đầu tư
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Tổng giá trị đầu tư hợp đồng
        /// </summary>
        public Decimal? PaymentAmnount { get; set; }

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
        /// Gốc đến hạn trả
        /// </summary>
        public decimal LastPeriodPayment { get; set; }

        public decimal InitTotalValue { get; set; }

        /// <summary>
        /// Tổng giá trị = gốc + lãi
        /// </summary>
        public decimal TongGiaTri { get; set; }

        /// <summary>
        /// Thuế thu nhập cá nhân
        /// </summary>
        public decimal IncomeTax { get; set; }
        
        /// <summary>
        /// Số tiền thực nhận =  gốc + lãi + thuế
        /// </summary>
        public decimal ActualReceivedMoney { get; set; }

        /// <summary>
        /// Lãi đến hạn trả
        /// </summary>
        public decimal InterestDue { get; set; }

    }
}
