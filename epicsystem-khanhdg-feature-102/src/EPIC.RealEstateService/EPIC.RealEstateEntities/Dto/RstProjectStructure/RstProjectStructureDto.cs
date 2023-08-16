using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectStructure
{
    public class RstProjectStructureDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int PartnerId { get; set; }
        public int Level { get; set; }
        public int? ParentId { get; set; }
        public int BuildingDensityType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
