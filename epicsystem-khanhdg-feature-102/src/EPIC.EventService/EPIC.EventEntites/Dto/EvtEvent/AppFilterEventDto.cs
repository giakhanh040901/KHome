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
    public class AppFilterEventDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "eventId")]
        public int? EventId { get; set; }
        [FromQuery(Name = "eventDetailId")]
        public int? EventDetailId { get; set; }

        private string _name { get; set; }
        [FromQuery(Name = "name")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }
        [FromQuery(Name = "isSaleView")]
        public bool IsSaleView { get; set; }
    }
}
