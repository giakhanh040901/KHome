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
using EPIC.Entities;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Ảnh minh họa tiện ích
    /// </summary>
    [Table("RST_PROJECT_UTILITY_MEDIA", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(ProjectId), nameof(PartnerId), nameof(Status), IsUnique = false, Name = "IX_RST_PROJECT_UTILITY_MEDIA")]
    public class RstProjectUtilityMedia : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProjectUtilityMedia).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(ProjectId))]
        public int ProjectId { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }

        /// <summary>
        /// Tên hình ảnh
        /// </summary>
        [ColumnSnackCase(nameof(Title))]
        [MaxLength(512)]
        [Comment("Ten hinh anh")]
        public string Title { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [ColumnSnackCase(nameof(Url))]
        [Comment("Duong dan")]
        [MaxLength(2048)]
        public string Url { get; set; }

        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Trang thai  (A : Kich hoat; D : Khong kich hoat)")]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(Type))]
        [Comment("Loai tien ich (1: Noi khu, 2:Ngoai khu)")]
        public int Type { get; set; }

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
