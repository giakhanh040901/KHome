using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerProductOverview
{
    public class AppProductOverviewDto
    {
        /// <summary>
        /// Loại nội dung tổng quan
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Nội dung tổng quan sản phẩm
        /// </summary>
        public string OverviewContent { get; set; }

        /// <summary>
        /// Url hình ảnh tổng quan
        /// </summary>
        public string OverviewImageUrl { get; set; }

        /// <summary>
        /// Danh sách tổ chức
        /// </summary>
        public List<AppProductOverviewOrgDto> Organization { get; set; }

        /// <summary>
        /// Hồ sơ pháp lý
        /// </summary>
        public List<AppProductOverviewFileDto> LegalRecords { get; set; }

        /// <summary>
        ///Thông tin phân phối
        /// </summary>
        public List<AppProductOverviewFileDto> DistributionInfo { get; set; }
    }
}
