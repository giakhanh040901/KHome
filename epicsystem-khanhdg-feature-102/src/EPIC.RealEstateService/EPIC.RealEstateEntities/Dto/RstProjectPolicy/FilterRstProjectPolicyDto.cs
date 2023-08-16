using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectPolicy
{
    public class FilterRstProjectPolicyDto : PagingRequestBaseDto
    {
        private string _status;
        [FromQuery(Name = "status")]
        public string Status 
        { 
            get => _status; 
            set => _status = value?.Trim(); 
        }

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
        /// Mã dự án
        /// </summary>
        [FromQuery(Name = "projectId")]
        public int? ProjectId { get; set; }

        /// <summary>
        /// Loại chính sách (1: Chính sách CĐT quà tặng, 2:Chính sách CĐT thanh toán sớm, 3: Chính sách CĐT mua nhiều, 4: Chính sách CĐT ngoại giao
        /// </summary>
        [FromQuery(Name = "policyType")]
        public int? PolicyType { get; set; }

        /// <summary>
        /// 1: ONLINE, 2:OFFLINE, 3:ALL
        /// </summary>
        [FromQuery(Name = "source")]
        public int? Source { get; set; }
    }
}
