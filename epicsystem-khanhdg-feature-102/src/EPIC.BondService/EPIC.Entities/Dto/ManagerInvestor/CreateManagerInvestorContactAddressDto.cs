using EPIC.Entities.Dto.Investor;
using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class CreateManagerInvestorContactAddressDto : CreateContactAddressDto
    {
        public int InvestorGroupId { get; set; }
        public bool isTemp { get; set; }
    }
}
