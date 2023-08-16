using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpCoreBusinessCustomerBank
    {
        public decimal BusinessCustomerBankAccId { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string BankAccNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        public decimal? BusinessCustomerId { get; set; }
        public string BankAccName { get; set; }
        public decimal? Status { get; set; }
        public string IsDefault { get; set; }
        public decimal? BankId { get; set; }
    }
}
