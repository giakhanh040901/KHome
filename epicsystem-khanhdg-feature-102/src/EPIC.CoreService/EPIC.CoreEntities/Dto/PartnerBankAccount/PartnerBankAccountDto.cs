using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.PartnerBankAccount
{
    public class PartnerBankAccountDto
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public string BankAccName { get; set; }
        public string BankAccNo { get; set; }
        public int BankId { get; set; }
        public int Status { get; set; }
        public string IsDefault { get; set; }
        public string BankName { get; set;}
    }
}
