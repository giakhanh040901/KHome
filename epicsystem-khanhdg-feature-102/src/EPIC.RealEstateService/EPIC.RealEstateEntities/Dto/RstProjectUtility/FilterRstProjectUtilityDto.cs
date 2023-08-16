using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectUtility
{
    public class FilterRstProjectUtilityDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Lọc theo loại tiện ích
        /// </summary>
        [FromQuery(Name = "type")]
        public int? Type { get; set; }

        /// <summary>
        /// Nhóm tiện ích
        /// </summary>
        [FromQuery(Name = "groupId")]
        public int? GroupId { get; set; }

        
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
        
        private string _isHighlight;
        /// <summary>
        /// Nổi bật
        /// </summary>
        [FromQuery(Name = "isHighlight")]
        public string IsHighlight
        {
            get => _isHighlight;
            set => _isHighlight = value?.Trim();
        }

        private string _isSelected;
        /// <summary>
        /// Chọn
        /// </summary>
        [FromQuery(Name = "isSelected")]
        public string IsSelected
        {
            get => _isSelected;
            set => _isSelected = value?.Trim();
        }
    }
}
