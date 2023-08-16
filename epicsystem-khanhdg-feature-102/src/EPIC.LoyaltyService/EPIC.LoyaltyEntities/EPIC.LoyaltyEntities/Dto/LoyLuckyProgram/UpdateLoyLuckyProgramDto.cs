using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyProgram
{
    public class UpdateLoyLuckyProgramDto
    {
        public int Id { get; set; } 
        private string _code;
        /// <summary>
        /// Mã chương trình
        /// </summary>
        [Required(ErrorMessage = "Mã chương trình không được bỏ trống")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        private string _name;
        /// <summary>
        /// Tên chương trình
        /// </summary>
        [Required(ErrorMessage = "Tên chương trình không được bỏ trống")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        [Required(ErrorMessage = "Thời gian bắt đầu không được bỏ trống")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Thời gian kết thúc
        /// </summary>
        [Required(ErrorMessage = "Thời gian kết thúc không được bỏ trống")]
        public DateTime EndDate { get; set; }

        private string _descriptionContentType;
        /// <summary>
        /// Loại nội dung: MARKDOWN, HTML
        /// </summary>
        public string DescriptionContentType
        {
            get => _descriptionContentType;
            set => _descriptionContentType = value?.Trim();
        }

        private string _descriptionContent;
        /// <summary>
        /// Nội dung mô tả
        /// </summary>
        public string DescriptionContent
        {
            get => _descriptionContent;
            set => _descriptionContent = value?.Trim();
        }

        /// <summary>
        /// Ảnh đại diện chương trình
        /// </summary>
        public IFormFile AvatarImageUrl { get; set; }

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
