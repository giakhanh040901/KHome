using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProject
{
    public class FilterRstProjectDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Chủ đầu tư
        /// </summary>
        [FromQuery(Name = "ownerId")]
        public int? OwnerId { get; set; }

        /// <summary>
        /// Loại hình dự án:  1: Đất đấu giá, 2: Đất BT, 3: Đất giao
        /// </summary>
        [FromQuery(Name = "projectType")]
        public int? ProjectType { get; set; }

        private string _name;
        [FromQuery(Name = "name")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        private string _code;
        [FromQuery(Name = "code")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        /// <summary>
        /// Trạng thái (1: Khoi tao, 2: Cho duyet, 3: Hoat dong, 4: Huy duyet, 5:Dong)
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }
        [FromQuery(Name = "productTypes")]
        public List<int> ProductTypes { get; set; }
    }
}
