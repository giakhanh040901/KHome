using EPIC.EventEntites.Dto.EvtTicket;
using EPIC.EventEntites.Entites;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventDetail
{
    public class EvtEventDetailDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Loại hình sự kiện
        /// </summary>
        public IEnumerable<int> EventTypes { get; set; }
        /// <summary>
        /// Id sự kiện
        /// </summary>
        public int EventId { get; set; }    
        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Số loại vé
        /// </summary>
        public int TicketTypeQuantity { get; set;}
        /// <summary>
        /// Tổng số lượng vé
        /// </summary>
        public int TicketQuantity { get; set;}
        /// <summary>
        /// Đăng ký
        /// </summary>
        public int RegisterQuantity { get; set;}
        /// <summary>
        /// Đã thanh toán
        /// </summary>
        public int PayQuantity { get; set;}
        /// <summary>
        /// Thời gian chờ thanh toán
        /// </summary>
        public int? PaymentWaitingTime { get; set; }
        /// <summary>
        /// Có Hiển thì số vé còn lại trên app không
        /// </summary>
        public bool IsShowRemaingTicketApp { get; set; }
        /// <summary>
        /// Số lượng vé còn lại
        /// </summary>
        public int RemainingTickets { get; set; }
        /// <summary>
        /// Danh sách vé
        /// </summary>
        public IEnumerable<EvtTicketDto> Tickets { get; set; }
    }
}
