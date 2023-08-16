using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDistributionPolicy
{
    public class FilterDistrobutionPolicyDto
    {
        [FromQuery(Name = "openSellId")]
        public int OpenSellId { get; set; }
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
    }
}
