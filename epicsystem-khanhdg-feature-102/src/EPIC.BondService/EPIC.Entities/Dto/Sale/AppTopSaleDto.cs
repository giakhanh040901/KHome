using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    /// <summary>
    /// Top sale có doanh số cao nhất
    /// </summary>
    public class AppTopSaleDto
    {
        /// <summary>
        /// Top của Sale, nếu chưa có doanh số = null
        /// </summary>
        public int? Index { get; set; }
        public int SaleId { get; set; }
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public string AvatarImageUrl { get; set; }

        /// <summary>
        /// Loại sale : 1 Quản lý, 2 Nhân viên, 3 CTV
        /// </summary>
        public int SaleType { get; set; }

        /// <summary>
        /// Ngày vào đại lý
        /// </summary>
        public DateTime? SignDate { get; set; }

        /// <summary>
        /// Mã giới thiệu 
        /// </summary>
        public string ReferralCode { get; set; }
        /// <summary>
        /// Doanh số
        /// </summary>
        public decimal TotalValueMoney { get; set; }

        /// <summary>
        /// Số dư
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// Tổng hợp đồng 
        /// </summary>
        public int TotalContract { get; set; }
    }
}
