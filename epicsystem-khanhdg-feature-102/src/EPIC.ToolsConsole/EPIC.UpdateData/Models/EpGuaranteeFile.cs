using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpGuaranteeFile
    {
        public decimal GuaranteeFileId { get; set; }
        public decimal? GuaranteeAssetId { get; set; }
        public string Title { get; set; }
        public string FileUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Deleted { get; set; }
    }
}
