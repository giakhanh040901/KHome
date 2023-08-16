using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvestorAsset
    {
        public decimal? AssetId { get; set; }
        public string CertificateNo { get; set; }
        public decimal? OgAssetId { get; set; }
        public string OgCertificateNo { get; set; }
        public decimal? BondId { get; set; }
        public decimal? BondSecondaryId { get; set; }
        public decimal? OrderId { get; set; }
        public decimal? InvestorId { get; set; }
        public string AccountNo { get; set; }
        public decimal? TradeQtty { get; set; }
        public decimal? TradeValue { get; set; }
        public decimal? HoldQtty { get; set; }
        public decimal? HoldValue { get; set; }
        public decimal? PendingQtty { get; set; }
        public decimal? PendingValue { get; set; }
        public string AllowSell { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string IsActive { get; set; }
        public string Deleted { get; set; }
    }
}
