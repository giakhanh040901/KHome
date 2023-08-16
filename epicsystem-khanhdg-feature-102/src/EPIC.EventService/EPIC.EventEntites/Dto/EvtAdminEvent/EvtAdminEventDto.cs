using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtAdminEvent
{
    /// <summary>
    /// ADmin event
    /// </summary>
    public class EvtAdminEventDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id sự kiện
        /// </summary>
        public int EventId { get; set; }
        /// <summary>
        /// Tên sự kiện
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// Investor Id
        /// </summary>
        public int InvestorId { get; set; }
        /// <summary>
        /// Tên investor
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string Phone { get; set; }
    }
}
