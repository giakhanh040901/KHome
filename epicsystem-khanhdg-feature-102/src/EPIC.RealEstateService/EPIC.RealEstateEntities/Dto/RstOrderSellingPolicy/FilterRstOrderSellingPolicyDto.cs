using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using EPIC.Utils.ConstantVariables.RealEstate;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrderSellingPolicy
{
    public class FilterRstOrderSellingPolicyDto : PagingRequestBaseDto
    {
        /// <summary>
        /// OrderId
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Lọc theo đối tác (1) hoặc đại lý (2)
        /// <see cref="AppRstPolicyTypes"/>
        /// </summary>
        public int? PolicyType { get; set; }

        private string _isSelected { get; set; }
        [FromQuery(Name = "isSelected")]
        public string IsSelected
        {
            get => _isSelected;
            set => _isSelected = value?.Trim();
        }

        /// <summary>
        /// List id đại lý
        /// </summary>
        [FromQuery(Name = "tradingProviderIds")]
        public List<int> TradingProviderIds { get; set; }
    }
}
