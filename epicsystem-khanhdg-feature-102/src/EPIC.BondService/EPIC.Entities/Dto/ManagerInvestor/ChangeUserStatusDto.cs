using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class ChangeUserStatusDto
    {
        public int UserId { get; set; }
        public int InvestorId { get; set; }

        [StringRange(AllowableValues = new string[] { InvestorStatus.ACTIVE, InvestorStatus.DEACTIVE, InvestorStatus.TEMP })]
        public string Status { get; set; }
    }
}
