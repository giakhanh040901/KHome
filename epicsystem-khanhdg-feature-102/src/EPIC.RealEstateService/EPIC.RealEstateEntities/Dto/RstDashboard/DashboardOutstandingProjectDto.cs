using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDashboard
{
    public class DashboardOutstandingProjectDto
    {
        public int Id { get; set; }
        /// <summary>
        /// số lượng sản phẩm đã bán
        /// </summary>
        public int TotalProductItemSell { get; set; }
        /// <summary>
        /// tổng số lượng sản phẩm
        /// </summary>
        public int TotalProductItem { get; set; }
        /// <summary>
        /// tên dự án
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// địa chỉ dự án
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Diện tích đất
        /// </summary>
        public string LandArea { get; set; }
        /// <summary>
        /// Đường dẫn file ảnh dự án
        /// </summary>
        public string UrlImage { get; set; }

    }
}
