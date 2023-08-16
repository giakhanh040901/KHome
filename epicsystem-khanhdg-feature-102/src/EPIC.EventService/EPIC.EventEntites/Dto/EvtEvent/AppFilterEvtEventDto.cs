using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEvent
{
    public class AppFilterEvtEventDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "isSaleView")]
        public bool IsSaleView { get; set; }
        /// <summary>
        /// Loại hình sự kiện
        /// </summary>
        [FromQuery(Name = "eventType")]
        public int? EventType { get; set; }
    }
}
