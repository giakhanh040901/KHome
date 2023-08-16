using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDashboard
{
    public class DashboardOverViewDto
    {
        /// <summary>
        /// tổng số lượng sản phẩm
        /// </summary>
        public int TotalProductItem { get; set; }
        /// <summary>
        /// dự án đang bán
        /// </summary>
        public int ProjectSell { get ; set; }
        /// <summary>
        /// tỷ lệ sản phẩm đã bán
        /// </summary>
        public decimal Ratio { get; set; }
        /// <summary>
        /// tổng sản phẩm đã bán
        /// </summary>
        public int TotalProductItemSell { get; set; }
        /// <summary>
        /// số lượng khách hàng
        /// </summary>
        public int TotalCustomer { get; set; }
        /// <summary>
        /// số lượng của khách hàng đặt mua, đặt cọc nhiều căn hộ nhất nhiều nhất
        /// </summary>
        public int CustomerMaxsell { get; set; }
    }
}
