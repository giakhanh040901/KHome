using EPIC.RealEstateEntities.Dto.RstProjectStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    /// <summary>
    /// Các tham số chuẩn bị cho lọc sản phẩm dự án
    /// </summary>
    public class AppGetParamsFindProductItemDto
    {
        /// <summary>
        /// Giá bán tối thiểu
        /// </summary>
        public decimal? MinSellingPrice { get; set; }

        /// <summary>
        /// Giá bán tối đa
        /// </summary>
        public decimal? MaxSellingPrice { get; set; }

        /// <summary>
        /// Mật độ xây dựng
        /// </summary>
        public List<RstProjectStructureChildDto> BuildingDensity { get; set; }
    }
}
