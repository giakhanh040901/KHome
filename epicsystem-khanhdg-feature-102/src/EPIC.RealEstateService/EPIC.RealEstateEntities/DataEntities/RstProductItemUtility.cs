using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Utils;
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

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Tiện ích căn hộ
    /// </summary>
    [Table("RST_PRODUCT_ITEM_UTILITY", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(PartnerId), nameof(ProductItemId), nameof(ProductItemUtilityId), nameof(ProjectUtilityExtendId), IsUnique = false, Name = "IX_RST_PRODUCT_ITEM_UTILITY")]
    public class RstProductItemUtility : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProductItemUtility).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }

        /// <summary>
        /// Id căn hộ
        /// </summary>
        [ColumnSnackCase(nameof(ProductItemId))]
        public int ProductItemId { get; set; }

        /// <summary>
        /// Id Tiện ích
        /// </summary>
        [ColumnSnackCase(nameof(ProductItemUtilityId))]
        public int? ProductItemUtilityId { get; set; }

        /// <summary>
        /// Id Tiện ích mở rộng
        /// </summary>
        [ColumnSnackCase(nameof(ProjectUtilityExtendId))]
        public int? ProjectUtilityExtendId { get; set; }

        /// <summary> 
        /// trạng thái A:Active, D:Deactive
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
