using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvDistributionFile
    {
        public decimal Id { get; set; }
        public decimal? DistributionId { get; set; }
        public string Title { get; set; }
        public string FileUrl { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
    }
}
