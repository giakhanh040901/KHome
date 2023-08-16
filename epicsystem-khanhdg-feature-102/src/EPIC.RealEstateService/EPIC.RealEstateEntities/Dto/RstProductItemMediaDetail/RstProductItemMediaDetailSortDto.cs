using EPIC.RealEstateEntities.Dto.RstProductItemMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItemMediaDetail
{
    public class RstProductItemMediaDetailSortDto
    {
        public int ProductItemId { get; set; }
        public List<RstProductItemMediaDetailSortOrderDto> Sort { get; set; }
    }
}
