using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpProductBondSecondary
    {
        public decimal BondSecondaryId { get; set; }
        public decimal? ProductBondId { get; set; }
        public decimal? TradingProviderId { get; set; }
        public decimal? BondPrimaryId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Status { get; set; }
        public DateTime? OpenCellDate { get; set; }
        public DateTime? CloseCellDate { get; set; }
        public string IsClose { get; set; }
        public string IsShowApp { get; set; }
        public decimal? BusinessCustomerBankAccId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public string IsCheck { get; set; }
        public string ContentType { get; set; }
        public string OverviewContent { get; set; }
        public string OverviewImageUrl { get; set; }
    }
}
