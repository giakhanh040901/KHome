using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpDistributionConPayment
    {
        public decimal PaymentId { get; set; }
        public decimal? DistributionContractId { get; set; }
        public bool? TransactionType { get; set; }
        public bool? PaymentType { get; set; }
        public bool? Status { get; set; }
        public string Description { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public DateTime? CancelDate { get; set; }
        public string CancelBy { get; set; }
        public decimal? TotalValue { get; set; }
        public DateTime? TradingDate { get; set; }
    }
}
