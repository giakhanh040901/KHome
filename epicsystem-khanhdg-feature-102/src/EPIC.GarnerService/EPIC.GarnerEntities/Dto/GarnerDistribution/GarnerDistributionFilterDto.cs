using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerDistribution
{
    public class GarnerDistributionFilterDto
    {
        /// <summary>
        /// List id đại lý
        /// </summary>
        [FromQuery(Name = "tradingProviderIds")]
        public List<int> TradingProviderIds { get; set; }

        [FromQuery(Name = "status")]
        public string Status { get; set; }
    }
}
