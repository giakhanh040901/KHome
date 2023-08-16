using EPIC.Utils.Attributes;
using System;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyProgramInvestor
{
    public class AppLoyLuckyProgramByInvestorDto
    {
        /// <summary>
        /// Id của chương trình
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Mã chương trình
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên chương trình
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ảnh đại diện chương trình
        /// </summary>
        public string AvatarImageUrl { get; set; }

        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Thời gian kết thúc
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
