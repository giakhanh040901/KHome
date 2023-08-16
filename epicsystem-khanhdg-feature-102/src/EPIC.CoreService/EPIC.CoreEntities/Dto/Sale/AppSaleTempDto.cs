using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.Sale
{
    public class AppSaleTempDto
    {
        public int Id { get; set; }
        public int? InvestorId { get; set; }
        public int? BusinessCustomerId { get; set; }
        public int TradingProviderId { get; set; }
        public int? DepartmentId { get; set; }
        public int? Source { get; set; }
        public int? Status { get; set; }
        public string EmployeeCode { get; set; }
        public int? SaleType { get; set; }
        public int? SaleParentId { get; set; }
        public int? InvestorBankAccId { get; set; }
        public int? BusinessCustomerBankAccId { get; set; }
    }
}
