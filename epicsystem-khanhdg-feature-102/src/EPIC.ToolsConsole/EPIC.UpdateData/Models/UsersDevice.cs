using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class UsersDevice
    {
        public decimal Id { get; set; }
        public decimal UserId { get; set; }
        public string DeviceId { get; set; }
        public bool? Status { get; set; }
        public string Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
