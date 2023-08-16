using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpTradingProvider
    {
        public int TradingProviderId { get; set; }
        public decimal? Status { get; set; }
        public string Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal? BusinessCustomerId { get; set; }
        public decimal? PartnerId { get; set; }
        public string AliasName { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
        public string Server { get; set; }
        public string StampImageUrl { get; set; }
        public string IsDefaultBond { get; set; }
        public string IsDefaultCps { get; set; }
        public string IsDefaultGarner { get; set; }
        public string IsDefaultInvest { get; set; }
        public string IsIpPayment { get; set; }
    }
}
