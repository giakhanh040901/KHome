using EPIC.CompanySharesEntities.Dto.CpsSecondary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto
{
    public class CreateCpsPolicySpecificDto : CreateCpsPolicyBaseDto
    {
        public int PolicyTempId { get; set; }
    }
}
