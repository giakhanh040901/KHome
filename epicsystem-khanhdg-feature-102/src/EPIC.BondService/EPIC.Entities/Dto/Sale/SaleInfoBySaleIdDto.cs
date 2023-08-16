using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class SaleInfoBySaleIdDto
    {
        public int? InvestorId { get; set; }
        public int? BusinessCustomerId { get; set; }
        public int SaleId { get; set; }
        /// <summary>
        /// Mã tư vấn của tôi
        /// </summary>
        public string ReferralCode { get; set; }
        /// <summary>
        /// Tên đầy đủ
        /// </summary>
        public string Fullname { get; set; }
        /// <summary>
        /// Loại hình Sale
        /// </summary>
        public int? SaleType { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Tên tổ chức đối tác
        /// </summary>
        public string TradingProviderName { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentAddress { get; set; }
        /// <summary>
        /// Khu vực hoạt động
        /// </summary>
        public string Area { get; set; }
        public string EmployeeCode { get; set; }
        public int? SaleParentId { get; set; }
        public int? ManagerId { get; set; }
        public string AvatarImageUrl { get; set; }

        /// <summary>
        /// Phòng ban cấp trên
        /// </summary>
        public int? DepartmentParentId { get; set; }
        public int? InvestorBankAccId { get; set; }
        public int? BusinessCustomerBankAccId { get; set; }

        /// <summary>
        /// Ngày ký vào đại lý
        /// </summary>
        public DateTime? SignDate { get; set; }

        /// <summary>
        /// Tự động điều hướng
        /// </summary>
        public string AutoDirectional { get; set; }
        public AppSaleManagerDto SaleManager { get; set; }
    }
}
