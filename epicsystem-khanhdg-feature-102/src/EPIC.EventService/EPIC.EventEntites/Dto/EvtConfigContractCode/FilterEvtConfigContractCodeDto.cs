using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtConfigContractCode
{
    public class FilterEvtConfigContractCodeDto : PagingRequestBaseDto
    {
        private string _status { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        [FromQuery(Name = "status")]
        public string Status
        {
            get => _status;
            set => _status = value?.Trim();
        }
    }
}
