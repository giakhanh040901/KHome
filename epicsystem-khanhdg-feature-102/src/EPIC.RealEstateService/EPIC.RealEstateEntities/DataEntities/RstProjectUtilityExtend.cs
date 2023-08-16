using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Tiện ích khác
    /// </summary>
    [Table("RST_PROJECT_UTILITY_EXTEND", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(ProjectId), nameof(PartnerId), IsUnique = false, Name = "IX_RST_PROJECT_UTILITY_EXTEND")]
    public class RstProjectUtilityExtend : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProjectUtilityExtend).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(ProjectId))]
        public int ProjectId { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }

        /// <summary>
        /// Tên tiệc ích
        /// </summary>
        [Required]
        [ColumnSnackCase(nameof(Title))]
        [MaxLength(512)]
        [Comment("Ten tien ich")]
        public string Title { get; set; }

        /// <summary>
        /// Id nhóm tiện ích
        /// </summary>
        [Required]
        [ColumnSnackCase(nameof(GroupUtilityId))]
        public int GroupUtilityId { get; set; }

        /// <summary>
        /// Tên icon
        /// </summary>
        [ColumnSnackCase(nameof(IconName))]
        [MaxLength(2048)]
        public string IconName { get; set; }

        /// <summary> 
        /// trạng thái A:Active, D:Deactive
        /// </summary>
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        public string Status { get; set; }

        /// <summary>
        /// Loại tiện ích 1: Noi khu, 2 Ngoai khu
        /// </summary>
        [ColumnSnackCase(nameof(Type))]
        public int Type { get; set; }

        /// <summary>
        /// Nổi bật
        /// </summary>
        [ColumnSnackCase(nameof(IsHighlight), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        public string IsHighlight { get; set; }

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
