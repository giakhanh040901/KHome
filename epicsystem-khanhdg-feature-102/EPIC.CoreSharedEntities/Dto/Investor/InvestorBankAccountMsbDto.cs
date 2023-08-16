using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class InvestorBankAccountMsbDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string Reason { get; set; }
        public int InvestorId { get; set; }
    }
}
