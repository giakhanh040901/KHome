using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectStructure
{
    public class ViewRstProjectStructureDto
    {
        public string ProjectName { get; set; }
        public string AvatarImage { get; set; }
        public List<RstProjectStructureDto> ProjectStructures { get; set; }
    }
}
