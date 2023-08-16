using EPIC.InvestEntities.Dto.ProjectOverViewFile;
using EPIC.InvestEntities.Dto.ProjectOverviewOrg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Distribution
{
    public class OverViewDistributionDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public string ContentType { get; set; }
        public string OverviewContent { get; set; }
        public string OverviewImageUrl { get; set; }
        public List<ViewProjectOverViewFileDto> ProjectOverviewFiles { get; set; }
        public List<ViewProjectOverviewOrgDto> ProjectOverviewOrgs { get; set; }
    }
}
