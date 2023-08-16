using EPIC.RealEstateEntities.Dto.RstProjectUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItemProjectUtility
{
    public class RstSelectProductItemProjectUtilityDto 
    {
        /// <summary>
        /// Id tiện ích đc chọn
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Id tiện ích dự án cài đặt
        /// </summary>
        public int? ProjectUtilityId { get; set; }

        /// <summary>
        /// Id tiện ích dự mở rộng
        /// </summary>
        public int? ProjectUtilityExtendId { get; set; }
        /// <summary>
        /// Id tiện ích
        /// </summary>
        public int? UtilityId { get; set; }
        /// <summary>
        /// Tên tiện ích
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Nổi bật
        /// </summary>
        public string IsHighlight { get; set; }
        /// <summary>
        /// Nhóm tiện ích
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// Loại tiện ích
        /// </summary>
        public int? Type { get; set; }
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
