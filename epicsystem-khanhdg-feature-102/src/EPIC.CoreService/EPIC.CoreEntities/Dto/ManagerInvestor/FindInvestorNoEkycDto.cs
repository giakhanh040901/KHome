using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ManagerInvestor
{
    public class FindInvestorNoEkycDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "tradingProviderId")]
        public int? TradingProviderId { get; set; }

        /// <summary>
        /// Truyền dạng 1,2
        /// </summary>
        [FromQuery(Name = "step")]
        public string Step { get; set; }

        /// <summary>
        /// Truyền dạng A,T
        /// </summary>
        [FromQuery(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Truyền dạng 1,2
        /// </summary>
        [FromQuery(Name = "source")]
        public string Source { get; set; }
    }
}
