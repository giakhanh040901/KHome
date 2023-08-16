using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEvent
{
    public class AppEventDetailDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// co hien so luong tren app khong
        /// </summary>
        public bool IsShowRemaingTicketApp { get; set; }
        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime? EndDate { get; set; }
        public IEnumerable<AppTicketInfoDto> Tickets { get; set; }
    }

    public class AppTicketInfoDto : AppEvtTicketDto
    {
        /// <summary>
        /// mốc Thời gian còn lại
        /// </summary>
        public DateTime? TimeRemaining { get; set; }
        /// <summary>
        /// Số vé còn lại 
        /// </summary>
        public int RemainingTickets { get; set; }
    }
}
