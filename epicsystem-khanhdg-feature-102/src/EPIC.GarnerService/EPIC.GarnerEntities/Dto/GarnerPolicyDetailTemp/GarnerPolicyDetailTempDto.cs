namespace EPIC.GarnerEntities.Dto.GarnerPolicyDetailTemp
{
    public class GarnerPolicyDetailTempDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public int PolicyTempId { get; set; }
        public int SortOrder { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public decimal Profit { get; set; }
        public int? InterestDays { get; set; }
        public int PeriodQuantity { get; set; }
        public string PeriodType { get; set; }
        public int? InterestType { get; set; }
        public int? InterestPeriodQuantity { get; set; }
        public string InterestPeriodType { get; set; }
        public int? RepeatFixedDate { get; set; }
        public string Status { get; set; }
    }
}
