using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.BusinessLicenseFile
{
    public class UpdateBusinessLicenseFileDto : CreateBusinessLicenseFileDto
    {
        public int Id { get; set; }
    }
}
