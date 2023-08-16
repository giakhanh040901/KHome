using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class UserGroup
    {
        public decimal? AutoId { get; set; }
        public decimal? UserId { get; set; }
        public decimal? GroupId { get; set; }
        public string IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Status { get; set; }
    }
}
