using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOwner
{
    public class FilterRstOwnerDto : PagingRequestBaseDto
    {
        private string _status;
        [FromQuery(Name = "status")]
        public string Status
        {
            get => _status;
            set => _status = value?.Trim();
        }

        /// <summary>
        /// Tên doanh nghiệp
        /// </summary>
        private string _name;
        [FromQuery(Name = "name")]
        public string Name 
        { 
            get => _name; 
            set => _name = value?.Trim(); 
        }

        /// <summary>
        /// Mã số thuế
        /// </summary>
        private string _taxCode;
        [FromQuery(Name = "taxCode")]
        public string TaxCode
        {
            get => _taxCode;
            set => _taxCode = value?.Trim();
        }
    }
}
