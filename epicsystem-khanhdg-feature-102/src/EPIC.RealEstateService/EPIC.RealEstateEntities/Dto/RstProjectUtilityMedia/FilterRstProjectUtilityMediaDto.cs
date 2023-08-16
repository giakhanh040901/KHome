using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectUtilityMedia
{
    public class FilterRstProjectUtilityMediaDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "type")]
        public int? Type { get; set; }

        private string _title;
        [FromQuery(Name = "title")]

        public string Title
        {
            get => _title;
            set => _title = value?.Trim();
        }

        private string _status;
        [FromQuery(Name = "Status")]
        public string Status
        {
            get => _status;
            set => _status = value?.Trim();
        }
    }
}
