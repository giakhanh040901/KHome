using System;

namespace EPIC.CoreEntities.Dto.ExportData
{
    public class ExportDataInvestOrderDto
    {
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Mã hợp đồng hệ thống
        /// </summary>
        public string ContractCode { get; set; }
        /// <summary>
        /// Mã dự án
        /// </summary>
        public string InvCode { get; set; }
        /// <summary>
        /// Tên dự án
        /// </summary>
        public string InvName { get; set; }

        /// <summary>
        /// Tên chính sách
        /// </summary>
        public string PolicyName { get; set; }

        /// <summary>
        /// Mã chính sách
        /// </summary>
        public string PolicyCode { get; set; }

        /// <summary>
        /// Tên kỳ hạn
        /// </summary>
        public string PolicyDetailName { get; set; }


        /// <summary>
        /// Ngày đặt lệnh
        /// </summary>
        public DateTime? BuyDate { get; set; }

        /// <summary>
        /// Ngày đầu tư
        /// </summary>
        public DateTime? InvestDate { get; set; }

        /// <summary>
        /// Ngày đáo hạn
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Giá trị hợp đồng
        /// </summary>
        public decimal? InitTotalValue { get; set; }

        /// <summary>
        /// Giá trị còn lại
        /// </summary>
        public decimal? TotalValue { get; set; }

        /// <summary>
        /// Mã giới thiệu
        /// </summary>
        public string SaleReferralCode { get; set; }

        /// <summary>
        /// Tên người giới thiệu
        /// </summary>
        public string SaleName { get; set; }

        /// <summary>
        /// Phòng giao dịch quản lý
        /// </summary>
        public string DepartmentManagerSaleName { get; set; }
        /// <summary>
        /// Phòng giao dịch quản lý hợp đồng
        /// </summary>
        public string DepartmentManagerOrderName { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Người duyệt
        /// </summary>
        public string ApproveBy { get; set; }

        /// <summary>
        /// Trạng thái hợp đồng: 1 Khởi tạo, 2 Chờ Thanh toán, 4 Chờ duyệt hợp đồng
        /// 5 Đang đầu tư, 8 Đã tất toán, 9 Đã xóa
        /// </summary>
        public int Status { get; set; }
    }
}
