using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSellDetail
{
    public class FilterRstOpenSellDetailForOrder
    {
        /// <summary>
        /// Tìm theo Id dự án
        /// </summary>
        [FromQuery(Name = "projectId")]
        public int ProjectId { get; set; }

        private string _keyword { get; set; }
        /// <summary>
        /// Tìm kiếm theo mã căn hộ, tên căn hộ
        /// </summary>
        [FromQuery(Name = "keyword")]
        public string Keyword
        {
            get => _keyword;
            set => _keyword = value?.Trim();
        }
    }
}
