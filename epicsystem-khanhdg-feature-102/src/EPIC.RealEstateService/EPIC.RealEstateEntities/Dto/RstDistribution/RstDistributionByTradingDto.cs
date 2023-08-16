using EPIC.RealEstateEntities.Dto.RstProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDistribution
{
    public class RstDistributionByTradingDto
    {
        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public RstProjectDistributionDto Project { get; set; }
    }
}
