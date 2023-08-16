using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerProductOverview
{
    public class GarnerProductOverviewOrgDto
    {
        public int Id { get; set; }
        public int DistributionId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Role { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string Status { get; set; }
        public string Deleted { get; set; }
    }
}
