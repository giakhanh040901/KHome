using EPIC.RealEstateEntities.Dto.RstProjectUtilityExtend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectUtilityExtend
{
    public class UpdateRstProjectUtilityExtendDto : ViewCreateRstProjectUtilityExtendDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
    }
}
