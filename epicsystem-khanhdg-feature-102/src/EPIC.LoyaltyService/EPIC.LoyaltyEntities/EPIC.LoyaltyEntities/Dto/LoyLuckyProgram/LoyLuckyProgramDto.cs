using EPIC.LoyaltyEntities.Dto.LoyHistoryUpdate;
using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyProgram
{
    public class LoyLuckyProgramDto
    {
        public int Id { get; set; }

        public int TradingProviderId { get; set; }

        /// <summary>
        /// Mã chương trình
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên chương trình
        /// </summary>
        public string Name { get; set; }

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
        /// Cài đặt thời gian tham gia: null thì không cài
        /// 1. Tính theo mốc thời gian
        /// 2. Cài đặt thời gian tham gia
        /// </summary>
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

        /// <summary>
        /// Trạng thái
        /// <see cref="LoyLuckyProgramStatus"/>
        /// </summary>
        public int Status { get; set; }

        public List<LoyHistoryUpdateDto> HistoryUpdates { get; set; }
    }
}
