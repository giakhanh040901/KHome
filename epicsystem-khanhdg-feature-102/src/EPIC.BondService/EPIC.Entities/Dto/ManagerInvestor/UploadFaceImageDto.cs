using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    /// <summary>
    /// Manager Investor Upload avatar
    /// </summary>
    public class UploadFaceImageDto
    {
        [FileMaxLength(MaxLength = 5242880)]
        [FileExtention(AllowableExtentions = new string[] { ".jpg", ".jpeg" })]
        [Required(ErrorMessage = "Ảnh khuôn mặt không được để trống")]
        public IFormFile FaceImage { get; set; }
        public int InvestorId { get; set; }
        public int InvestorGroupId { get; set; }
        public bool IsTemp { get; set; }
    }
}
