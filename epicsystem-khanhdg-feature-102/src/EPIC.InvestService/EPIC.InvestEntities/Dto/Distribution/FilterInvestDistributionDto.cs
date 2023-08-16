using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Distribution
{
    public class FilterInvestDistributionDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Status
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        [FromQuery(Name = "isActive")]
        public bool? IsActive { get; set; }

        private string _isClose { get; set; }
        [FromQuery(Name = "isClose")]
        public string IsClose
        {
            get => _isClose;
            set => _isClose = value?.Trim();
        }



        /// <summary>
        /// id đại lý
        /// </summary>
        [FromQuery(Name = "tradingProviderId")]
        public int? TradingProviderId { get; set; }

        /// <summary>
        /// List id đại lý
        /// </summary>
        [FromQuery(Name = "tradingProviderIds")]
        public List<int> TradingProviderIds { get; set; }
    }
}
