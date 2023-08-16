using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSellDetail
{
    public class FilterRstOpenSellDetailDto : PagingRequestBaseDto
    {

        [FromQuery(Name = "openSellId")]
        public int OpenSellId { get; set; }

        [FromQuery(Name = "level")]
        public int? Level { get; set; }

        [FromQuery(Name = "projectId")]
        public int? ProjectId { get; set; }

        [FromQuery(Name = "redBookType")]
        public int? RedBookType { get; set; }
        public int? BuildingDensityId { get; set; }

        [FromQuery(Name = "status")]
        public int? Status { get; set; }
    }
}
