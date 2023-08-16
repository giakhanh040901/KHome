using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEvent
{
    public class FilterEvtEventDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Loại sự kiện
        /// </summary>
        [FromQuery(Name = "eventType")]
        public List<int> EventTypes { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        [FromQuery(Name = "status")]
        public List<int> Status { get; set; }

        /// <summary>
        /// Ngày tổ chức
        /// </summary>
        [FromQuery(Name = "startDate")]
        public DateTime? StartDate { get; set; }

        private string _organizator { get; set; }
        /// <summary>
        /// Ban tổ chức
        /// </summary>
        [FromQuery(Name = "organizator")]
        public string Organizator
        {
            get => _organizator;
            set => _organizator = value?.Trim();
        }
    }
}
