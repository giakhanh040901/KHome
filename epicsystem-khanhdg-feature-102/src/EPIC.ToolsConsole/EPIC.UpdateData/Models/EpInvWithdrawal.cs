using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvWithdrawal
    {
        public decimal Id { get; set; }
        public decimal OrderId { get; set; }
        public string CifCode { get; set; }
        public decimal AmountMoney { get; set; }
        public decimal PolicyDetailId { get; set; }
        public decimal TradingProviderId { get; set; }
        public decimal Type { get; set; }
        public decimal Source { get; set; }
        public decimal Status { get; set; }
        public DateTime WithdrawalDate { get; set; }
        public string RequestIp { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveBy { get; set; }
        public string ApproveIp { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public decimal? ActuallyAmount { get; set; }
    }
}
