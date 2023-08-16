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
    [Table("GAN_PRODUCT_FILE", Schema = DbSchemas.EPIC_GARNER)]
    public class GarnerProductFile : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(GarnerProductFile)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [ColumnSnackCase(nameof(PartnerId))]
        [Comment("Id doi tac")]
        public int PartnerId { get; set; }

        [Required]
        [ColumnSnackCase(nameof(ProductId))]
        [Comment("Id san pham")]
        public int ProductId { get; set; }

        [ColumnSnackCase(nameof(DocumentType))]
        [MaxLength(1)]
        [Comment("Loai tai lieu: 1: tai san dam bao, 2: ho so phap ly,...")]
        public int DocumentType { get; set; }

        [ColumnSnackCase(nameof(Title))]
        [MaxLength(1024)]
        [Comment("Tieu de")]
        public string Title { get; set; }

        [ColumnSnackCase(nameof(Url))]
        [MaxLength(1024)]
        [Comment("Duong dan")]
        public string Url { get; set; }

        [Required]
        [ColumnSnackCase(nameof(Status))]
        [MaxLength(1)]
        [Comment("Trang thai")]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(TotalValue), TypeName = "DECIMAL(18, 2)")]
        public decimal? TotalValue { get; set; }

        [ColumnSnackCase(nameof(Description))]
        [MaxLength(2048)]
        public string Description { get; set; }

        #region audit
        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        [ColumnSnackCase(nameof(ModifiedDate), TypeName = "DATE")]
        public DateTime? ModifiedDate { get; set; }

        [ColumnSnackCase(nameof(ModifiedBy))]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [ColumnSnackCase(nameof(Deleted), TypeName = "VARCHAR2")]
        [Required]
        [MaxLength(1)]
        public string Deleted { get; set; }
        #endregion
    }
}
