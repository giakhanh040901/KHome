using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpCoreBusinessCusPartner
    {
        public decimal Id { get; set; }
        public decimal? BusinessCustomerId { get; set; }
        public decimal? PartnerId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Deleted { get; set; }
    }
}
