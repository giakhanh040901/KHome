using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Department;
using EPIC.Entities.Dto.Investor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class SaleTempDto
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
		public InvestorDto Investor { get; set; }
		public BusinessCustomerDto BusinessCustomer { get; set; }
		public DepartmentDto Department { get; set; }

		/// <summary>
		/// Tên sale quản lý
		/// </summary>
		public string SaleManagerName { get; set; }
		public string Fullname { get; set; }
    }
}
