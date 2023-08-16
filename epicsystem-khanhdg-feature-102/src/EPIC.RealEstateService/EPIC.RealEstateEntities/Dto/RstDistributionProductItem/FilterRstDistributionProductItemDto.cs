using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDistributionProductItem
{
    public class FilterRstDistributionProductItemDto: PagingRequestBaseDto
    {
        /// <summary>
        /// Lọc theo trạng thái
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }
    }
}
