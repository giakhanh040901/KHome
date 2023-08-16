using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtOrder
{
    public class FilterEvtOrderTicketInfoDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "orderId")]
        public int OrderId { get; set; }
    }

    public class FilterEvtOrderTicketDto : FilterEvtOrderTicketInfoDto
    {
        /// <summary>
        /// mã yêu cầu
        /// </summary>
        [FromQuery(Name = "contractCode")]
        public string ContractCode { get; set; }

        /// <summary>
        /// mã vé
        /// </summary>
        [FromQuery(Name = "ticketCode")]
        public string TicketCode { get; set; }

        [FromQuery(Name = "status")]
        public List<int?> Status { get; set; }

        /// <summary>
        /// lọc theo sự kiện
        /// </summary>
        [FromQuery(Name = "eventIds")]
        public List<int?> EventIds { get; set; }

        /// <summary>
        /// lọc theo hinh thuc checkin
        /// </summary>
        [FromQuery(Name = "checkInType")]
        public int? CheckInType { get; set; }

        /// <summary>
        /// lọc theo số điện thoại
        /// </summary>
        private string _phone;
        [FromQuery(Name = "phone")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }
    }
}
