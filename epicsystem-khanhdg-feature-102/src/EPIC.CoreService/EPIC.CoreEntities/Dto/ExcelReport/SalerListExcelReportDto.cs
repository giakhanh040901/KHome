using Microsoft.Extensions.Primitives;
//using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ExcelReport
{
    public class SalerListExcelReportDto
    {
        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Mã saler
        /// </summary>
        public string SalerCode { get; set; }

        /// <summary>
        /// Tên nhân viên sale
        /// </summary>
        public string SaleName { get; set; }

        /// <summary>
        /// Giới tính của sale
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Email của sale
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Vai trò của sale
        /// </summary>
        public string SaleType { get; set; }


        /// <summary>
        /// Ngày duyệt hợp đồng
        /// </summary>
        public DateTime? ApproveContractDate { get; set; }

        /// <summary>
        /// Nguồn đăng kí
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Trạng thái hoạt đồng của sale
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Ngày nghỉ việc
        /// </summary>
        public DateTime? DeactiveDate { get; set; }

        /// <summary>
        /// Vùng
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Tên của quản lý sale
        /// </summary>
        public string SaleManagerName { get; set; }

        /// <summary>
        /// Mã quản lý
        /// </summary>
        public string SaleManagerCode { get; set; }


        /// <summary>
        /// Địa chỉ phòng ban
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Người quản lý trực tiếp
        /// </summary>
        public string ManagerName { get; set; }

        /// <summary>
        /// Mã người quản lý trực tiếp
        /// </summary>
        public string ManagerCode { get; set; }

        /// <summary>
        /// Tên người quản lý cấp 1 ( Khu vực )
        /// </summary>
        public string ManagerNameLv1 { get; set; }

        /// <summary>
        /// Mã người quản lý cấp 1 ( Khu vực )
        /// </summary>
        public string ManagerCodeLv1 { get; set; }

        /// <summary>
        /// Tên người quản lý cấp 2 (Vùng)
        /// </summary>
        public string ManagerNameLv2 { get; set; }

        /// <summary>
        /// Mã người quản lý cấp 2 (Vùng)
        /// </summary>
        public string ManagerCodeLv2 { get; set; }

        /// <summary>
        /// Tên người quản lý cấp 3 (Miền)
        /// </summary>
        public string ManagerNameLv3 { get; set; }

        /// <summary>
        /// Tên người quản lý cấp 3 (Miền) 
        /// </summary>
        public string ManagerCodeLv3 { get; set; }

        /// <summary>
        /// Tên người quản lý cấp 4 
        /// </summary>
        public string ManagerNameLv4 { get; set; }

        /// <summary>
        /// Mã người quản lý cấp 4
        /// </summary>
        public string ManagerCodeLv4 { get; set; }

        /// <summary>
        /// Số giấy tờ
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// Kiểu giấy tờ
        /// </summary>
        public string IdType { get; set; }

        /// <summary>
        /// Địa chỉ liên hệ
        /// </summary>
        public string ContractAddress { get; set; }

        /// <summary>
        /// Doanh số bán hàng của saler
        /// </summary>
        public decimal DoanhSo { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string Sdt { get; set; }

        public string SaleManagerReferralCode { get; set; }
    }
}
