using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint
{
    public class FindAccumulatePointByInvestorId : PagingRequestBaseDto
    {
        /// <summary>
        /// lý do
        /// </summary>
        [FromQuery(Name = "reason")]
        public int? Reason { get; set; }

        /// <summary>
        /// loại hình (1: tích; 2: tiêu)
        /// </summary>
        [FromQuery(Name = "pointType")]
        public int? PointType { get; set; }
    }
}
