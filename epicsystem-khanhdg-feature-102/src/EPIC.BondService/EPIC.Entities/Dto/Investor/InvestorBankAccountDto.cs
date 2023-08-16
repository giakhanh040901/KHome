using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Investor
{
    public class InvestorBankAccountDto
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string OwnerAccount { get; set; }
        public string IsDefault { get; set; }
    }
}
