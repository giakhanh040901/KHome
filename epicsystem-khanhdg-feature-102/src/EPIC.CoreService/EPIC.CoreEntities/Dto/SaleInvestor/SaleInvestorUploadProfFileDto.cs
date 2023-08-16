using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.SaleInvestor
{
    public class SaleInvestorUploadProfFileDto
    {
        [FromForm(Name = "investorId")]
        public int InvestorId { get; set; }

        [FromForm(Name = "profFile")]
        [ListFormFile(ErrorMessage = "Không được bỏ trống file upload", MaxLength = 5242880, MaxFileCount = 10)]
        public List<IFormFile> ProfFile { get; set; }
    }
}
