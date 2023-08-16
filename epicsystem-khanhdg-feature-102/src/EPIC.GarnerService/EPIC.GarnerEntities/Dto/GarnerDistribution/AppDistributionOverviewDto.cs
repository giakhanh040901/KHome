using EPIC.GarnerEntities.Dto.GarnerProductOverview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerDistribution
{
    public class AppDistributionOverviewDto : AppProductOverviewDto
    {
        /// <summary>
        /// Loại hình tích lũy
        /// </summary>
        public int ProductType { get; set; }

        /// <summary>
        /// Mã sản phẩm tích lũy
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Tên sản phẩm tích lũy
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Lợi nhuận
        /// </summary>
        public decimal Profit { get; set; }

        /// <summary>
        /// Số tiền tối thiểu: Tích lũy từ
        /// </summary>
        public decimal MinMoney { get; set; }

        /// <summary>
        /// Tổng số người tham gia tích lũy (hợp đồng active)
        /// </summary>
        public int TotalParticipants { get; set; }

        /// <summary>
        /// Tỷ lệ đánh giá
        /// </summary>
        public decimal RatingRate { get; set; }

        /// <summary>
        /// Tổng số người tham giá đánh giá
        /// </summary>
        public int TotalReviewers { get; set; }

        /// <summary>
        /// Mô tả thông tin chính sách
        /// </summary>
        public string Description { get; set; }

    }
}
