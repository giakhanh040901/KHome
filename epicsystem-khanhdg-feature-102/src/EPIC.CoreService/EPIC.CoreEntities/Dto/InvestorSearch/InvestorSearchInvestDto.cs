namespace EPIC.CoreEntities.Dto.InvestorSearch
{
    public class InvestorSearchInvestDto : InvestorSearchResultDto
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
        public int SystemProductType { get; set; }
    }
}
