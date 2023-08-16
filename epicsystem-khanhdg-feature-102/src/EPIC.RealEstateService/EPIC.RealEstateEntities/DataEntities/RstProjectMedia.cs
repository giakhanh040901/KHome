using DocumentFormat.OpenXml.CustomXmlSchemaReferences;
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
    /// Hình ảnh dự án
    /// </summary>
    [Table("RST_PROJECT_MEDIA", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(PartnerId), nameof(ProjectId), nameof(Status), IsUnique = false, Name = "IX_RST_PROJECT_MEDIA")]
    public class RstProjectMedia : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProjectMedia).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }

        [ColumnSnackCase(nameof(ProjectId))]
        public int ProjectId { get; set; }

        /// <summary>
        /// Tên nhóm (nhóm hình ảnh)
        /// </summary>
        [MaxLength(256)]
        [ColumnSnackCase(nameof(GroupTitle))]
        public string GroupTitle { get; set; }

        /// <summary>
        /// Đường dẫn file ảnh
        /// </summary>
        //[Required]
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
        /// IMAGE: ảnh, VIDEO: video
        /// <see cref="MediaTypes"/>
        /// </summary>
        [MaxLength(256)]
        [ColumnSnackCase(nameof(MediaType), TypeName = "VARCHAR2")]
        public string MediaType { get; set; }

        /// <summary>
        /// Vị trí
        /// <see cref="RstMediaLocations"/>
        /// </summary>
        [MaxLength(256)]
        [ColumnSnackCase(nameof(Location), TypeName = "VARCHAR2")]
        public string Location { get; set; }

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
