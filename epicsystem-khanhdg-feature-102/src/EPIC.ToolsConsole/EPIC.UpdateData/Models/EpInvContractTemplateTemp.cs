using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvContractTemplateTemp
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int TradingProviderId { get; set; }
        public int ContractType { get; set; }
        public string Status { get; set; }
        public int ContractSource { get; set; }
        public string FileInvestor { get; set; }
        public string FileBusinessCustomer { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
    }
}
