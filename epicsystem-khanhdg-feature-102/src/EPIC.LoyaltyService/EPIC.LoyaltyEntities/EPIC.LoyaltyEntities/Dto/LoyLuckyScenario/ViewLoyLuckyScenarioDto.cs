using EPIC.Utils.ConstantVariables.Loyalty;
using System;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyScenario
{
    public class ViewLoyLuckyScenarioDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Tên kịch bản
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tổng số quà tặng trong LoyLuckyScenarioDetail
        /// </summary>
        public int GiftQuantity { get; set; }

        /// <summary>
        /// Tổng giá trị quà tặng được lấy từ giá trị của voucherId trong LoyLuckyScenarioDetail
        /// </summary>
        public decimal TotalValueGift { get; set; }

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
    }
}
