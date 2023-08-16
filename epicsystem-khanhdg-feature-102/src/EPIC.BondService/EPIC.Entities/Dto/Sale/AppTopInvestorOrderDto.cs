using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    /// <summary>
    /// Top nhà đầu tư đặt lệnh
    /// </summary>
    public class AppTopInvestorOrderDto
    {
        /// <summary>
        /// Top thứ tự nhà đầu tư nếu doanh số TotalValueMoney = 0 thì null
        /// </summary>
        public int? Index { get; set; }
        public int InvestorId { get; set; }
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
        public string ReferralCodeSelf { get; set; }
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
