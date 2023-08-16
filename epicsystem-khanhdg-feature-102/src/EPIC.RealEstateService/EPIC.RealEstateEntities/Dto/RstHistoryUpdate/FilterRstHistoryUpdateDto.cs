using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstHistoryUpdate
{
    public class FilterRstHistoryUpdateDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "realTableId")]
        public int? RealTableId { get; set; }
    }
}
