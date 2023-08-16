using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvestProject
{
    /// <summary>
    /// Thông tin chung của dự án đầu tư theo phát hành thứ cấp
    /// </summary>
    public class ProjectDistributionDto
    {
        public int DistributionId { get; set; }
        public int TradingProviderId { get; set; }
        public int ProjectId { get; set; }
        public string InvCode { get; set; }
        public string TradingProviderName { get; set; }
        public decimal? Profit { get; set; }
        /// <summary>
        /// Ảnh logo của dự án
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Ảnh phân phối sản phẩm
        /// </summary>
        public string DistributionImage { get; set; }

        /// <summary>
        /// Số kỳ hạn tối thiểu
        /// </summary>
        public int? MinPeriodQuantity { get; set; }
        /// <summary>
        /// Số kỳ hạn tối đa
        /// </summary>
        public int? MaxPeriodQuantity { get; set; }
        /// <summary>
        /// Loại kỳ hạn tối thiểu D M Y
        /// </summary>
        public string MinPeriodType { get; set; }
        /// <summary>
        /// Loại kỳ hạn tối đa D M Y
        /// </summary>
        public string MaxPeriodType { get; set; }
    }

    public class ProjectDistributionFindDto
    {
        public int DistributionId { get; set; }
        public int TradingProviderId { get; set; }
        public int ProjectId { get; set; }
        public string InvCode { get; set; }
        public string TradingProviderName { get; set; }
        public decimal? Profit { get; set; }
        /// <summary>
        /// Ảnh logo của dự án
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Ảnh phân phối sản phẩm
        /// </summary>
        public string DistributionImage { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
