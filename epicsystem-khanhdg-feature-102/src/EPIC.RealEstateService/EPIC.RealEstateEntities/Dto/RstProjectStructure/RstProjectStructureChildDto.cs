using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectStructure
{
    public class RstProjectStructureChildDto
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        /// <summary>
        /// Mật độ mức mấy (1, 2, 3)
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Id cha
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Loại mật độ xây dựng: 1:Tòa, 2: Khu, 3: Ô đất, 4: Lô, 5: Tầng<br/>
        /// <see cref="RstBuildingDensityTypes"/>
        /// </summary>
        public int? BuildingDensityType { get; set; }

        /// <summary>
        /// Mã mật độ
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên mật độ
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Danh sách số tầng trong mật độ này
        /// </summary>
        public List<string> NoFloors { get; set; }
    }
}
