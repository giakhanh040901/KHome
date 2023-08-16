using EPIC.Utils.Attributes;
using EPIC.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Entities;

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_PRODUCT_PRICE", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Bang gia")]
    public class GarnerProductPrice : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(GarnerProductPrice)).ToSnakeUpperCase()}";

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
        public int DistributionId { get; set; }

        [Required]
        [ColumnSnackCase(nameof(PriceDate), TypeName = "DATE")]
        [Comment("Ngay trong ky)")]
        public DateTime PriceDate { get; set; }

        [Required]
        [ColumnSnackCase(nameof(Price))]
        [Comment("Gia ban)")]
        public decimal Price { get; set; }

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
