using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpProductBondSecondPrice
    {
        public decimal PriceId { get; set; }
        public decimal? TradingProviderId { get; set; }
        public decimal? BondSecondaryId { get; set; }
        public DateTime? PriceDate { get; set; }
        public decimal? Price { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Deleted { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
