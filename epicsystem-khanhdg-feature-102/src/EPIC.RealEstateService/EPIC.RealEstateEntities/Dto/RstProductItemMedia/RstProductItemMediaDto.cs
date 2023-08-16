using EPIC.RealEstateEntities.Dto.RstProductItemMediaDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItemMedia
{
    public class RstProductItemMediaDto
    {
        public int Id { get; set; }
        public int ProductItemId { get; set; }
        public string GroupTitle { get; set; }
        public List<RstProductItemMediaDetailDto> ProductItemMediaDetails { get; set; }
    }
}
