using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.Dashboard
{
    public class FilterCoreDashboardDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Ngày bắt đầu 
        /// </summary>
        [FromQuery(Name = "startDate")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Ngày cuối
        /// </summary>
        [FromQuery(Name = "endDate")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Đại lý
        /// </summary>
        [FromQuery(Name = "tradingProviderId")]
        public int? TradingProviderId { get; set; }
    }
}
