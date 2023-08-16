using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtAdminEvent
{
    public class CreateEvtAdminEventDto
    {
        /// <summary>
        /// Id sự kiện
        /// </summary>
        public int EventId { get; set; }
        /// <summary>
        /// Danh sách investor
        /// </summary>
        public List<int> InvestorIds { get; set; }
    }
}
