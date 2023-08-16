using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDistribution
{
    /// <summary>
    /// Cập nhật phân phối 
    /// </summary>
    public class UpdateRstDistributionDto : CreateRstDistributionDto
    {
        public int Id { get; set; }
    }
}
