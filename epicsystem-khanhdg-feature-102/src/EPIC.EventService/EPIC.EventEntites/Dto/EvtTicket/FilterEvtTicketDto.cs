using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtTicket
{
    public class FilterEvtTicketDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Khung giờ sự kiện
        /// </summary>
        [FromQuery(Name = "eventDetailId")]
        public int EventDetailId { get; set; }
    }
}
