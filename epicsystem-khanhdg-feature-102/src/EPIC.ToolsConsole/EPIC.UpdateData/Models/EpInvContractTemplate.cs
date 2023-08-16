using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvContractTemplate
    {
        public decimal Id { get; set; }
        public decimal? DistributionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal? TradingProviderId { get; set; }
        public string ContractTempUrl { get; set; }
        public string Status { get; set; }
        public decimal? Classify { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        public string Type { get; set; }
        public string DisplayType { get; set; }
    }
}
