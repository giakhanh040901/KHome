using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.UploadFile
{
    public class UploadFileDto
    {
        public IFormFile file { get; set; }
        public string folder { get; set; }
    }
}
