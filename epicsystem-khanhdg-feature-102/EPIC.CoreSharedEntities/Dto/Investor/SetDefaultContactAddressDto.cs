using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class SetDefaultContactAddressDto
    {
        public int InvestorId { get; set; }
        public int ContactAddressId { get; set; }
    }
}
