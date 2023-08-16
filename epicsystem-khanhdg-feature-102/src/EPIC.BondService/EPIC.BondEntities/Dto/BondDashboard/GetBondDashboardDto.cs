
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.BondDashboard
{
    public class GetBondDashboardDto
    {
        [FromQuery(Name = "firstDate")]
        public DateTime FirstDate { get; set; }

        [FromQuery(Name = "endDate")]
        public DateTime EndDate { get; set; }

        [FromQuery(Name = "secondaryId")]
        public int? SecondaryId { get; set; }
    }
}
