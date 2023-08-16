using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProject
{
    public class AppRstProjectShortInfoDto
    {
        /// <summary>
        /// Id dự án
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Tên dự án
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Mã dự án
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Ảnh đại diện dự án
        /// </summary>
        public string AvatarUrlImage { get; set; }
        /// <summary>
        /// Đường dẫn điều hướng khi click Ảnh đại diện dự án
        /// </summary>
        public string AvatarUrlPath { get; set; }
    }
}
