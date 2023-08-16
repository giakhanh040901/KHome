using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.SaleInvestor
{
    public class SaleInvestorUploadAvatarDto
    {
        [FileMaxLength(MaxLength = 5242880)]
        [FileExtention(AllowableExtentions = new string[] { ".jpg", ".jpeg", ".png" })]
        [Required(ErrorMessage = "Avatar không được bỏ trống")]
        public IFormFile Image { get; set; }

        public int InvestorId { get; set; }

    }
}
