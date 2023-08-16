using EPIC.Entities.Dto.Investor;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.Sale
{
    public class SaleFilterDto
    {
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; }

        [FromQuery(Name = "pageNumber")]
        public int PageNumber { get; set; }

        private string _saleTypes;
        [FromQuery(Name = "saleTypes")]
        public string SaleTypes
        {
            get => _saleTypes;
            set => _saleTypes = value?.Trim();
        }
        [FromQuery(Name = "departmentId")]
        public int DepartmentId { get; set; }

        [FromQuery(Name = "investorId")]
        public int? InvestorId { get; set; }

        [FromQuery(Name = "businessCustomerId")]
        public int? BusinessCustomerId { get; set; }

        [FromQuery(Name = "saleType")]
        public int? SaleType { get; set; }

        private string _fullName;
        [FromQuery(Name = "fullName")]
        public string FullName
        {
            get => _fullName;
            set => _fullName = value?.Trim();
        }
        private string _status;
        [FromQuery(Name = "status")]
        public string Status
        {
            get => _status;
            set => _status = value?.Trim();
        }
        private string _employeeCode;
        [FromQuery(Name = "employeeCode")]
        public string EmployeeCode
        {
            get => _employeeCode;
            set => _employeeCode = value?.Trim();
        }

        private string _phone;
        [FromQuery(Name = "phone")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }
        private string _idNo;
        [FromQuery(Name = "idNo")]
        public string IdNo
        {
            get => _idNo;
            set => _idNo = value?.Trim();
        }
        private string _taxCode;
        [FromQuery(Name = "taxCode")]
        public string TaxCode
        {
            get => _taxCode;
            set => _taxCode = value?.Trim();
        }
        private string _referralCode;
        [FromQuery(Name = "referralCode")]
        public string ReferralCode
        {
            get => _referralCode;
            set => _referralCode = value?.Trim();
        }
        [FromQuery(Name = "isInvestor")]
        public bool? IsInvestor { get; set; } = null;
        private string _keyword;
        [FromQuery(Name = "keyword")]
        public string Keyword
        {
            get => _keyword;
            set => _keyword = value?.Trim();
        }
        public int Skip
        {
            get
            {
                int skip = (PageNumber - 1) * PageSize;
                if (skip < 0)
                {
                    skip = 0;
                }
                return skip;
            }
        }
    }
}
