using EPIC.RealEstateEntities.Dto.RstProjectMediaDetail;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Media;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectMedia
{
    public class AppViewProjectMediaDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Tên nhóm (nhóm hình ảnh)
        /// </summary>
        public string GroupTitle { get; set; }
        /// <summary>
        /// Đường dẫn file ảnh
        /// </summary>
        //[Required]
        public string UrlImage { get; set; }

        /// <summary>
        /// Đường dẫn điều hướng khi click ảnh
        /// </summary>
        public string UrlPath { get; set; }

        /// <summary>
        /// IMAGE: ảnh, VIDEO: video
        /// <see cref="MediaTypes"/>
        /// </summary>
        public string MediaType { get; set; }

        /// <summary>
        /// Vị trí
        /// <see cref="RstMediaLocations"/>
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Danh sách media detail nếu là nhóm ảnh
        /// </summary>
        public List<AppViewProjectMediaDetailDto> Details { get; set; }
    }
}
