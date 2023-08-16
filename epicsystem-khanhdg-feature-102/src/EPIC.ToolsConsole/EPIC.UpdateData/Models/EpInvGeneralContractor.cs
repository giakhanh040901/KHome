using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvGeneralContractor
    {
        public decimal Id { get; set; }
        public decimal? Status { get; set; }
        public string Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal? BusinessCustomerId { get; set; }
        public decimal? PartnerId { get; set; }
    }
}
