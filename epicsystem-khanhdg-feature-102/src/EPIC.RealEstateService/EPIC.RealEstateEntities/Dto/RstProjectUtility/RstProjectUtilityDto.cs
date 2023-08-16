using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectUtility
{
    public class RstProjectUtilityDto
    {
        /// <summary>
        /// Id của tiện ích dự án
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Id của tiện ích
        /// </summary>
        public int UtilityId { get; set; }
        /// <summary>
        /// Tên tiện ích
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Nổi bật
        /// </summary>
        public string IsHighlight { get; set; }
        /// <summary>
        /// Chọn hay không ở phần quản lý 
        /// </summary>
        public string IsSelected { get; set; }
        /// <summary>
        /// Id tiện ích
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// Tên nhóm tiện ích
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// Loại tiện ích
        /// </summary>
        public int? Type { get; set; }

    }
}
