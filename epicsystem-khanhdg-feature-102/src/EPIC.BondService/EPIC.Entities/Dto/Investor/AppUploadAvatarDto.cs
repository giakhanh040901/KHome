using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Investor
{
    public class AppUploadAvatarDto
    {
        [FileMaxLength(MaxLength = 5242880)]
        [FileExtention(AllowableExtentions = new string[] { ".jpg", ".jpeg", ".png" })]
        [Required(ErrorMessage = "Ảnh đại diện không được bỏ trống")]
        public IFormFile Avatar { get; set; }
    }
}
