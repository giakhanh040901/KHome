using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Utils.Validation;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class SaveFaceMatchImageDto
    {
        private string _phone { get; set; }

        [FileMaxLength(MaxLength = 52428800)]
        [FileExtention(AllowableExtentions = new string[] { ".jpg", ".jpeg", ".png" })]
        [Required(ErrorMessage = "Ảnh khuôn mặt không được để trống")]
        public IFormFile Image { get; set; }

        [IntegerRange(AllowableValues = new int[] { FaceMatchImageTypes.ANH_MAT_CUOI, FaceMatchImageTypes.ANH_MAT_TRAI, FaceMatchImageTypes.ANH_MAT_PHAI, FaceMatchImageTypes.ANH_MAT_NHAY_MAT })]
        public int ImageType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại không được bỏ trống")]
        public string Phone { get => _phone; set => _phone = value?.Trim(); }
    }
}
