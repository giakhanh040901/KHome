using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvContractTemplate1
    {
        [Key]
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public string DisplayType { get; set; }
        public string Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Status { get; set; }
        public int PolicyId { get; set; }
        public int ContractSource { get; set; }
        public int ContractTemplateTempId { get; set; }
        public int ConfigContractId { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
