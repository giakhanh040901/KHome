using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.SaleInvestor
{
    public class ViewInvestorsBySaleDto
    {
        public int? Index { get; set; }
        public int InvestorId { get; set; }
        public string Phone { get; set; }
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
        /// <summary>
        /// Doanh số
        /// </summary>
        public decimal TotalValueMoney { get; set; }

        /// <summary>
        /// Số dư
        /// </summary>
        public decimal Balance { get; set; }

        public string Status { get; set; }
        public int TotalContract { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
