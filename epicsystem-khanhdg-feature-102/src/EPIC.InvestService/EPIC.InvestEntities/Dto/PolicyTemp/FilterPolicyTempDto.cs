using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EPIC.InvestEntities.Dto.PolicyTemp
{
    public class FilterPolicyTempDto : PagingRequestBaseDto
    {
        private string _status { get; set; }
        [FromQuery(Name = "status")]
        public string Status
        {
            get => _status;
            set => _status = value?.Trim();
        }

        [FromQuery(Name = "classify")]
        public decimal? Classify { get; set; }
        [FromQuery(Name = "type")]
        public int? Type { get; set; }
        [FromQuery(Name = "tradingProviderId")]
        public int? TradingProviderId { get; set; }
    }
}
