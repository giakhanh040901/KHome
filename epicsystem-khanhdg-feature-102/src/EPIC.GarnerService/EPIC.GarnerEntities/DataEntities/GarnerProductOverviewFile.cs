using EPIC.Entities;
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
    [Table("GAN_PRODUCT_OVERVIEW_FILE", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Tong quan file San pham tich luy")]
    public class GarnerProductOverviewFile : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(GarnerProductOverviewFile)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [ColumnSnackCase(nameof(TradingProviderId))]
        [Comment("Id dai ly")]
        public int TradingProviderId { get; set; }

        [Required]
        [ColumnSnackCase(nameof(DistributionId))]
        [Comment("Id Phan phoi san pham tich luy")]
        public int DistributionId { get; set; }

        [ColumnSnackCase(nameof(DocumentType))]
        [Comment("Loai tai lieu: 1: Thong tin san pham, 2: Chinh sach san pham, 3: Ho so phap ly, 4:  ...")]
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
        [Comment("Trang thai")]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(EffectiveDate), TypeName = "DATE")]
        public DateTime? EffectiveDate { get; set; }

        [ColumnSnackCase(nameof(ExpirationDate), TypeName = "DATE")]
        public DateTime? ExpirationDate { get; set; }

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
