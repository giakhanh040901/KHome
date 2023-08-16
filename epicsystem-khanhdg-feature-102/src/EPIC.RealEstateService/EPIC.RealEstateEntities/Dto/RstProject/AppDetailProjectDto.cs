using EPIC.RealEstateEntities.Dto.RstOwner;
using EPIC.RealEstateEntities.Dto.RstProjectMediaDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProject
{
    public class AppDetailProjectDto
    {
        /// <summary>
        /// Tab tổng quan dự án
        /// </summary>
        public AppOverviewProjectDto OverviewProject { get; set; }
        /// <summary>
        /// Tab chủ đầu tư
        /// </summary>
        public AppViewDetailProjectOwnerDto Owner { get; set; }
        /// <summary>
        /// Tab hồ sơ dự án
        /// </summary>
        public AppProjectFilesDto Files { get; set; }
        /// <summary>
        /// Tab hồ sơ thiết kế (Hình ảnh căn hộ mẫu; Mặt bằng);
        /// </summary>
        public List<AppViewProjectMediaDetailDto> GroupMediaDetail { get; set; }

    }
}
