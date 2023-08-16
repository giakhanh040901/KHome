using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstExportExcel
{
    public class RstListSyntheticMoneyProjectDto
    {
        /// <summary>
        /// Mã dự án
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Mật độ xây dựng
        /// </summary>
        public string BuildingDensityType { get; set; }
        /// <summary>
        /// Phân loại sản phẩm 
        /// </summary>
        public string ClassifyType { get; set; }
        /// <summary>
        /// trạng thái của sản phẩm
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// số lượng căn hộ
        /// </summary>
        public int Total { get; set; }
        public int TradingProviderId { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
