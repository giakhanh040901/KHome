using EPIC.RealEstateEntities.Dto.RstProjectUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItemProjectUtility
{
    public class RstProductItemUtilityDto
    {
        public int Id { get; set; }
        /// <summary>
        /// ID dự án
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// ID căn hộ
        /// </summary>
        public int ProductItemId { get; set; }
        /// <summary>
        /// Id tiện ích
        /// </summary>
        public int UtilityId { get; set; }
        /// <summary>
        /// Tên tiện ích
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Chon
        /// </summary>
        public string IsProductItemSelected { get; set; }
    }
}
