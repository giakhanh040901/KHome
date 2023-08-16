using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItemProjectUtility
{
    public class CreateRstProductItemUtilityDto
    {
        /// <summary>
        /// Id tiện ích
        /// </summary>
        public List<int?> ProductItemUtilities { get; set; }

        /// <summary>
        /// ID tiện ích mở rộng
        /// </summary>
        public List<int?> ProjectUtilityExtends { get; set; }

        /// <summary>
        /// Id productItem
        /// </summary>
        public int ProductItemId { get; set; }
    }
}
