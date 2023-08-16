using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpProductBondSecondaryNews
    {
        public decimal Id { get; set; }
        public decimal? BondSecondaryId { get; set; }
        public string ImgUrl { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public decimal? TradingProviderId { get; set; }
    }
}
