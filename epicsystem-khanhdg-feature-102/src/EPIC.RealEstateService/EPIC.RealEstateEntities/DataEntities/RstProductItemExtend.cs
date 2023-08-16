using DocumentFormat.OpenXml.Wordprocessing;
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

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Các thông tin khác của căn hộ dự án
    /// </summary>
    [Table("RST_PRODUCT_ITEM_EXTEND", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(ProductItemId), IsUnique = false, Name = "IX_RST_PRODUCT_ITEM_EXTEND")]
    public class RstProductItemExtend : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProductItemExtend).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(ProductItemId))]
        public int ProductItemId { get; set; }

        /// <summary>
        /// Tiêu đề thông tin
        /// </summary>
        [Required]
        [ColumnSnackCase(nameof(Title))]
        [MaxLength(512)]
        public string Title { get; set; }

        /// <summary>
        /// Tên icon
        /// </summary>
        [ColumnSnackCase(nameof(IconName))]
        [MaxLength(128)]
        public string IconName { get; set; }

        /// <summary> 
        /// Mô tả nội dung thông tin
        /// </summary>
        [ColumnSnackCase(nameof(Description), TypeName = "VARCHAR2")]
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
