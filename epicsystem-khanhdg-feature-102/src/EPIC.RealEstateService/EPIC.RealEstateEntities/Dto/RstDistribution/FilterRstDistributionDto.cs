using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDistribution
{
    public class FilterRstDistributionDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Dự án
        /// </summary>
        [FromQuery(Name = "projectId")]
        public int? ProjectId { get; set; }

        /// <summary>
        /// Đại lý
        /// </summary>
        [FromQuery(Name = "tradingProviderId")]
        public int? TradingProviderId { get; set; }

        /// <summary>
        /// Trạng thái (1: Khoi tao, 2: Cho duyet, 3: Hoat dong, 4: Huy duyet, 5:Dong)
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }
    }
}
