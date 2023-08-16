using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDistributionProductItem
{
    public class FilterRstDistributionProductItemByTradingDto
    {
        /// <summary>
        /// Dự án
        /// </summary>
        [FromQuery(Name = "projectId")]
        public int ProjectId { get; set; }

        /// <summary>
        /// Loại sổ đỏ
        /// </summary>
        [FromQuery(Name = "redBookType")]
        public int? RedBookType { get; set; }

        /// <summary>
        /// Mật độ xây dựng
        /// </summary>
        [FromQuery(Name = "buildingDensityId")]
        public int? BuildingDensityId { get; set; }

        private string _keyword;
        [FromQuery(Name = "keyword")]
        public string Keyword
        {
            get => _keyword;
            set => _keyword = value?.Trim();
        }
    }
}
