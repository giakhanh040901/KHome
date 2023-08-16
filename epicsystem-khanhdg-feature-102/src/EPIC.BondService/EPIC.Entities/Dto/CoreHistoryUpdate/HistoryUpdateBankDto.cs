using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.CoreHistoryUpdate
{
    public class HistoryUpdateBankDto
    {
        public int InvestorGroupId { get; set; }
        public int BankId { get; set; }
        public string BankAccount { get; set; }  
        public string IsDefault { get; set; }
        public string Deleted { get; set; }
        public string OwnerAccount { get; set; }

    }
}
