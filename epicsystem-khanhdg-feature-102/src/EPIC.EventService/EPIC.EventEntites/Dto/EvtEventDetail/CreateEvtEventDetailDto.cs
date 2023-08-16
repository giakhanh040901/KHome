using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventDetail
{
    public class CreateEvtEventDetailDto
    {
        /// <summary>
        /// Id sự kiện
        /// </summary>
        public int EventId { get; set; }
        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Thời gian kết thúc
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Thời gian chờ thanh toán (Nếu không cài thì là không có)
        /// </summary>
        public int? PaymentWaittingTime { get; set; }
        public bool IsShowRemaingTicketApp { get; set; }
    }
}
