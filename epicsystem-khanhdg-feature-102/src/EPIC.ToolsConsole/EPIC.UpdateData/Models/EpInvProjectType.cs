using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvProjectType
    {
        public decimal Id { get; set; }
        public decimal? ProjectId { get; set; }
        public decimal? Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Deleted { get; set; }
    }
}
