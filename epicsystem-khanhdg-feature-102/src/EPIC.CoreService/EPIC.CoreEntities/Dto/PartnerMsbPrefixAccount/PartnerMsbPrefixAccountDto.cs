using EPIC.Entities.Dto.BusinessCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.PartnerMsbPrefixAccount
{
    public class PartnerMsbPrefixAccountDto
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public int PartnerBankAccountId { get; set; }
        public string PrefixMsb { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        public BusinessCustomerBankDto BusinessCustomerBank { get; set; }
    }
}
