using EPIC.Entities.Dto.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.SaleAppStatistical
{
    public class SaleInfoAppDto
    {
        public int? Index { get; set; }
        public int SaleId { get; set; }

        public int? InvestorId { get; set; }
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
        /// Mã nhân viên
        /// </summary>
        public string EmployeeCode { get; set; }
        /// <summary>
        /// Ngày bắt đầu tham gia vào đại lý
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Doanh số
        /// </summary>
        public decimal? Sales { get; set; }

        /// <summary>
        /// Số dư
        /// </summary>
        public decimal? Surplus { get; set; }

        /// <summary>
        /// Số hợp đồng
        /// </summary>
        public int TotalContract { get; set; }

        private string _status;
        public string Status 
        { 
            get => _status; 
            set => _status = value?.Trim();
        }
    }
}
