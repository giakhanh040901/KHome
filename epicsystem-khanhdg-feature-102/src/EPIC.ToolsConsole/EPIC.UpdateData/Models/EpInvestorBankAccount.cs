using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvestorBankAccount
    {
        public decimal Id { get; set; }
        public decimal InvestorId { get; set; }
        public string BankAccount { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string IsDefault { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Deleted { get; set; }
        public string OwnerAccount { get; set; }
        public decimal? BankId { get; set; }
        public decimal? InvestorGroupId { get; set; }
        public decimal? ReferId { get; set; }
        public string IsDefaultSale { get; set; }
    }
}
