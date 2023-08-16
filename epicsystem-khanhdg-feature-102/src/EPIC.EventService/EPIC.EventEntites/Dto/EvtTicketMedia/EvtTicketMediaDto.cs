using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtTicketMedia
{
    public class EvtTicketMediaDto
    {
        /// <summary>
        /// Id ảnh
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id vé
        /// </summary>
        public int TicketId { get; set; }
        /// <summary>
        /// Đường dẫn ảnh
        /// </summary>
        public string UrlImage { get; set; }
    }
}
