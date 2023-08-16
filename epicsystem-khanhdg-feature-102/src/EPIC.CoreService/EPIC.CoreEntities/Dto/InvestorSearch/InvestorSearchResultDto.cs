namespace EPIC.CoreEntities.Dto.InvestorSearch
{
    /// <summary>
    /// Kết quả search toàn bộ hệ thống cho khách hàng cá nhân
    /// </summary>
    public interface InvestorSearchResultDto
    {
        /// <summary>
        /// Loại sản phẩm (1. Garner, 2.Invest, 4. RST, 6.Event)
        /// </summary>
        public int SystemProductType { get; set; }
    }
}
