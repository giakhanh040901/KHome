using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_HISTORY_UPDATE_DETAIL", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Lich su thay doi chi tiet tung truong neu can")]
    public class GarnerHistoryUpdateDetail
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(GarnerHistoryUpdateDetail)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [ColumnSnackCase(nameof(OldValue))]
        [MaxLength(128)]
        [Comment("Gia tri cu")]
        public string OldValue { get; set; }

        [ColumnSnackCase(nameof(NewValue))]
        [MaxLength(128)]
        [Comment("Gia tri moi")]
        public string NewValue { get; set; }

        [Required]
        [ColumnSnackCase(nameof(FieldName))]
        [MaxLength(128)]
        [Comment("Ten truong")]
        public string FieldName { get; set; }
    }
}
