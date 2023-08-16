using EPIC.Utils;
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
    public class FaceRecognitionLoggedInDto
    {
        [FileMaxLength(MaxLength = 52428800)]
        [FileExtention(AllowableExtentions = new string[] { ".jpg", ".jpeg" })]
        [Required(ErrorMessage = "Ảnh khuôn mặt không được để trống")]
        public IFormFile FaceImage { get; set; }
    }
}
