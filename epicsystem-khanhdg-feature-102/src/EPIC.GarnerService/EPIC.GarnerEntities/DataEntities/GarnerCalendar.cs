using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_CALENDAR", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Lich")]
    public class GarnerCalendar
    {
        //public static string SEQ = $"SEQ_{nameof(GarnerCalendar).ToSnakeUpperCase()}";

        [ColumnSnackCase(nameof(WorkingYear))]
        public int WorkingYear { get; set; }

        [ColumnSnackCase(nameof(WorkingDate), TypeName = "DATE")]
        public DateTime WorkingDate { get; set; }

        [ColumnSnackCase(nameof(BusDate), TypeName = "DATE")]
        public DateTime? BusDate { get; set; }

        [Column("IS_DAYOFF", TypeName = "VARCHAR2")]
        [MaxLength(1)]
        public string IsDayOff { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }
    }
}
