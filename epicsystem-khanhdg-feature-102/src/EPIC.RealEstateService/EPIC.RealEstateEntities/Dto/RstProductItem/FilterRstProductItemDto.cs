using EPIC.EntitiesBase.Dto;
using EPIC.Utils.ConstantVariables.RealEstate;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    public class FilterRstProductItemDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Số căn/tên
        /// </summary>
        private string _name;
        [FromQuery(Name = "name")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        /// <summary>
        /// Mã căn/mã sản phẩm
        /// </summary>
        private string _code;
        [FromQuery(Name = "code")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        /// <summary>
        /// Id dự án
        /// </summary>
        [FromQuery(Name = "projectId")]
        public int? ProjectId { get; set; }

        /// <summary>
        /// Id mật độ xây dựng (lấy từ RstProjectStructure)
        /// </summary>
        [FromQuery(Name = "buildingDensityId")]
        public int? BuildingDensityId { get; set; }

        /// <summary>
        /// Phân loại sản phẩm (1: Căn hộ thông thường, 2: Căn hộ Studio, 3: Căn hộ Officetel, 4: Căn hộ Shophouse, 5: Căn hộ Penthouse, 6: Căn hộ Duplex, 
        /// 7: Căn hộ Sky Villa, 8: Nhà ở nông thôn, 9: Biệt thự nhà ở, 10: Liền kề, 11: Chung cư thấp tầng, 12: Căn Shophouse, 13: Biệt thự nghỉ dưỡng, 14: Villa)
        /// </summary>
        [FromQuery(Name = "classifyType")]
        public int? ClassifyType { get; set; }

        /// <summary>
        /// Trạng thái của sản phẩm (1: Khởi tạo (có thể là chưa mở bán hoặc đang mở bán) 2: Giữ chỗ, 3: Khóa căn, 4: Đã cọc, 5: Đã bán) 
        /// 2 trạng thái 6: chưa mở bán và 7: đang bán <br/>
        /// <see cref="RstProductItemStatus"/>
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        /// <summary>
        /// Loại sổ (1: Có sổ đỏ, 2: Chưa có sổ, 3: Sổ 50 năm, 4: Sổ lâu dài)
        /// </summary>
        [FromQuery(Name = "redBookType")]
        public int? RedBookType { get; set; }
    }
}
