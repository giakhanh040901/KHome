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
    public class FilterEvtEventMediaDto : PagingRequestBaseDto
    {
        private string _status;
        [FromQuery(Name = "status")]
        public string Status
        {
            get => _status;
            set => _status = value?.Trim();
        }

        /// <summary>
        /// Vị trí
        /// </summary>
        private string _location;
        [FromQuery(Name = "location")]
        public string Location
        {
            get => _location;
            set => _location = value?.Trim();
        }

        [FromQuery(Name = "eventId")]
        public int? EventId { get; set; }

        /// <summary>
        /// Loại sự kiện
        /// </summary>
        private string _mediaType;
        [FromQuery(Name = "mediaType")]
        public string MediaType
        {
            get => _mediaType;
            set => _mediaType = value?.Trim();
        }
    }
}
