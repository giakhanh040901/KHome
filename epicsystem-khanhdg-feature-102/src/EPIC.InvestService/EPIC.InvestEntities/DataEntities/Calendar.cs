using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.InvestEntities.DataEntities
{
    [Table("EP_INV_CALENDAR", Schema = DbSchemas.EPIC)]
    public class Calendar
    {
        [ColumnSnackCase(nameof(WorkingDate), TypeName = "DATE")]
        public DateTime WorkingDate { get; set; }
        [ColumnSnackCase(nameof(WorkingYear))]
        public int WorkingYear { get; set; }
        [ColumnSnackCase(nameof(BusDate), TypeName = "DATE")]
        public DateTime BusDate { get; set; }
        [Column("IS_DAYOFF", TypeName = "VARCHAR2")]
        [MaxLength(1)]
        public string IsDayOff { get; set; }
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }
    }
}
