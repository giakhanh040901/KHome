using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.DistributionNews
{
    public class CreateDistributionNewsDto
    {
        public int DistributionId { get; set; }
        public string ImgUrl { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
