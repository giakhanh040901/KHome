using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ExportData
{
    public class ExportDataSaleDto
    {
        public int SaleId { get; set; }
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Mã tư vấn viên
        /// </summary>
        public string ReferralCodeSelf { get; set; }

        /// <summary>
        /// Họ và tên sale
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// Loại sale
        /// </summary>
        public int SaleType { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Phòng ban cấp 1
        /// </summary>
        public string DepartmentName1 { get; set; }
        /// <summary>
        /// Phòng ban cấp 2
        /// </summary>
        public string DepartmentName2 { get; set; }
        /// <summary>
        /// Phòng ban cấp 3
        /// </summary>
        public string DepartmentName3 { get; set; }
        /// <summary>
        /// Phòng ban cấp 4
        /// </summary>
        public string DepartmentName4 { get; set; }
    }
}
