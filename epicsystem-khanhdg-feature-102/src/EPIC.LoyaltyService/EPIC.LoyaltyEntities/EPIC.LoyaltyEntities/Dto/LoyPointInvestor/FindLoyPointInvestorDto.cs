using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyPointInvestor
{
    /// <summary>
    /// Thông tin khách hàng
    /// </summary>
    public class FindLoyPointInvestorDto
    {
        public int InvestorId { get; set; }
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }
        /// <summary>
        /// Sdt của khách
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Email của khách
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Tên của khách
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Tên hạng
        /// </summary>
        public string RankName { get; set; }

        /// <summary>
        /// Điểm hiện tại
        /// </summary>
        public int? LoyCurrentPoint { get; set; }

        /// <summary>
        /// Tổng điểm
        /// </summary>
        public int? LoyTotalPoint { get; set; }
    }
}
