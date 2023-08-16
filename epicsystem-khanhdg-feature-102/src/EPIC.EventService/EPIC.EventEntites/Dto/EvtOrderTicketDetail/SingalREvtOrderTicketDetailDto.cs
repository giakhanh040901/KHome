using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtOrderTicketDetail
{
    public class SingalREvtOrderTicketDetailDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ID đại lý
        /// </summary>
        public int TradingProviderId { get; set; }
        /// <summary>
        /// Id chi tiết lệnh
        /// </summary>
        public int OrderDetailId { get; set; }
        /// <summary>
        /// Id vé
        /// </summary>
        public int TicketId { get; set; }
        /// <summary>
        /// Mã QR vé
        /// </summary>
        public string TicketCode { get; set; }
        /// <summary>
        /// Thời gian check
        /// </summary>
        public DateTime? CheckIn { get; set; }
        /// <summary>
        /// Thời gian check out
        /// </summary>
        public DateTime? CheckOut { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Hình thức check in
        /// </summary>
        public int? CheckInType { get; set; }
        /// <summary>
        /// Hình thức check out
        /// </summary>
        public int? CheckOutType { get; set; }
    }
}
