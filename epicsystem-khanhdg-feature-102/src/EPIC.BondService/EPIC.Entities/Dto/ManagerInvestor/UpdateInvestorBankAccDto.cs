using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class UpdateInvestorBankAccDto
    {
        public int InvestorBankAccId { get; set; }
        public int InvestorId { get; set; }
        public int InvestorGroupId { get; set; }
        public string BankAccount { get; set; }
        public string OwnerAccount { get; set; }
        public bool IsTemp { get; set; }
    }
}
