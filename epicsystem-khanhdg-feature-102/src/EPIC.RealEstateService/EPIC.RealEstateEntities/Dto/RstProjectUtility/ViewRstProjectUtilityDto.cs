using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectUtility
{
    public class ViewRstProjectUtilityDto
    {
        public int Id { get; set; }
        public int UtilityId { get; set; }
        public int ProjectId { get; set; }
        public string IsHighlight { get; set; }
    }
}
