using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.Validation;
using System;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyProgram
{
    public class UpdateLuckyLoyProgramSettingDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Cài đặt thời gian tham gia: null thì không cài
        /// 1. Tính theo mốc thời gian
        /// 2. Cài đặt thời gian tham gia
        /// </summary>
        [IntegerRangeAttribute(AllowableValues = new int[] { LoyJoinTimeSettingTypes.THEO_MOC_THOI_GIAN, LoyJoinTimeSettingTypes.THEO_THOI_GIAN_THAM_GIA })]
        public int? JoinTimeSetting { get; set; }

        /// <summary>
        /// Số lượt quay/ Khách hàng
        /// </summary>
        public int? NumberOfTurn { get; set; }

        /// <summary>
        /// Thời gian bắt đầu được quay nếu cài JoinTimeSetting = 1
        /// Nếu JoinTimeSetting = 2 thì Thời gian bắt đầu được quay tính từ lúc khách quay
        /// </summary>
        public DateTime? StartTurnDate { get; set; }

        /// <summary>
        /// Reset lượt quay
        /// </summary>
        public int? ResetTurnQuantity { get; set; }

        /// <summary>
        /// Reset lượt quay theo D, M, Y...
        /// </summary>
        public string ResetTurnType { get; set; }

        /// <summary>
        /// Số lượt tham gia tối đa của mỗi địa chỉ Ip
        /// </summary>
        public int? MaxNumberOfTurnByIp { get; set; }
    }
}
