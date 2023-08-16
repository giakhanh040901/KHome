using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEvent
{
    public class SignalREvtEventDto
    {
        /// <summary>
        /// ID sự kiện
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Tên sự kiện
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// BTC
        /// </summary>
        public string Organizator { get; set; }
        /// <summary>
        /// Id đại lý
        /// </summary>
        public int TradingProviderId { get; set; }
        /// <summary>
        /// Loại hình sự kiện
        /// </summary>
        public IEnumerable<int> EventTypes { get; set; }
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
        public string TicketTicketPurchasePolicy { get; set; }
        /// <summary>
        /// Số lượng vé còn lại
        /// </summary>
        public int RemainingTickets { get; set; }
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
    }
}
