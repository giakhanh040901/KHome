using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    public class BusinessCustomerBankTemp
    {
        public int Id { get; set; }
        public int? BusinessCustomerBankAccId { get; set; }
        public int? BusinessCustomerTempId { get; set; }
        public int? BusinessCustomerId { get; set; }
        public string BankAccName { get; set; }
        public string BankName { get; set; }
        public string BankAccNo { get; set; }
        public int BankId { get; set; }
        public string BankBranchName { get; set; }
        public int? Status { get; set; }
        public string IsDefault { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        public int IsTemp { get; set; }
    }
}
