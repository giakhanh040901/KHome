using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectMediaDetail
{
    public class AppViewProjectMediaDetailDto
    {
        /// <summary>
        /// Id media detail
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Tên nhóm hình ảnh
        /// </summary>
        public string GroupTitle { get; set; }
        /// <summary>
        /// Loại nhóm hình ảnh (CanHoMauDuAn: Hình ảnh căn hộ mẫu; AnhMatBangDuAn: Mặt bằng)
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Đường dẫn 
        /// </summary>
        public string UrlImage { get; set; }
        /// <summary>
        /// Loại hình ảnh
        /// </summary>
        public string MediaType { get; set; }
    }
}
