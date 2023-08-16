using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    public class FilterRstProductItemCanDistributionDto
    {
        /// <summary>
        /// Id của phân phối được chọn để phân phối sản phẩm
        /// </summary>
        [FromQuery(Name = "distributionId")]
        public int DistributionId { get; set; }

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

        /// <summary>
        /// Trạng thái của căn hộ
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        private string _keyword;
        [FromQuery(Name = "keyword")]
        public string Keyword
        {
            get => _keyword;
            set => _keyword = value?.Trim();
        }
    }
}
