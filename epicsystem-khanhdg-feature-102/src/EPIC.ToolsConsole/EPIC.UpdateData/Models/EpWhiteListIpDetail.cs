using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpWhiteListIpDetail
    {
        public decimal Id { get; set; }
        public decimal WhiteListIpId { get; set; }
        public string IpAddressStart { get; set; }
        public string IpAddressEnd { get; set; }
        public decimal TradingProviderId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Deleted { get; set; }
    }
}
