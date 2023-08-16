using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using EPIC.Utils.ConstantVariables.Loyalty;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint
{
    public class FindHisAccumulatePointDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Tích điểm hay tiêu điểm
        /// 1: Tích điểm, 2: Tiêu điểm<br/>
        /// <see cref="LoyPointTypes"/>
        /// </summary>
        [FromQuery(Name = "pointType")]
        public int? PointType { get; set; }

        [FromQuery(Name = "status")]
        public int? Status { get; set; }
    }
}
