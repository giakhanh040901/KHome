using EPIC.Entities.Dto.Investor;
using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class CreateInvestorBankTempDto : CreateBankDto
    {
        public int InvestorGroupId { get; set; }
        public bool IsTemp { get; set; }
    }
}
