using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Investor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Department
{
    public class DepartmentSaleDto
    {
        public int SaleId { get; set; }
        public int? InvestorId { get; set; }
        public int? BusinessCustomerId { get; set; }
        public int? SaleType { get; set; }
        public string Status { get; set; }
        public string EmployeeCode { get; set; }
        public string DepartmentName { get; set; }
        public int? DepartmentId { get; set; }
        public int? SaleParentId { get; set; }
        public string ReferralCode { get; set; }
        public string ContractCode { get; set; }
        public int? InvestorBankAccId { get; set; }
        public int? BusinessCustomerBankAccId { get; set; }
        public DateTime CreatedDate { get; set; }
        public InvestorDto Investor { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
    }
}
