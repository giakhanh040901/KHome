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
    public class FilterSaleTempDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "saleType")]
        public int? SaleType { get; set; }
        [FromQuery(Name = "status")]
        public int? Status { get; set; }
        [FromQuery(Name = "isInvestor")]
        public int? IsInvestor { get; set; }
        [FromQuery(Name = "source")]
        public int? Source { get; set; }
        private string _employeeCode { get; set; }
       
        [FromQuery(Name = "employeeCode")]
        public string EmployeeCode
        {
            get => _employeeCode;
            set => _employeeCode = value?.Trim();
        }

        private string _phone { get; set; }
       
        [FromQuery(Name = "phone")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }

        private string _email { get; set; }

        [FromQuery(Name = "email")]
        public string Email
        {
            get => _email;
            set => _email = value?.Trim();
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

        private string _taxCode { get; set; }

        [FromQuery(Name = "taxCode")]
        public string TaxCode
        {
            get => _taxCode;
            set => _taxCode = value?.Trim();
        }

      
        [FromQuery(Name = "tradingProviderId")]
        public int? TradingProviderId { get; set; }


    }
}
