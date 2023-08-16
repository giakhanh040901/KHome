using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtTicketTemplate
{
    public class FilterEvtTicketTemplateDto : PagingRequestBaseDto
    {
        /// <summary>
        /// id su kien
        /// </summary>
        [FromQuery(Name = "eventId")]
        public int EventId { get; set; }
    }
}
