using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.BusinessLicenseFile
{
    public class CreateBusinessLicenseFileTempDto
    {
        public int BusinessCustomerTempId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
