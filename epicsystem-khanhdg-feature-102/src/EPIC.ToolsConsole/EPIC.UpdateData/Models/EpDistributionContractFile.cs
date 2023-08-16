using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpDistributionContractFile
    {
        public decimal FileId { get; set; }
        public decimal? DistributionContractId { get; set; }
        public string Title { get; set; }
        public string FileUrl { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public decimal? Status { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveBy { get; set; }
        public DateTime? CancelDate { get; set; }
        public string CancelBy { get; set; }
    }
}
