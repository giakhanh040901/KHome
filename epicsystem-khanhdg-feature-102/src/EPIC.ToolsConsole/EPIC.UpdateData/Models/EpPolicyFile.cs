using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpPolicyFile
    {
        public decimal? PolicyFileId { get; set; }
        public decimal? BondSecondaryId { get; set; }
        public decimal? TradingProviderId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public decimal? Status { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
