using EPIC.LoyaltyEntities.Dto.LoyLuckyProgramInvestor;
using EPIC.LoyaltyEntities.Dto.LoyLuckyRotationInterface;
using EPIC.LoyaltyEntities.Dto.LoyLuckyScenarioDetail;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyScenario
{
    public class AppLoyLuckyScenarioDto
    {
        /// <summary>
        /// Id chương trình
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id kịch bản
        /// </summary>
        public int LuckyScenarioId { get; set; }

        /// <summary>
        /// Tên chương trình
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mã chương trình
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Thời gian kết thúc
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Loại nội dung: MARKDOWN, HTML
        /// </summary>
        public string DescriptionContentType { get; set; }

        /// <summary>
        /// Nội dung mô tả
        /// </summary>
        public string DescriptionContent { get; set; }

        /// <summary>
        /// Ảnh đại diện chương trình
        /// </summary>
        public string AvatarImageUrl { get; set; }

        /// <summary>
        /// Tên kịch bản
        /// </summary>
        public string LuckyScenarioName { get; set; }

        /// <summary>
        /// Ảnh đại diện của kịch bản
        /// </summary>
        public string LuckyScenarioAvatarImageUrl { get; set; }

        /// <summary>
        /// Số lượng giải thưởng
        /// </summary>
        public int? PrizeQuantity { get; set; }
        /// <summary>
        /// Loại kịch bản: 1 Vòng quay may mắn, 2: Tích điểm đổi quà, 3: Rơi quà may mắn
        /// <see cref="LoyLuckyScenarioTypes"/>
        /// </summary>
        public int LuckyScenarioType { get; set; }

        public IEnumerable<LoyLuckyScenarioDetailDto> LuckyScenarioDetails { get; set; }
        public LoyLuckyRotationInterfaceDto LuckyRotationInterface { get; set; }

        /// <summary>
        /// Chi tiết lịch sử chương trình
        /// </summary>
        public IEnumerable<AppLoyLuckyProgramInvestorDetailDto> LuckyProgramInvestorDetails { get; set; }
    }
}
