using EPIC.InvestEntities.Dto.DistributionNews;
using EPIC.InvestEntities.Dto.DistributionVideo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvestProject
{
    public class AppProjectNewsDto
    {
        public List<AppDistributionNewsDto> News { get; set; }
        public AppDistributionVideoDto Videos { get; set; }

    }
}
