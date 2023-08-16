namespace EPIC.CoreEntities.Dto.CallCenterConfig
{
    /// <summary>
    /// Danh sách userId call center cho app
    /// </summary>
    public class UserIdCallCenterDto
    {
        public int Id { get; set; }
        public int? TradingProviderId { get; set; }
        public int UserId { get; set; }
        public int SortOrder { get; set; }
    }
}
