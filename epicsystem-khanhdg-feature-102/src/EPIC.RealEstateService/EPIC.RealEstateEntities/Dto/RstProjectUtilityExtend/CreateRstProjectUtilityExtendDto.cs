using EPIC.RealEstateEntities.Dto.RstProjectUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectUtilityExtend
{
    public class CreateRstProjectUtilityExtendDto
    {
        public int ProjectId { get; set; }
        public List<ViewCreateRstProjectUtilityExtendDto> UtilityExtends { get; set; }
      
    }
    public class ViewCreateRstProjectUtilityExtendDto
    {
        public string Title { get; set; }
        public int GroupUtilityId { get; set; }
        public string IconName { get; set; }
        public int Type { get; set; }
        public string IsHighlight { get; set; }
    }

}
