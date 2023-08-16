using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtTicket
{
    public class CreateListRepicateTicketDto
    {
        /// <summary>
        /// Id khung giờ hiện tại
        /// </summary>
        public int EventDetailId { get; set; }
        /// <summary>
        /// Danh sách Id vé cần nhân bản
        /// </summary>
        public List<int> ReplicateTickets { get; set; }
    }
}
