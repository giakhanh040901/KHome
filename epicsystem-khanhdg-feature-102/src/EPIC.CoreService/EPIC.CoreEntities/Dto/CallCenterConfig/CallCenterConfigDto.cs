using EPIC.Entities.Dto.TradingProvider;

namespace EPIC.CoreEntities.Dto.CallCenterConfig
{
    /// <summary>
    /// Danh sách userId call center cho app
    /// </summary>
    public class CallCenterConfigDto
    {
        public int Id { get; set; }
        public int? TradingProviderId { get; set; }
        public TradingProviderCallCenterConfigDto TradingProvider { get; set; }
        public int UserId { get; set; }
        public UserCallCenterDto User { get; set; }
        public int SortOrder { get; set; }
    }

    public class TradingProviderCallCenterConfigDto
    {
        public string BusinessCustomerName { get; set; }
        public string AliasName { get; set; }
    }

    public class UserCallCenterDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Fullname { get; set; }
    }
}
