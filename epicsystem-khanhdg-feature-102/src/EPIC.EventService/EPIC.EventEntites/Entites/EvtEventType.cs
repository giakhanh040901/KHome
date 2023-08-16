using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.EventEntites.Entites
{
    /// <summary>
    /// Loại hình Sự kiện
    /// </summary>
    [Table("EVT_EVENT_TYPE", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(EventId), IsUnique = false, Name = "IX_EVT_EVENT_TYPE")]
    public class EvtEventType
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(EvtEventType).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        /// <summary>
        /// Id sự kiện
        /// </summary>
        [ColumnSnackCase(nameof(EventId))]
        public int EventId { get; set; }
        public EvtEvent Event { get; set; }
        /// <summary>
        /// Loại sự kiện
        /// </summary>
        [ColumnSnackCase(nameof(Type))]
        public int Type { get; set; }
    }
}
