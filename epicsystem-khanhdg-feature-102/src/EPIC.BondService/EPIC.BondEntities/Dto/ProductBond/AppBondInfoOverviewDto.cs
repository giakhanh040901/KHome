using EPIC.BondEntities.Dto.BondInfoOverviewFile;
using EPIC.BondEntities.Dto.BondSecondaryOverviewOrg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto
{
    public class AppBondInfoOverviewDto
    {
        public string OverviewImageUrl { get; set; }
        public string ContentType { get; set; }
        public string OverviewContent { get; set; }
        public List<BondInfoOverviewFileDto> BondOverviewFiles { get; set; }
        public List<BondInfoOverviewOrgDto> BondOverviewOrgs { get; set; }
    }
}
