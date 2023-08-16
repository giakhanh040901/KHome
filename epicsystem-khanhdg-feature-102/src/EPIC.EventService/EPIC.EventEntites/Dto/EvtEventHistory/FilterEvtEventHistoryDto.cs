using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventHistory
{
    public class FilterEvtEventHistoryDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Trạng thái
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }
        /// <summary>
        /// Đã diễn ra chưa
        /// </summary>
        [FromQuery(Name = "isTookPlace")]
        public bool IsTookPlace { get; set; }

        [FromQuery(Name = "isSaleView")]
        public bool IsSaleView { get; set; }
    }
}
