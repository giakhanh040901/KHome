using Microsoft.VisualBasic;
using System;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyProgramInvestor
{
    public class AppLoyLuckyProgramDetailDto
    {
        /// <summary>
        /// Id lịch sử trúng thưởng
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên giải thưởng
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ảnh đại diện của voucher
        /// </summary>
        public string VoucherAvatar { get; set; }

        /// <summary>
        /// Ngày trúng thưởng
        /// </summary>
        public DateTime? CreatedDate { get; set; }
    }
}
