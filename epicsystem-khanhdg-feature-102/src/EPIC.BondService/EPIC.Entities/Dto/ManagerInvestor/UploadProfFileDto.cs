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
    public class UploadProfFileDto
    {
        [ListFormFile(ErrorMessage = "Không được bỏ trống file upload", MaxLength = 5242880, MaxFileCount = 10)]
        public List<IFormFile> ProfFile { get; set; }
    }
}
