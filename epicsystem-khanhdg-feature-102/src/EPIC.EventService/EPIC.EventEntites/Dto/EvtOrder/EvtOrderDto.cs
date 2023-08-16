using EPIC.EventEntites.Dto.EvtOrderDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtOrder
{
    public class EvtOrderDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Có bản ghi thanh toán chưa
        /// </summary>
        public bool HavePayment { get; set; }
        /// <summary>
        /// Khung giờ 
        /// </summary>
        public int EventDetailId { get; set; }
        /// <summary>
        /// so tien da thanh toan
        /// </summary>
        public decimal AmountPaid { get; set; }
        
        /// <summary>
        /// dia chi nhan ve
        /// </summary>
        public int? ContractAddressId { get; set; }
        /// <summary>
        /// chi tiet dia chi
        /// </summary>
        public string ContractAddressName { get; set; }
        /// <summary>
        /// loại
        /// </summary>
        public int? Source { get; set; }
        /// <summary>
        /// 1: Quản trị viên đặt; 2: Khách đặt; 3: Sale đặt
        /// </summary>
        public int? OrderSource { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Ngày duyệt
        /// </summary>
        public DateTime? ApproveDate { get; set; }

        /// <summary>
        /// mã yêu cầu
        /// </summary>
        public string ContractCode { get; set; }
        /// <summary>
        /// mã yêu cầu được cấu hình (nếu có)
        /// </summary>
        public string ContractCodeGen { get; set; }
        /// <summary>
        /// tên sự kiện
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// đơn vị tổ chức
        /// </summary>
        public string Organizator { get; set; }
        /// <summary>
        /// số điện thoại investor
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// tên investor
        /// </summary>
        public string Fullname { get; set; }
        /// <summary>
        /// email investor
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// số giấy tờ investor
        /// </summary>
        public string IdNo { get; set; }
        /// <summary>
        /// địa chỉ liên hệ
        /// </summary>
        public string AddressInvestor { get; set; }
        public IEnumerable<int> Types { get; set; }
        /// <summary>
        /// Địa điểm tổ chức
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Thành phố
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// thời gian tổ chức
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// thời gian tổ chức
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Số lượng vé
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Tổng tiền
        /// </summary>
        public decimal TotalMoney { get; set; }
        /// <summary>
        /// Id khách hàng
        /// </summary>
        public int InvestorId { get; set; }
        /// <summary>
        /// Id Giấy tờ khách hàng
        /// </summary>
        public int InvestorIdenId { get; set; }
        /// <summary>
        /// Mã sale
        /// </summary>
        public int? ReferralSaleId { get; set; }
        public int? DepartmentId { get; set; }
        /// <summary>
        /// Thời gian thanh toán còn lại
        /// </summary>
        public DateTime? ExpiredTime { get; set; }
        /// <summary>
        /// Nhận vé bản cứng (Yes/No)
        /// </summary>
        public bool IsReceiveHardTicket { get; set; }
        /// <summary>
        /// Yêu cầu nhận hóa đơn (Yes/No)
        /// </summary>
        public bool IsRequestReceiveRecipt { get; set; }
        public int Status { get; set; }
        public bool IsLock { get; set; }
        /// <summary>
        /// Ngày yêu cầu giao nhận
        /// </summary>
        public DateTime? PendingDate { get; set; }
        /// <summary>
        /// Ngày giao nhận (ngày xử lý)
        /// </summary>
        public DateTime? DeliveryDate { get; set; }
        /// <summary>
        /// nguoi xu ly ve
        /// </summary>
        public string PendingDateModifiedBy { get; set; }
        /// <summary>
        /// nguoi giao ve
        /// </summary>
        public string FinishedDateModifiedBy { get; set; }
        /// <summary>
        /// trang thai yeu cau ve cung
        /// </summary>
        public int? DeliveryStatus { get; set; }
        /// <summary>
        /// Ngày hoàn thành (ngày khách nhận)
        /// </summary>
        public DateTime? FinishedDate { get; set; }

        /// <summary>
        /// Ngày yêu cầu giao hóa đơn
        /// </summary>
        public DateTime? PendingInvoiceDate { get; set; }
        /// <summary>
        /// Ngày giao nhận (ngày xử lý) hóa đơn
        /// </summary>
        public DateTime? DeliveryInvoiceDate { get; set; }
        /// <summary>
        /// nguoi xu ly hoa don
        /// </summary>
        public string PendingInvoiceDateModifiedBy { get; set; }
        /// <summary>
        /// nguoi giao hoa don
        /// </summary>
        public string FinishedInvoiceDateModifiedBy { get; set; }
        /// <summary>
        /// trang thai giao nhan hoa don
        /// </summary>
        public int? DeliveryInvoiceStatus { get; set; }
        /// <summary>
        /// Ngày hoàn thành (ngày khách nhận) hóa đơn
        /// </summary>
        public DateTime? FinishedInvoiceDate { get; set; }
        /// <summary>
        /// Chi tiết lệnh
        /// </summary>
        public IEnumerable<EvtOrderDetailDto> OrderDetails { get; set; }
        public IEnumerable<string> TicketFilledUrls { get; set; }
        public EvtOrderSaleDto SaleInfo { get; set; }
    }

    public class EvtOrderSaleDto
    {
        /// <summary>
        /// Mã giới thiệu
        /// </summary>
        public string ReferralCode { get; set; }
        /// <summary>
        /// Tên sale
        /// </summary>
        public string SaleName { get; set; }
        /// <summary>
        /// Phone sale
        /// </summary>
        public string SalePhone { get; set; }
        /// <summary>
        /// Email sale
        /// </summary>
        public string SaleEmail { get; set; }
      
        /// <summary>
        /// phong giao dich
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// phong giao dich quan ly lenh
        /// </summary>
        public string ManagerDepartmentName { get; set; }

    }

    public class EvtOrderValidDto : EvtOrderDto
    {
    }
}
