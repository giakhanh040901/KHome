using EPIC.Utils.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPIC.BondEntities.DataEntities
{
    public class BondCalendar
    {
        [Key]
        [ColumnSnackCase("TRADING_PROVIDER_ID")]
        public int TradingProviderId { get; set; }

        [Key]
        [ColumnSnackCase("WORKING_DATE")]
        public DateTime WorkingDate { get; set; }

        [Key]
        [ColumnSnackCase("WORKING_YEAR")]
        public int WorkingYear { get; set; }

        [ColumnSnackCase("BUS_DATE")]
        public DateTime BusDate { get; set; }

        [ColumnSnackCase("IS_DAYOFF")]
        public string IsDayOff { get; set; }
    }
}
