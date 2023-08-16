using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvestProject
{
    public class FilterInvestProjectDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "status")]
        public int? Status { get; set; }
        
        [FromQuery(Name = "tradingProviderId")]
        public int? TradingProviderId { get; set; }

        [FromQuery(Name = "partnerId")]
        public int? PartnerId { get; set; }
    }
}
