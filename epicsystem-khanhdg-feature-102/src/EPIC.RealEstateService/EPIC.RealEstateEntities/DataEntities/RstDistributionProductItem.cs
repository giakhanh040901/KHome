using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
using System.ComponentModel.DataAnnotations;
using System;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities.DataEntities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Phân phối sản phẩm nào
    /// </summary>
    [Table("RST_DISTRIBUTION_PRODUCT_ITEM", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(ProductItemId), nameof(DistributionId), IsUnique = false, Name = "IX_RST_DISTRIBUTION_PRODUCT_ITEM")]
    public class RstDistributionProductItem : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstDistributionProductItem).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(DistributionId))]
        public int DistributionId { get; set; }

        /// <summary>
        /// Id căn từ ProductItem
        /// </summary>
        [ColumnSnackCase(nameof(ProductItemId))]
        public int ProductItemId { get; set; }
        /// <summary>
        /// Trạng thái có khóa căn không? (A/D)
        /// </summary>
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        public string Status { get; set; }

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
