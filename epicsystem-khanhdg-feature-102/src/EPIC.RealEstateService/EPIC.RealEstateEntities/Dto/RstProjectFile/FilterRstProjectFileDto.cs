using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectFile
{
    public class FilterRstProjectFileDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Id dự án
        /// </summary>
        [FromQuery(Name = "projectId")]
        public int? ProjectId { get; set; }

        /// <summary>
        /// Loại hình file dự án: 1: Hồ sơ pháp lý
        /// </summary>
        [FromQuery(Name = "projectFileType")]
        public int? ProjectFileType { get; set; }
        
        /// <summary>
        /// Trạng thái: A: kích hoạt, D: đóng
        /// </summary>
        [FromQuery(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Tên file
        /// </summary>
        [FromQuery(Name = "name")]
        public string Name { get; set; }
    }
}
