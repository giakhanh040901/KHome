using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectStructure
{
    public class FilterRstProjectStructureDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "projectId")]
        public int? ProjectId { get; set; }
    }
}
