using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    /// <summary>
    /// báo cáo excel danh sách saler
    /// </summary>
    public class SalerListExcel
    {
        public int? SaleId { get; set; }
        public int? InvestorId { get; set; }
        public int? BusinessCustomerId { get; set; }

        /// <summary>
        /// Mã investor id của sale quản lý
        /// </summary>
        public int? InvestorManagerId { get; set; }

        /// <summary>
        /// Nguồn đăng kí
        /// </summary>
        public int Source { get; set; }

        /// <summary>
        /// Mã businesscustomer id của sale quản lý
        /// </summary>
        public int? BusinessManagerId { get; set; }

        /// <summary>
        /// Vai trò của thằng sale
        /// </summary>
        public int? SaleType { get; set; }

        /// <summary>
        /// Ngày nghỉ việc
        /// </summary>
        public DateTime? DeactiveDate { get; set; }
        /// <summary>
        /// Khu vực
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Trạng thái hoạt động của sale
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Ngày kí hợp đồng của sale
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string DepartmentName { get; set; }
       
        /// <summary>
        /// Id phòng ban
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Mã saler
        /// </summary>
        public string RefferalCodeSelf { get; set; }

        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Người quản lý trực tiếp
        /// </summary>
        public int SaleParentId { get; set; }

        /// <summary>
        /// Mã sale quản lý
        /// </summary>
        public string SaleManagerCode { get; set; }

        /// <summary>
        /// Mã id phòng ban
        /// </summary>
        public int ParentId { get; set; }
    }
}
