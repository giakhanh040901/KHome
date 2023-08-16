using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class EKYCFaceMatchDto
    {
        private string _phone;

        [FileMaxLength(MaxLength = 52428800)]
        [FileExtention(AllowableExtentions = new string[] { ".jpg", ".jpeg", ".png" })]
        [Required(ErrorMessage = "Ảnh cccd/cmnd/hộ chiếu không được để trống")]
        public IFormFile IdCardImage { get; set; }

        [FileMaxLength(MaxLength = 52428800)]
        [FileExtention(AllowableExtentions = new string[] { ".jpg", ".jpeg", ".png" })]
        [Required(ErrorMessage = "Ảnh khuôn mặt không được để trống")]
        public IFormFile FaceImage { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại không được bỏ trống")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }
    }

    public class EKYCFaceMatchResultDto
    {
        /// <summary>
        /// Có giống hay không (trên 80% là hợp lệ)
        /// </summary>
        public bool IsMatch { get; set; }
        /// <summary>
        /// Giống bao nhiêu so với ảnh gốc trên 100
        /// </summary>
        public double Similarity { get; set; }
        /// <summary>
        /// Cả 2 ảnh đều là Id card
        /// </summary>
        public bool IsBothImgIDCard { get; set; }
    }
}
