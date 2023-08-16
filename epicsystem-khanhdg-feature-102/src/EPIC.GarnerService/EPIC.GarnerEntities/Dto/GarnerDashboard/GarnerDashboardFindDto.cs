using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerDashboard
{
    public class GarnerDashboardFindDto
    {
        [FromQuery(Name = "firstDate")]
        public DateTime FirstDate { get; set; }
        [FromQuery(Name = "endDate")]
        public DateTime EndDate { get; set; }
        [FromQuery(Name = "ProductId")]
        public int? ProductId { get; set; }
    }
}
