using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvOrderPayment
    {
        public decimal Id { get; set; }
        public decimal? TradingProviderId { get; set; }
        public decimal? OrderId { get; set; }
        public string PaymentNo { get; set; }
        public DateTime? TranDate { get; set; }
        public decimal? TranType { get; set; }
        public decimal? TranClassify { get; set; }
        public decimal? PaymentType { get; set; }
        public decimal? PaymentAmnount { get; set; }
        public string Description { get; set; }
        public decimal? Status { get; set; }
        public string ApproveBy { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string CancelBy { get; set; }
        public DateTime? CancelDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public decimal? TradingBankAccId { get; set; }
    }
}
