using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtOrderTicketDetail
{
    public class AppViewOrderTicketDetailDto
    {
        public int Id { get; set; }
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int TicketId { get; set; }
        public string TicketCode { get; set; }
        /// <summary>
        /// Thời gian checkIn thực tế
        /// </summary>
        public DateTime? CheckIn { get; set; }
        /// <summary>
        /// Thời gian check Out thực tế
        /// </summary>
        public DateTime? CheckOut { get; set; }
        /// <summary>
        /// Thời gian checkIn trên vé
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// THời gian checkOut trên vé
        /// </summary>
        public DateTime? EndDate { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Avatar đại lý
        /// </summary>
        public string AvatarTradingImageUrl { get; set; }
        /// <summary>
        /// File đã fill cho ticket
        /// </summary>
        public string TicketFilledUrl { get; set; }
    }
}
