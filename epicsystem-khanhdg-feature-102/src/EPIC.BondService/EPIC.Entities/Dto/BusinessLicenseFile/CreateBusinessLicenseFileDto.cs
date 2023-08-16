using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.BusinessLicenseFile
{
    public class CreateBusinessLicenseFileDto
    {
        public int BusinessCustomerId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
