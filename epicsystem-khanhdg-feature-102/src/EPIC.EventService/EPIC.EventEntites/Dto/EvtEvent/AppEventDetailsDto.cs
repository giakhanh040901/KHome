using EPIC.EventEntites.Dto.EvtEventDescriptionMedia;
using EPIC.EventEntites.Dto.EvtTicket;
using EPIC.Utils.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEvent
{
    public class AppEventDetailsDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
       
        /// <summary>
        /// giá vè từ .. (nhỏ nhất )
        /// </summary>
        public decimal MinPrice { get; set; }
        /// <summary>
        /// có phải là sự kiện free không ?
        /// </summary>
        public bool IsFree { get; set; }
        /// <summary>
        /// Cho phép khác hàng xuất vé bản cứng
        /// </summary>
        public bool CanExportTicket { get; set; }
        /// <summary>
        /// Cho phép khách hàng yêu cầu lấy hóa đơn
        /// </summary>
        public bool CanExportRequestRecipt { get; set; }
        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Tên sự kiện
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Địa điểm tổ chức
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Mã Thành phố
        /// </summary>
        public string ProvinvceCode { get; set; }
        /// <summary>
        /// Tên Thành phố
        /// </summary>
        public string ProvinvceName { get; set; }
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
        /// Website sự kiện
        /// </summary>
        public string Website { get; set; }
        /// <summary>
        /// Faecbook sự kiện
        /// </summary>
        public string Facebook { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool? IsHighlight { get; set; }
        /// <summary>
        /// số vé có thể mua
        /// </summary>
        public int? CanBuy { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string OverviewContent { get; set; }
        /// <summary>
        /// Mô tả Markdown hoặc HTML
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// Số người quan tâmm
        /// </summary>
        public int InterestedPeople { get; set; }
        /// <summary>
        /// Chính sách bán vé
        /// </summary>
        public string TicketPurchasePolicy { get; set; }
        /// <summary>
        /// loại file cua chinh sach
        /// </summary>
        public string PolicyFileType { get; set; }

        /// <summary>
        /// avatar của 5 người gần nhất
        /// </summary>
        public IEnumerable<string> Images { get; set; }
        /// <summary>
        /// Loại hình sự kiện
        /// </summary>
        public IEnumerable<int> EventTypes { get; set; }
        
        /// <summary>
        /// Danh sách vé
        /// </summary>
        public IEnumerable<AppEvtTicketDto> Tickets { get; set; }
        public IEnumerable<AppEvtEventDescriptionMediaDto> EvtEventDescriptionMedias { get; set; }
    }

    public class AppEvtTicketDto
    {
        public int Id { get; set; }
        public int EventDetailId { get; set; }
        public int OrderDetailId { get; set; }
        /// <summary>
        /// Tên loại
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Free vé hay không
        /// </summary>
        public bool IsFree { get; set; }
        /// <summary>
        /// Giá vé
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Số lượng vé
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Số lượng mua tối thiểu trong 1 lần
        /// </summary>
        public int MinBuy { get; set; }
        /// <summary>
        /// Số lần mua tối đa trong 1 lần
        /// </summary>
        public int MaxBuy { get; set; }
        /// <summary>
        /// Ngày bán
        /// </summary>
        public DateTime StartSellDate { get; set; }
        /// <summary>
        /// Ngày kết thúc bán
        /// </summary>
        public DateTime EndSellDate { get; set; }
        /// <summary>
        /// Mô tả vé
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Hiển thị vé còn lại trên app
        /// </summary>
        public bool IsShowRemaingTicketApp { get; set; }
        /// <summary>
        /// Số lượng vé còn lại
        /// </summary>
        public int RemainingTickets { get; set; }
        /// <summary>
        /// Danh sách hình ảnh
        /// </summary>
        public IEnumerable<string> UrlImages { get; set; }
       
    }
}