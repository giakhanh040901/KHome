using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDistributionPolicyTemp
{
    /// <summary>
    /// Bộ lọc
    /// </summary>
    public class FilterRstDistributionPolicyTempDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Lọc theo trạng thái
        /// </summary>
        private string _status { get; set; }
        [FromQuery(Name = "status")]
        public string Status
        {
            get => _status;
            set => _status = value?.Trim();
        }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [FromQuery(Name = "createdDate")]
        public DateTime? CreatedDate { get; set; }
    }
}
