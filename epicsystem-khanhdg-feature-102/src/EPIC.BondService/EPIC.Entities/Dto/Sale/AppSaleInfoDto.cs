using EPIC.Entities.Dto.Investor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class AppSaleInfoDto
    {
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
        public string DepartmentAddress { get; set; }
        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string EmployeeCode { get; set; }
        /// <summary>
        /// Khu vực hoạt động
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// Id Tài khoản ngân hàng thụ hưởng
        /// </summary>
        public int? InvestorBankAccId { get; set; }
        /// <summary>
        /// Đường dẫn Avatar
        /// </summary>
        public string AvatarImageUrl { get; set; }

        /// <summary>
        /// Thông tin của Quản lý
        /// </summary>
        public AppSaleManagerDto SaleManager { get; set; }

        /// <summary>
        /// Ngày ký vào đại lý
        /// </summary>
        public DateTime? SignDate { get; set; }

        /// <summary>
        /// Tự động điều hướng
        /// </summary>
        public string AutoDirectional { get; set; }

        /// <summary>
        /// Danh sách ngân hàng của Sale
        /// </summary>
        public List<InvestorBankAccountDto> InvestorBankAccounts { get; set; }
    }

    public class AppCheckSaler
    {
        public int Status { get; set; }
    }
    public class AppSaleStatusByTrading
    {
        public string Status { get; set; }
    }
}
