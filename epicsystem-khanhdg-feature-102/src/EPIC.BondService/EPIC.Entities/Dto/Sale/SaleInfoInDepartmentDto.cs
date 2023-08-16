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
    /// <summary>
    /// Xem thông tin Sale trong phòng ban của đại lý
    /// </summary>
    public class SaleInfoInDepartmentDto
    {
        public int SaleId { get; set; }
        public int? InvestorId { get; set; }
        public int? BusinessCustomerId { get; set; }

        /// <summary>
        /// Loại Sale
        /// </summary>
        public int SaleType { get; set; }

        /// <summary>
        /// Trạng thái Sale trong đại lý
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string EmployeeCode { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        
        public int? SaleParentId { get; set; }

        /// <summary>
        /// Mã giới thiệu
        /// </summary>
        public string ReferralCode { get; set; }

        /// <summary>
        /// Ngày ký vào đại lý
        /// </summary>
        public DateTime? SignDate { get; set; }

        /// <summary>
        /// Ngày quản lý phòng ban
        /// Nếu Sale là quản lý phòng ban
        /// </summary>
        public DateTime? ManagerStartDate { get; set; }

        /// <summary>
        /// Ngày vào phòng ban
        /// </summary>
        public DateTime? StartInDepartmentDate { get; set; }
    }
}
