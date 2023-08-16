using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItemMedia
{
    public class RstProductItemMediaSortDto
    {
        public int ProductItemId { get; set; }
        public string Location { get; set; }
        public List<RstProductItemMediaSortOrderDto> Sort { get; set; }
    }
}
