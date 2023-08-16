using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDashboard
{
    public class NumberOfApartmentsChartDto
    {
        /// <summary>
        /// Phân loại sản phẩm (1: Căn hộ thông thường, 2: Căn hộ Studio, 3: Căn hộ Officetel, 4: Căn hộ Shophouse, 5: Căn hộ Penthouse, 6: Căn hộ Duplex, 
        /// 7: Căn hộ Sky Villa, 8: Nhà ở nông thôn, 9: Biệt thự nhà ở, 10: Liền kề, 11: Chung cư thấp tầng, 12: Căn Shophouse, 13: Biệt thự nghỉ dưỡng, 14: Villa)
        /// <see cref="RstClassifyType"/>
        /// </summary>
        public int ClassifyType { get; set; }
        /// <summary>
        /// tổng số lượng căn
        /// </summary>
        public int TotalProductItem { get; set; }
        /// <summary>
        /// số lượng căn đã bán
        /// </summary>
        public int TotalProductItemSell { get; set; }
    }
}
