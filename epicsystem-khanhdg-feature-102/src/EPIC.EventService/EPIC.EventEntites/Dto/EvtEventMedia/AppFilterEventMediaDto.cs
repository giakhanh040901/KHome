using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventMedia
{
    public class AppFilterEventMediaDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "eventId")]
        public int? EventId { get; set; }
        private string _mediaType { get; set; }
        [FromQuery(Name = "mediaType")]
        public string MediaType
        {
            get => _mediaType;
            set => _mediaType = value?.Trim();
        }

        private string _mediaDetailType { get; set; }
        [FromQuery(Name = "mediaDetailType")]
        public string MediaDetailType
        {
            get => _mediaDetailType;
            set => _mediaDetailType = value?.Trim();
        }
        [FromQuery(Name = "isSaleView")]
        public bool IsSaleView { get; set; }
    }
}
