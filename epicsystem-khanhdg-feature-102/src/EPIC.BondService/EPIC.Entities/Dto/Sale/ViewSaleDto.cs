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
    public class ViewSaleDto
    {
        public int SaleId { get; set; }
        public int? InvestorId { get; set; }
        public int? BusinessCustomerId { get; set; }
        public string SaleName { get; set; }
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
        public DateTime? SaleTradingCreatedDate { get; set; }
        public InvestorDto Investor { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
        public DepartmentDto Department { get; set; }

        /// <summary>
        /// Trạng thái Sale trong đại lý
        /// </summary>
        public string SaleTradingStatus { get; set; }

        /// <summary>
        /// Ngày DeactiveDate Sale trong phòng ban
        /// </summary>
        public DateTime? DeactiveDate { get; set; }
        /// <summary>
        /// Quản lý của phòng ban
        /// </summary>
        public int? ManagerDepartmentId { get; set; }

        /// <summary>
        /// Tên quản lý của phòng ban
        /// </summary>
        public string ManagerDepartmentName { get; set; }

        /// <summary>
        /// Tự động điều hướng
        /// </summary>
        public string AutoDirectional { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string Fullname { get; set; }
    }
}
