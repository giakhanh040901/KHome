using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDashboard
{
    public class GetRstDashboardDto
    {
        [FromQuery(Name = "firstDate")]
        public DateTime? FirstDate { get; set; }

        [FromQuery(Name = "endDate")]
        public DateTime? EndDate { get; set; }

        [FromQuery(Name = "distributionId")]
        public int? DistributionId { get; set; }

        [FromQuery(Name = "tradingProviderId")]
        public int? TradingProviderId { get; set; }
        [FromQuery(Name = "projectId")]
        public int? ProjectId { get; set; }
        [FromQuery(Name = "sort")]
        public bool Sort { get; set; }
    }
}
