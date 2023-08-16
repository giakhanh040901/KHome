using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpBondRenewalsRequest
    {
        public decimal Id { get; set; }
        public decimal OrderId { get; set; }
        public decimal RenewalsPolicyDetailId { get; set; }
        public decimal SettlementMethod { get; set; }
        public string Deleted { get; set; }
    }
}
