using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpGuaranteeAsset
    {
        public decimal GuaranteeAssetId { get; set; }
        public decimal? ProductBondId { get; set; }
        public string Code { get; set; }
        public decimal? AssetValue { get; set; }
        public decimal? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public string DescriptionAsset { get; set; }
    }
}
