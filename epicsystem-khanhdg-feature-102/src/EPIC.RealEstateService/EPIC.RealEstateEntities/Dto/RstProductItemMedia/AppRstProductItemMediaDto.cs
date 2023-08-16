using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Media;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItemMedia
{
    public class AppRstProductItemMediaDto
    {
        public int Id { get; set; }

        public int ProductItemId { get; set; }

        /// <summary>
        /// Tên nhóm (nhóm hình ảnh)
        /// </summary>
        public string GroupTitle { get; set; }

        /// <summary>
        /// Vị trí  AnhDaiDienCanHo, SlideHinhAnhCanHo, MatBangCanHo, VatLieu"<br/>
        /// <see cref="RstProductItemMediaLocations"/>
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Loại media: IMAGE, VIDEO<br/>
        /// <see cref="MediaTypes"/>
        /// </summary>
        public string MediaType { get; set; }

        /// <summary>
        /// Đường dẫn file ảnh
        /// </summary>
        public string UrlImage { get; set; }

        /// <summary>
        /// Đường dẫn điều hướng khi click ảnh
        /// </summary>
        public string UrlPath { get; set; }
    }
}
