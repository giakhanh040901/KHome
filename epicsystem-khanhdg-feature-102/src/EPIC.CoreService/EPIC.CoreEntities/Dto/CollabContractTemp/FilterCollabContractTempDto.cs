using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.CollabContractTemp
{
    public class FilterCollabContractTempDto : PagingRequestBaseDto
    {
        private string _type { get; set; }
        /// <summary>
        /// Keyword tìm kiếm
        /// </summary>
        [FromQuery(Name = "type")]
        public string Type
        {
            get => _type;
            set => _type = value?.Trim();
        }

        [FromQuery(Name = "tradigProviderId")]
        public int? TradigProviderId { get; set; }
    }
}
