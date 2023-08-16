using EPIC.Entities.DataEntities;
using EPIC.EventEntites.Dto.EvtAdminEvent;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEvent
{
    public class EvtEventDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }

        /// <summary>
        /// Tên sự kiện
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ban tổ chức
        /// </summary>
        public string Organizator { get; set; }
        /// <summary>
        /// Loại hình sự kiện
        /// </summary>
        public IEnumerable<int> EventTypes { get; set; }
        /// <summary>
        /// Địa điểm tổ chức
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Mã Thành phố
        /// </summary>
        public string ProvinceCode { get; set; }
        /// <summary>
        /// Tên thành phố
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Vĩ độ
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// Kinh độ
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// Đối tượng xem
        /// </summary>
        public int Viewing { get; set; }
        /// <summary>
        /// Cấu trúc mã hợp đồng
        /// </summary>
        public int ConfigContractCodeId { get; set; }
        /// <summary>
        /// Tài khoản nhận tiền
        /// </summary>
        public int BackAccountId { get; set; }
        /// <summary>
        /// Website sự kiện
        /// </summary>
        public string Website { get; set; }
        /// <summary>
        /// Faecbook sự kiện
        /// </summary>
        public string Facebook { get; set; }
        /// <summary>
        /// SĐT
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Có show App hay không
        /// </summary>
        public string IsShowApp { get; set; }
        /// <summary>
        /// Check
        /// </summary>
        public string IsCheck { get; set; }
        /// <summary>
        /// Sự kiện  nổi bật
        /// </summary>
        public bool? IsHighlight { get; set; }
        /// <summary>
        /// Cho phép khác hàng xuất vé bản cứng
        /// </summary>
        public bool? CanExportTicket { get; set; }
        /// <summary>
        /// Cho phép khách hàng yêu cầu lấy hóa đơn
        /// </summary>
        public bool? CanExportRequestRecipt { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Loại mô tả
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// Nội dung mô tả
        /// </summary>
        public string OverviewContent { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Người cài
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Ngày cài
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Số vé
        /// </summary>
        public int TicketQuantity { get; set; }
        /// <summary>
        /// Đăng ký
        /// </summary>
        public int RegisterQuantity { get; set; }
        /// <summary>
        /// Hợp lệ
        /// </summary>
        public int ValidQuantity { get; set; }
        /// <summary>
        /// Tham gia
        /// </summary>
        public int ParticipateQuantity { get; set; }
        /// <summary>
        /// File chính sách mua vé
        /// </summary>
        public string TicketPurchasePolicy { get; set; }
        /// <summary>
        /// Số lượng vé còn lại
        /// </summary>
        public int RemainingTickets { get; set; }
        /// <summary>
        /// Ngày kết thúc sự kiện (nếu EndDate nhỏ hiện tại thì không hiện sự kiện)
        /// </summary>
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public IEnumerable<int> TradingBankAccounts { get; set; }
        public IEnumerable<EvtAdminEventDto> TicketInspector { get; set; }
    }

    public class ViewEventByTradingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsSalePartnership { get; set; }
    }
}
