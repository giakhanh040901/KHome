using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtTicket
{
    public class AppQrScanTicketDto
    {
        /// <summary>
        /// Id vé
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id loại vé
        /// </summary>
        public int TicketId { get; set; }
        /// <summary>
        /// Trạng thái vé
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Mã vé
        /// </summary>
        public string TicketCode { get; set; }
        /// <summary>
        /// Thời gian check in
        /// </summary>
        public DateTime? CheckIn { get; set; }
        /// <summary>
        /// Thời gian check out
        /// </summary>
        public DateTime? CheckOut { get; set; }
        /// <summary>
        /// Check in tự động/thủ công
        /// </summary>
        public int? CheckInType { get; set; }
        /// <summary>
        /// Check out tự động/thủ công
        /// </summary>
        public int? CheckOutType { get; set; }
    }
}
