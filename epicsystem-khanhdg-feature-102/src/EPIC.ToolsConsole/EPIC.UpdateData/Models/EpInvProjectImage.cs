using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvProjectImage
    {
        public decimal Id { get; set; }
        public decimal? ProjectId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Deleted { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
