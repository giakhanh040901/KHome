using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.SaleInvestor
{
    public class InvestorInfoBySaleDto
    {
        public int InvestorId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public string AvatarImageUrl { get; set; }

        /// <summary>
        /// Mã giới thiệu của nhà đầu tư
        /// </summary>
        public string ReferralCode { get; set; }
        
        public DateTime? CreatedDate { get; set; }

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
