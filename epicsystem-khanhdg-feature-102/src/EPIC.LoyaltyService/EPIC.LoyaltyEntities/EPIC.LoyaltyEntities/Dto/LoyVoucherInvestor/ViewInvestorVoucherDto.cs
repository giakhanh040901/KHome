using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor
{
    public class ViewInvestorVoucherDto
    {
        public int InvestorId { get; set; }
        public int? UserId { get; set; }
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }
        /// <summary>
        /// Username của tài khoản
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Sdt của khách
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Email của khách
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Giới tính của khách
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// Tên của khách
        /// </summary>
        public string Fullname { get; set; }
        /// <summary>
        /// Số voucher khách đã đc cấp
        /// </summary>
        public int? VoucherNum { get; set; }

        /// <summary>
        /// Id Hạng
        /// </summary>
        public int? RankId { get; set; }

        /// <summary>
        /// Tên hạng
        /// </summary>
        public string RankName { get; set; }

        /// <summary>
        /// Tài khoản của khách đã được xác thực chưa
        /// </summary>
        public bool IsVerified { get; set; }

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
