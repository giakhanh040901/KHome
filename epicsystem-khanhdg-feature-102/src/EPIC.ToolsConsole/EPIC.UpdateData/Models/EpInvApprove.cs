using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvApprove
    {
        public decimal Id { get; set; }
        public decimal? UserRequestId { get; set; }
        public decimal? UserApproveId { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public string RequestNote { get; set; }
        public string ApproveNote { get; set; }
        public string CancelNote { get; set; }
        public decimal? Status { get; set; }
        public decimal? DataType { get; set; }
        public decimal? DataStatus { get; set; }
        public string DataStatusStr { get; set; }
        public decimal? ActionType { get; set; }
        public decimal? ReferId { get; set; }
        public string IsCheck { get; set; }
        public decimal? UserCheckId { get; set; }
        public decimal? ReferIdTemp { get; set; }
        public string CloseNote { get; set; }
        public string OpenNote { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime? OpenDate { get; set; }
        public string Summary { get; set; }
        public decimal? TradingProviderId { get; set; }
        public decimal? PartnerId { get; set; }
    }
}
