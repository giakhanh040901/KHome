using EPIC.Utils.ConstantVariables.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtOrder
{
    public class EvtOrderTicketInfo
    {
        /// <summary>
        /// id order ticket
        /// </summary>
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int OrderDetailId { get; set; }
        /// <summary>
        /// loai ve
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ma ve
        /// </summary>
        public string TicketCode { get; set; }
        /// <summary>
        /// trang thai
        /// <see cref="EvtOrderTicketStatus"/>
        /// </summary>
        public int Status { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string TicketFilledUrl { get; set; }
    }

    public class EvtOrderTicketDto : EvtOrderTicketInfo
    {
        /// <summary>
        /// check in thuc te
        /// </summary>
        public DateTime? CheckInReal { get; set; }
        /// <summary>
        /// check out thuc te
        /// </summary>
        public DateTime? CheckOutReal { get; set; }
        /// <summary>
        /// mã yêu cầu
        /// </summary>
        public string ContractCode { get; set; }
        /// <summary>
        /// su kien
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// hinh thuc check in
        /// </summary>
        public int? CheckInType { get; set; }
        /// <summary>
        /// SĐT khác hàng
        /// </summary>
        public string Phone { get; set; }

    }
}
