using EPIC.CompanySharesEntities.Dto.CpsInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CpsApp
{
    public class AppCpsInfoOverviewDto
    {
        public string OverviewImageUrl { get; set; }
        public string ContentType { get; set; }
        public string OverviewContent { get; set; }
        public List<CpsInfoOverviewFileDto> CpsOverviewFiles { get; set; }
        public List<CpsInfoOverviewOrgDto> CpsOverviewOrgs { get; set; }
    }
}
