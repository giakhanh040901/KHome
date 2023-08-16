using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerBlockadeLiberation
{
    public class FilterGarnerBlockadeLiberationDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "type")]
        public int? Type { get; set; }

        [FromQuery(Name = "status")]
        public int? Status { get; set; }
    }
}
