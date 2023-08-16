using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.BlockadeLiberation
{
    public class FilterBlockadeLiberationDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        [FromQuery(Name = "type")]
        public int? Type { get; set; }
        /// <summary>
        /// List id đại lý
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
