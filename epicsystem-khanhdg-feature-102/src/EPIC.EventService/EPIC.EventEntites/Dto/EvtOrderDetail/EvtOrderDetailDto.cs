using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtOrderDetail
{
    public class EvtOrderDetailDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int TicketId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        /// <summary>
        /// Loai ve
        /// </summary>
        public string Name { get; set;}
        /// <summary>
        /// Mô tả ngắn
        /// </summary>
        public string Depscription { get; set; }
        /// <summary>
        /// so ve hien tai
        /// </summary>
        public int CurrentTickets { get; set; }
    }
}
