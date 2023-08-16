namespace EPIC.CompanySharesEntities.Dto.CpsApp
{
    public class CpsInfoSecondaryFindDto
    {
        public int CpsSecondaryId { get; set; }
        public int TradingProviderId { get; set; }
        public string CpsCode { get; set; }
        public string TradingProviderName { get; set; }
        public decimal? Profit { get; set; }
        public string Icon { get; set; }
    }
}
