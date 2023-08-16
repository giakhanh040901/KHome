using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectUtilityExtend
{
    public class RstProjectUtilityExtendDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string IconName { get; set; }
        public int GroupUtilityId { get; set; }
        public string Status { get; set; }
        public int Type { get; set; }
        public string IsHighlight { get; set; }
    }
}
