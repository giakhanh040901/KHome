using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.DistributionVideo
{
    public class CreateDistributionVideoDto
    {
        public int DistributionId { get; set; }
        public string UrlVideo { get; set; }
        public string Title { get; set; }
    }
}
