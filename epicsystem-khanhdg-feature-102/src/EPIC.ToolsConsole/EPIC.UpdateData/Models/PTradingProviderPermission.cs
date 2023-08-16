using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class PTradingProviderPermission
    {
        public decimal Id { get; set; }
        public decimal TradingProviderId { get; set; }
        public string PermissionKey { get; set; }
        public decimal PermissionType { get; set; }
        public decimal PermissionInWeb { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
