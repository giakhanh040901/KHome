using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Utils;
using EPIC.Utils.Attributes;
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
    /// Cài chính sách ưu đãi chủ đầu tư - đối tác cài
    /// </summary>
    [Table("RST_PROJECT_POLICY", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(PartnerId), nameof(ProjectId), nameof(Status), nameof(PolicyType), IsUnique = false, Name = "IX_RST_PROJECT_POLICY")]
    public class RstProjectPolicy : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProjectPolicy).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }

        [ColumnSnackCase(nameof(ProjectId))]
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(256)]
        [ColumnSnackCase(nameof(Code))]
        public string Code { get; set; }

        [Required]
        [MaxLength(256)]
        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }

        /// <summary>
        /// Mô tả chính sách ưu đãi chủ đầu tư
        /// </summary>
        [MaxLength(512)]
        [ColumnSnackCase(nameof(Description))]
        public string Description { get; set; }

        [MaxLength(512)]
        [ColumnSnackCase(nameof(Icon), TypeName = "VARCHAR2")]
        public string Icon { get; set; }

        [MaxLength(1)]
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        public string Status { get; set; }

        /// <summary>
        /// Loại chính sách ưu đãi chủ đầu tư <br/>
        /// <see cref="RstProjectPolicyTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(PolicyType))]
        public int PolicyType { get; set; }
        /// <summary>
        /// Loại hình đặt cọc (1: ONLINE, 2:OFFLINE, 3:ALL)
        /// </summary>
        [ColumnSnackCase(nameof(Source))]
        public int Source { get; set; }
        /// <summary>
        /// Giá trị quy ra tiền
        /// </summary>
        [ColumnSnackCase(nameof(ConversionValue), TypeName = "NUMBER(18, 6)")]
        public decimal? ConversionValue { get; set; }

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
