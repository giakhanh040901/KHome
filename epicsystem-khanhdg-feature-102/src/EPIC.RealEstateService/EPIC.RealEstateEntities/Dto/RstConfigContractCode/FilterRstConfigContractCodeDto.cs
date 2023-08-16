using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstConfigContractCode
{
    public class FilterRstConfigContractCodeDto : PagingRequestBaseDto
    {
        private string _status;
        [FromQuery(Name = "status")]
        public string Status
        {
            get => _status;
            set => _status = value?.Trim();
        }

        /// <summary>
        /// 1. Lọc hợp đồng theo đại lý, 2. Lọc hợp đồng của đối tác
        /// </summary>
        [FromQuery(Name = "type")]
        public int? Type { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [FromQuery(Name = "createdDate")]
        public DateTime? CreatedDate { get; set; }
    }
}
