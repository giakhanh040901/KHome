using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.Sale
{
    public class FilterSaleRegisterDto : PagingRequestBaseDto
    {
        private string _phone { get; set; }

        [FromQuery(Name = "phone")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }

        private string _idNo { get; set; }

        [FromQuery(Name = "idNo")]
        public string IdNo
        {
            get => _idNo;
            set => _idNo = value?.Trim();
        }

        private string _investorName { get; set; }

        [FromQuery(Name = "investorName")]
        public string InvestorName
        {
            get => _investorName;
            set => _investorName = value?.Trim();
        }

        [FromQuery(Name = "status")]
        public int? Status { get; set; }
    }
}
