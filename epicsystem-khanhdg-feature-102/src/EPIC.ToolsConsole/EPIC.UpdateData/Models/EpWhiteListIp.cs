using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpWhiteListIp
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public decimal TradingProviderId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public decimal Type { get; set; }
    }
}
