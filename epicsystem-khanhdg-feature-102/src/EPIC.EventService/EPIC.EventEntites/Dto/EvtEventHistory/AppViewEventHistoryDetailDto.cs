using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtEventDescriptionMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventHistory
{
    public class AppViewEventHistoryDetailDto
    {
        public int OrderId { get; set; }
        public int InvestorId { get; set; }
        /// <summary>
        /// Chính sách bán vé
        /// </summary>
        public string TicketPurchasePolicy { get; set; }
        /// <summary>
        /// loại file cua chinh sach
        /// </summary>
        public string PolicyFileType { get; set; }
        /// <summary>
        /// Tên sự kiện
        /// </summary>
        public string Name { get; set; }
        public string ContractCode { get; set; }
        public string ContractCodeGen { get; set; }
        public int Status { get; set; }
        /// <summary>
        /// tong ve cua lenh
        /// </summary>
        public int TotalTicket { get; set; }
        /// <summary>
        /// ngay dat lenh
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Ngày check in 
        /// </summary>
        public DateTime? CheckIn { get; set; } 
        /// <summary>
        /// Ngày check out
        /// </summary>
        public DateTime? CheckOut { get; set; }
        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        public DateTime? StartDate { get; set;}
        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Ngày hết hạn thanh toán vé
        /// </summary>
        public DateTime? ExpiredTime { get; set; }
        /// <summary>
        /// Địa điểm tổ chức
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Mã Thành phố
        /// </summary>
        public string ProvinceCode { get; set; }
        /// <summary>
        /// Tên Thành phố
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
        public string Deleted { get; set; }
        public string OverviewContent { get; set; }
        /// <summary>
        /// Mô tả Markdown hoặc HTML
        /// </summary>
        public string ContentType { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsFree { get; set; }
        /// <summary>
        /// Nhận vé bản cứng (Yes/No)
        /// </summary>
        public bool IsReceiveHardTicket { get; set; }
        /// <summary>
        /// Yêu cầu nhận hóa đơn (Yes/No)
        /// </summary>
        public bool IsRequestReceiveRecipt { get; set; }
        public bool IsLock { get; set; }
        public IEnumerable<AppEvtTicketDto> Tickets { get; set; }
        public IEnumerable<AppEvtEventDescriptionMediaDto> EvtEventDescriptionMedias { get; set; }
    }
}
