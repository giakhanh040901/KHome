using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.WhiteListIp
{
    public class FilterWhiteListIpDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "tradingProviderId")]
        public int? TradingProviderId { get; set; }
    }
}
