using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EPIC.GarnerEntities.Dto.GarnerProduct
{
    /// <summary>
    /// Bộ lọc của dự án
    /// </summary>
    public class FilterGarnerProductDto : PagingRequestBaseDto
    {
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

        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        [FromQuery(Name = "productType")]
        public int? ProductType { get; set; }

        [FromQuery(Name = "issuerId")]
        public int? IssuerId { get; set; }
    }
}
