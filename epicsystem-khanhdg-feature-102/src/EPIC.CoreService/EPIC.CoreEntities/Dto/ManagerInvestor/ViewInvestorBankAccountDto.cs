using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ManagerInvestor
{
    public class ViewInvestorBankAccountDto
    {
        public int Id { get; set; }
        public int InvestorId { get; set; }
        public int InvestorGroupId { get; set; }
        public int BankId { get; set; }
        public string BankAccount { get; set; }
        public string BankName { get; set; }
        public string IsDefault { get; set; }
        public string OwnerAccount { get; set; }
        public string CoreBankName { get; set; }
        public string CoreFullBankName { get; set; }
        public string CoreLogo { get; set; }
        public string AllowDeleted { get; set; }
    }
}
