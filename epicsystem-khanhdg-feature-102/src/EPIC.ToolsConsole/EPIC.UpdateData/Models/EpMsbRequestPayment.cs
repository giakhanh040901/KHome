using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpMsbRequestPayment
    {
        public long Id { get; set; }
        public int TradingProdiverId { get; set; }
        public int ProductType { get; set; }
        public int RequestType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
