using EPIC.BondEntities.Dto.BondSecondaryOverviewFile;
using EPIC.BondEntities.Dto.BondSecondaryOverviewOrg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.BondSecondary
{
    public class BondSecondaryOverviewDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public string ContentType { get; set; }
        public string OverviewContent { get; set; }
        public string OverviewImageUrl { get; set; }
        public List<ViewBondSecondaryOverViewFileDto> BondSecondaryOverviewFiles { get; set; }
        public List<ViewBondSecondaryOverViewOrgDto> BondSecondaryOverviewOrgs { get; set; }
    }
}
