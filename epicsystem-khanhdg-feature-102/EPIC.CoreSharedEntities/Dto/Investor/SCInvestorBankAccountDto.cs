using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class SCInvestorBankAccountDto
    {
        public string BankAccount { get; set; }
        public string BankName { get; set; }
        public string OwnerAccount { get; set; }
        public string CoreFullBankName { get; set; }
    }
}
