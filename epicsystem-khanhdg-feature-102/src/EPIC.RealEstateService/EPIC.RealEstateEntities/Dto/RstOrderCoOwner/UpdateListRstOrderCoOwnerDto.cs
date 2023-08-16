using EPIC.RealEstateEntities.Dto.RstOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrderCoOwner
{
    public class UpdateListRstOrderCoOwnerDto
    {
        public int OrderId { get; set; }
        public List<UpdateRstOrderCoOwnerDto> OrderCoOwners { get; set; }
    }
}
