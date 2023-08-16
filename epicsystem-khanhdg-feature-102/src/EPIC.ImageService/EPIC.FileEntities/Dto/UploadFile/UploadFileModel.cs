using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.ImageAPI.Models
{
    public class UploadFileModel
    {
        public IFormFile File { get; set; }
        public string Folder { get; set; }
    }
}
