using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstSellingPolicy
{
    public class FilterRstSellingPolicyTempDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Tên chính sách
        /// </summary>
        private string _name;
        [FromQuery(Name = "name")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        /// <summary>
        /// Mã chính sách
        /// </summary>
        private string _code;
        [FromQuery(Name = "code")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [FromQuery(Name = "createdDate")]
        public DateTime? CreatedDate { get; set; }
    }
}
