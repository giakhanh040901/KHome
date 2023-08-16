using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectMedia
{
    public class RstProjectMediaSortDto
    {
        public int ProjectId { get; set; }
        public string Location { get; set; }
        public List<RstProjectMediaSortOrderDto> Sort { get; set; }
    }
}
