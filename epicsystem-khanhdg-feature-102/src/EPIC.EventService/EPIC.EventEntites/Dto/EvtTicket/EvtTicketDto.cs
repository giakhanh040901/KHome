using EPIC.Entities.DataEntities;
using EPIC.EventEntites.Dto.EvtTicketMedia;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtTicket
{
    public class EvtTicketDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        /// <summary>
        /// Id khung giờ
        /// </summary>
        public int EventDetailId { get; set; }
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
        /// Show app
        /// </summary>
        public string IsShowApp { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Đăng ký
        /// </summary>
        public int RegisterQuantity { get; set; }
        /// <summary>
        /// Đã thanh toán
        /// </summary>
        public int PayQuantity { get; set; }
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
        public string CreatedBy { get; set; }
        /// <summary>
        /// Số vé còn lại
        /// </summary>
        public int RemainingTickets { get; set; }
        /// <summary>
        /// Danh sách ảnh
        /// </summary>
        public IEnumerable<EvtTicketMediaDto> UrlImages { get; set; }
    }
}
