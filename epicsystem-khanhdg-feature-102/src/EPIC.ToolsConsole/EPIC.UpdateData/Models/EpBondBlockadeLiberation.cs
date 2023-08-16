using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpBondBlockadeLiberation
    {
        public decimal Id { get; set; }
        public decimal? Type { get; set; }
        public string BlockadeDescription { get; set; }
        public DateTime? BlockadeDate { get; set; }
        public decimal? OrderId { get; set; }
        public string Blockader { get; set; }
        public DateTime? BlockadeTime { get; set; }
        public string LiberationDescription { get; set; }
        public DateTime? LiberationDate { get; set; }
        public string Liberator { get; set; }
        public DateTime? LiberationTime { get; set; }
        public decimal? Status { get; set; }
        public decimal? TradingProviderId { get; set; }
        public string CreatedBy { get; set; }
    }
}
