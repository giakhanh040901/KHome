using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerProductPrice
{
    public class FilterGarnerProductPriceDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "ditributionId")]
        public int DitributionId { get; set; }
    }
}
