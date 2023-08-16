using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Distribution
{
    public class UpdateDistributionDto : CreateDistributionDto
    {
        public int Id { get; set; }
        public string Image { get; set; }
    }
}
