using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.CoreHistoryUpdate
{
    public class HistoryUpdateBusinessCustomerBankDto
    {
        public string BankAccName { get; set; }
        public string BankAccNo { get; set; }
        public int BankId { get; set; }
        public string BankBranchName { get; set; }
        public int? Status { get; set; }
        public string IsDefault { get; set; }
        public string Deleted { get; set; }
    }
}
