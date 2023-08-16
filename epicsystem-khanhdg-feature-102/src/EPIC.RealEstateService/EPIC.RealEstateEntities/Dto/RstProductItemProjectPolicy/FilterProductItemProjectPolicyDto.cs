using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItemProjectPolicy
{
    public class FilterProductItemProjectPolicyDto : PagingRequestBaseDto
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

        private string _code;
        /// <summary>
        /// Mã chính sách
        /// </summary>
        [FromQuery(Name = "code")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        private string _name;
        /// <summary>
        /// Tên chính sách
        /// </summary>
        [FromQuery(Name = "name")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }
        [FromQuery(Name = "policyType")]
        public int? PolicyType { get; set; }

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

        [FromQuery(Name = "productItemId")]
        public int ProductItemId { get; set; }
    }
}
