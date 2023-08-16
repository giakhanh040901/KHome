using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    /// <summary>
    /// Thông tin các căn hộ tương tự
    /// </summary>
    public class AppRstProductItemSimilarDto
    {
        /// <summary>
        /// Mã dự án
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// Mã căn hộ
        /// </summary>
        public string ProductItemCode { get; set; }

        /// <summary>
        /// Thời gian đã được giao dịch
        /// </summary>
        public DateTime TradingDate { get; set; }

        /// <summary>
        /// Danh sách các căn hộ tương tự
        /// </summary>
        public List<AppRstProductItemSimilarDetailDto> ProductItemSimilars { get; set; }
    }
}
