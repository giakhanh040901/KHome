using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class PPartnerPermission
    {
        public decimal Id { get; set; }
        public decimal PartnerId { get; set; }
        public string PermissionKey { get; set; }
        public decimal PermissionType { get; set; }
        public decimal PermissionInWeb { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
    }
}
