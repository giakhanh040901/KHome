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
    public class ViewSaleRegisterDto
    {
		public int Id { get; set; }
		public int InvestorId { get; set; }
		public int SaleManagerId { get; set; }
		public int? InvestorBankAccId { get; set; }
		public int? Status { get; set; }
		public InvestorDto Investor { get; set; }
		/// <summary>
		/// Quản lý là nhà đầu tư cá nhân
		/// </summary>
		public InvestorDto SaleInvestorManager { get; set; }
		/// <summary>
		/// Quản lý là khách hàng doanh nghiệp
		/// </summary>
        public BusinessCustomerDto SaleBusinessCustomerManager { get; set; }
		public DepartmentDto Department { get; set; }
	}
}
