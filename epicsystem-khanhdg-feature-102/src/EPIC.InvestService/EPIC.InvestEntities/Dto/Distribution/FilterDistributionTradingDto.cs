using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Distribution
{
    public class FilterDistributionTradingDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "status")]
        public int? Status { get; set; }
        [FromQuery(Name = "status")]
        private string _isShowApp { get; set; }
        [FromQuery(Name = "isShowApp")]
        public string IsShowApp
        {
            get => _isShowApp;
            set => _isShowApp = value?.Trim();
        }
    }
}
