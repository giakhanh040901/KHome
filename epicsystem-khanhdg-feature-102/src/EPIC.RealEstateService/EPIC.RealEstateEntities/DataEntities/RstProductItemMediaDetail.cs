using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Media;
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
    /// Nhóm hình ảnh sản phẩm
    /// </summary>
    [Table("RST_PRODUCT_ITEM_MEDIA_DETAIL", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(PartnerId), nameof(Status), nameof(ProductItemMediaId), IsUnique = false, Name = "IX_RST_PRODUCT_ITEM_MEDIA_DETAIL")]
    public class RstProductItemMediaDetail : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProductItemMediaDetail).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }

        /// <summary>
        /// Id hình ảnh sản phẩm
        /// </summary>
        [ColumnSnackCase(nameof(ProductItemMediaId))]
        public int ProductItemMediaId { get; set; }

        /// <summary>
        /// Đường dẫn file ảnh
        /// </summary>
        [Required]
        [MaxLength(512)]
        [ColumnSnackCase(nameof(UrlImage), TypeName = "VARCHAR2")]
        public string UrlImage { get; set; }

        /// <summary>
        /// IMAGE: ảnh, VIDEO: video
        /// <see cref="MediaTypes"/>
        /// </summary>
        [MaxLength(256)]
        [ColumnSnackCase(nameof(MediaType), TypeName = "VARCHAR2")]
        public string MediaType { get; set; }

        [MaxLength(1)]
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(SortOrder))]
        public int SortOrder { get; set; }

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
