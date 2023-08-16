using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_HISTORY_UPDATE", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Lich su thay doi chi luu 1 ban ghi cho moi lan thay doi")]
    public class GarnerHistoryUpdate : ICreatedBy
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(GarnerHistoryUpdate)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [ColumnSnackCase(nameof(RealTableId))]
        [Comment("Id bang that")]
        public long RealTableId { get; set; }

        [ColumnSnackCase(nameof(OldValue))]
        [MaxLength(128)]
        [Comment("Gia tri cu")]
        public string OldValue { get; set; }

        [ColumnSnackCase(nameof(NewValue))]
        [MaxLength(128)]
        [Comment("Gia tri moi")]
        public string NewValue { get; set; }

        [ColumnSnackCase(nameof(FieldName))]
        [MaxLength(128)]
        [Comment("Ten truong")]
        public string FieldName { get; set; }

        [Required]
        [ColumnSnackCase(nameof(UpdateTable))]
        [MaxLength(128)]
        [Comment("Update bang nao (1: GAN_PRODUCT, 2 GAN_POLICY) bang nào chua co thi them tiep vao day")]
        public int UpdateTable { get; set; }

        [Required]
        [ColumnSnackCase(nameof(Action))]
        [Comment("Hanh dong (1: them moi, 2: cap nhat, 3: xoa)")]
        public int Action { get; set; }

        [Required]
        [ColumnSnackCase(nameof(Summary))]
        [MaxLength(512)]
        [Comment("Noi dung tong quan la lam cai gi (vd: them moi san pham, phan phoi 500.000 co phan cho dai ly A)")]
        public string Summary { get; set; }

        [ColumnSnackCase(nameof(CreatedDate))]
        [Comment("Ngay thuc hien")]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        [Comment("Nguoi thuc hien")]
        public string CreatedBy { get; set; }

        public GarnerHistoryUpdate() { }
        public GarnerHistoryUpdate(long realTableId, string oldValue, string newValue, string fieldName, int updateTable, int action, string summary)
        {
            RealTableId = realTableId;
            OldValue = oldValue;
            NewValue = newValue;
            FieldName = fieldName;
            UpdateTable = updateTable;
            Action = action;
            Summary = summary;
        }
    }
}
