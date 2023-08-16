using EPIC.Utils;
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
    public class AddIdentificationDto
    {
        //[FileMaxLength(MaxLength = 10485760)]
        //[FileExtention(AllowableExtentions = new string[] { ".jpg", ".jpeg", ".png" })]
        [Required(ErrorMessage = "Ảnh mặt trước CMND/CCCD/Hộ chiếu không được bỏ trống")]
        public IFormFile FrontImage { get; set; }

        //[FileMaxLength(MaxLength = 10485760)]
        //[FileExtention(AllowableExtentions = new string[] { ".jpg", ".jpeg", ".png" })]
        //[Required(ErrorMessage = "Ảnh mặt sau CMND/CCCD/Hộ chiếu không được bỏ trống")]
        public IFormFile BackImage { get; set; }

        [Required(ErrorMessage = "Loại giấy tờ không được bỏ trống")]
        [StringRange(AllowableValues = new string[] { CardTypesInput.CMND, CardTypesInput.CCCD, CardTypesInput.PASSPORT })]
        public string Type { get; set; }
    }
}
