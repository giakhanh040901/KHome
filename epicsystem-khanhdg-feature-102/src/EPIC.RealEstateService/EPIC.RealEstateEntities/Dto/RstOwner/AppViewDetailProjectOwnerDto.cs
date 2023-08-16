using EPIC.RealEstateEntities.Dto.RstProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOwner
{
    public class AppViewDetailProjectOwnerDto
    {
        /// <summary>
        /// Tên chủ đầu tư
        /// </summary>
        public string OwnerName { get; set; }
        /// <summary>
        /// Đã xác minh 
        /// </summary>
        public bool IsCheck { get; set; }
        /// <summary>
        /// Đánh giá
        /// </summary>
        public int? Rating { get; set; }
        /// <summary>
        /// Mô tả chủ đầu tư
        /// </summary>
        public string DescriptionContent { get; set; }

        /// <summary>
        /// Loại nội dung: MARKDOWN, HTML
        /// </summary>
        public string DescriptionContentType { get; set; }

        /// <summary>
        /// Website của chủ đầu tư
        /// </summary>
        public string Website { get; set; }
        /// <summary>
        /// Hotline của chủ đầu tư
        /// </summary>
        public string Hotline { get; set; }
        /// <summary>
        /// Fanpage của chủ đầu tư
        /// </summary>
        public string Fanpage { get; set; }
        /// <summary>
        /// Danh sách dự án của chủ đầu tư do đại lý hiện tại mở bán
        /// </summary>
        public List<AppRstProjectShortInfoDto> ListProjects { get; set; }
    }
}
