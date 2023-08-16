using EPIC.LoyaltyEntities.Dto.LoyLuckyRotationInterface;
using EPIC.LoyaltyEntities.Dto.LoyLuckyScenarioDetail;
using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyScenario
{
    public class LoyLuckyScenarioDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Tên kịch bản
        /// </summary>
        public string Name { get; set; }

        public string AvatarImageUrl { get; set; }

        /// <summary>
        /// Số lượng giải thưởng
        /// </summary>
        public int? PrizeQuantity { get; set; }
        /// <summary>
        /// Loại kịch bản: 1 Vòng quay may mắn, 2: Tích điểm đổi quà, 3: Rơi quà may mắn
        /// <see cref="LoyLuckyScenarioTypes"/>
        /// </summary>
        public int LuckyScenarioType { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Trạng thái A/D
        /// </summary>
        public string Status { get; set; }

        public List<LoyLuckyScenarioDetailDto> LuckyScenarioDetails { get; set; }
        public LoyLuckyRotationInterfaceDto LuckyRotationInterface { get; set; }
    }
}
