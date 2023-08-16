using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpCifCode
    {
        public decimal CifId { get; set; }
        public string CifCode { get; set; }
        public decimal? InvestorId { get; set; }
        public decimal? BusinessCustomerId { get; set; }
        public string Deleted { get; set; }
    }
}
