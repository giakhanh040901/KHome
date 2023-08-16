using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpCalendar
    {
        public decimal? WorkingYear { get; set; }
        public DateTime? WorkingDate { get; set; }
        public DateTime? BusDate { get; set; }
        public string IsDayoff { get; set; }
        public decimal? TradingProviderId { get; set; }
    }
}
