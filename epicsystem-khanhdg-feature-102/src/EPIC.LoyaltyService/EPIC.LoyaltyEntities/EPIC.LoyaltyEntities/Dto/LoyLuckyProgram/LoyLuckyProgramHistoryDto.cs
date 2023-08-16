using System;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyProgram
{
    public class LoyLuckyProgramHistoryDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Số điện thọai
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Tên chương trình
        /// </summary>
        public string LuckyProgramName { get; set; }

        /// <summary>
        /// Qùa tặng tham gia
        /// </summary>
        public string GiftName { get; set; }

        /// <summary>
        /// Thời gian trúng thưởng
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Có trúng thưởng không
        /// </summary>
        public bool IsPrize { get; set; }
    }
}
