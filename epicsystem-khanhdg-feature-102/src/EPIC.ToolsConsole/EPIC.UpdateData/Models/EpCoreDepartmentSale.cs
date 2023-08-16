using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpCoreDepartmentSale
    {
        public decimal Id { get; set; }
        public decimal? TradingProviderId { get; set; }
        public decimal? DepartmentId { get; set; }
        public decimal? SaleId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
    }
}
