using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EPIC.GarnerEntities.Dto.GarnerDistribution
{
    public class FilterGarnerDistributionDto : PagingRequestBaseDto
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

        private string _isClose;
        [FromQuery(Name = "isClose")]
        public string IsClose
        {
            get => _isClose;
            set => _isClose = value?.Trim();
        }

        [FromQuery(Name = "status")]
        public int? Status { get; set; }
    }
}
