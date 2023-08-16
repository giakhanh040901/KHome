using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    public class FilterProductItemProjectUtilityDto : PagingRequestBaseDto
    {
        private string _status;
        /// <summary>
        /// Trạng thái
        /// </summary>
        [FromQuery(Name = "status")]
        public string Status
        {
            get => _status;
            set => _status = value?.Trim();
        }

        private string _selected;
        /// <summary>
        /// Trạng thái
        /// </summary>
        [FromQuery(Name = "selected")]
        public string Selected
        {
            get => _selected;
            set => _selected = value?.Trim();
        }

        private string _name;
        /// <summary>
        /// Tên tiện ích
        /// </summary>
        [FromQuery(Name = "name")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }
        /// <summary>
        /// Mã tiện ích
        /// </summary>
        [FromQuery(Name = "utilityId")]
        public int? UtilityId { get; set; }
        /// <summary>
        /// Mã căn hộ
        /// </summary>
        [FromQuery(Name = "productItemId")]
        public int ProductItemId { get; set; }
    }
}
