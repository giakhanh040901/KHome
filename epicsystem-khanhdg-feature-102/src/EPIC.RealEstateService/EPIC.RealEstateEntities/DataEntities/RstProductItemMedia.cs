using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Media;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Hình ảnh sản phẩm
    /// </summary>
    [Table("RST_PRODUCT_ITEM_MEDIA", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(PartnerId), nameof(ProductItemId), nameof(Status), IsUnique = false, Name = "IX_RST_PRODUCT_ITEM_MEDIA")]
    public class RstProductItemMedia : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProductItem).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }

        /// <summary>
        /// Id sản phẩm
        /// </summary>
        [ColumnSnackCase(nameof(ProductItemId))]
        public int ProductItemId { get; set; }

        /// <summary>
        /// Tên nhóm (nhóm hình ảnh)
        /// </summary>
        [MaxLength(256)]
        [ColumnSnackCase(nameof(GroupTitle))]
        public string GroupTitle { get; set; }

        /// <summary>
        /// Vị trí<br/>
        /// <see cref="RstProductItemMediaLocations"/>
        /// </summary>
        [MaxLength(256)]
        [ColumnSnackCase(nameof(Location), TypeName = "VARCHAR2")]
        public string Location { get; set; }

        /// <summary>
        /// Loại media<br/>
        /// <see cref="MediaTypes"/>
        /// </summary>
        [MaxLength(256)]
        [ColumnSnackCase(nameof(MediaType), TypeName = "VARCHAR2")]
        public string MediaType { get; set; }

        /// <summary>
        /// Đường dẫn file ảnh
        /// </summary>
        [MaxLength(512)]
        [ColumnSnackCase(nameof(UrlImage), TypeName = "VARCHAR2")]
        public string UrlImage { get; set; }

        /// <summary>
        /// Đường dẫn điều hướng khi click ảnh
        /// </summary>
        [MaxLength(512)]
        [ColumnSnackCase(nameof(UrlPath), TypeName = "VARCHAR2")]
        public string UrlPath { get; set; }

        /// <summary>
        /// Trạng thái A, D, X<br/>
        /// <see cref="Utils.Status"/>
        /// </summary>
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
