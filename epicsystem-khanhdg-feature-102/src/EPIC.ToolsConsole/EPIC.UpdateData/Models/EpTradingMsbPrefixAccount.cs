using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpTradingMsbPrefixAccount
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public int TradingBankAccountId { get; set; }
        public string PrefixMsb { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        public string AccessCode { get; set; }
        public string MId { get; set; }
        public string TId { get; set; }
    }
}
