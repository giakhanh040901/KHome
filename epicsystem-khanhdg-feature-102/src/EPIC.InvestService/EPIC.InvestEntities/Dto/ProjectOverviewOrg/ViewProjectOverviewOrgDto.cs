using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ProjectOverviewOrg
{
    public class ViewProjectOverviewOrgDto
    {
        public int Id { get; set; }
        public int DistributionId { get; set; }
        public int TradingProviderId { get; set; }
        public string Name { get; set; }
        public string OrgCode { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public int Status { get; set; }
    }
}
