using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EPIC.InvestEntities.Dto.Dashboard
{
    public class GetInvDashboardDuChiDto
    {
        [FromQuery(Name = "month")]
        public int Month { get; set; }

        [FromQuery(Name = "year")]
        public int Year { get; set; }
    }
}
