using EPIC.RealEstateEntities.Dto.RstProjectMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectMediaDetail
{
    public class RstProjectMediaDetailSortDto
    {
        public int ProjectId { get; set; }
        public List<RstProjectMediaDetailSortOrderDto> Sort { get; set; }
    }
}
