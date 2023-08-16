using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EPIC.Entities.Dto.Sale
{
    /// <summary>
    /// Thông tin nhân sự sale trong đại lý
    /// </summary>
    public class AppPersonnelSaleInfoDto
    {
        public int? Index { get; set; }
        public int SaleId { get; set; }
        
        public string Phone { get; set; }

        public string Email { get; set; }
        public string DepartmentName { get; set; }
        /// <summary>
        /// Tên của sale
        /// </summary>
        public string SaleName { get; set; }

        /// <summary>
        /// Loại sale
        /// </summary>
        public int SaleType { get; set; }

        /// <summary>
        /// Anh đại diện của Sale
        /// </summary>
        public string AvatarImageUrl { get; set; }
        /// <summary>
        /// Mã giới thiệu của Sale
        /// </summary>
        public string ReferralCode { get; set; }
        /// <summary>
        /// Ngày bắt đầu tham gia vào đại lý
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Tổng doanh số của sale trong Invest
        /// </summary>
        public decimal InvestTotalValueMoney { get; set; }

        /// <summary>
        /// Tổng doanh số của sale trong Garner
        /// </summary>
        public decimal GarnerTotalValueMoney { get; set; }
        /// <summary>
        /// Tổng doanh số của sale trong RealEstate
        /// </summary>
        public decimal RealEstateTotalValueMoney { get; set; }

    }
}
