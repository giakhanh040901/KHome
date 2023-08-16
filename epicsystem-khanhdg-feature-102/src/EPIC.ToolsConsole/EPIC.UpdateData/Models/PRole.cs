using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class PRole
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public decimal? PartnerId { get; set; }
        public decimal RoleType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public string Status { get; set; }
        public decimal? PermissionInWeb { get; set; }
        public decimal? TradingProviderId { get; set; }
    }
}
