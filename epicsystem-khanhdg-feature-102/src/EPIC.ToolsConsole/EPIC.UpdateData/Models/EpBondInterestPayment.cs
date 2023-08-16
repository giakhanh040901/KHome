using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpBondInterestPayment
    {
        public decimal Id { get; set; }
        public decimal OrderId { get; set; }
        public decimal PeriodIndex { get; set; }
        public string CifCode { get; set; }
        public decimal AmountMoney { get; set; }
        public decimal PolicyDetailId { get; set; }
        public decimal TradingProviderId { get; set; }
        public decimal Status { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public string ApproveIp { get; set; }
        public string IsLastPeriod { get; set; }
        public DateTime PayDate { get; set; }
    }
}
