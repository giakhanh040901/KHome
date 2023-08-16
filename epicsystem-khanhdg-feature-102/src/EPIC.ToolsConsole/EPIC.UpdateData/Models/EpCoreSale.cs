using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpCoreSale
    {
        public decimal SaleId { get; set; }
        public decimal? InvestorId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        public string Status { get; set; }
        public decimal? BusinessCustomerId { get; set; }
        public string AutoDirectional { get; set; }
    }
}
