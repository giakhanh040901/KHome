using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpCoreUserPartner
    {
        public decimal UserPartnerId { get; set; }
        public decimal? UserId { get; set; }
        public decimal? PartnerId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Deleted { get; set; }
    }
}
