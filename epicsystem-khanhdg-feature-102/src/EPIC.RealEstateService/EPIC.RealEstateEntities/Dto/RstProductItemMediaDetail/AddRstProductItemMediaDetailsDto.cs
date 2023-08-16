using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItemMediaDetail
{
    public class AddRstProductItemMediaDetailsDto
    {
        public int ProductItemMediaId { get; set; }
        public List<CreateRstProductItemMediaDetailDto> RstProductItemMediaDetails { get; set; }
    }
}
