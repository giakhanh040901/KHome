using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
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
    /// Phân phối cho đại lý
    /// </summary>
    [Table("RST_DISTRIBUTION", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(PartnerId), nameof(ProjectId), IsUnique = false, Name = "IX_DISTRIBUTION")]
    public class RstDistribution : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstDistribution).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Id Đối tác
        /// </summary>
        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }

        /// <summary>
        /// Id Dự án
        /// </summary>
        [ColumnSnackCase(nameof(ProjectId))]
        public int ProjectId { get; set; }
        public RstProject Project { get; set; }

        /// <summary>
        /// Id Đại lý phân phối
        /// </summary>
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        /// <summary>
        /// Loại phân phối 1: Phân phối không độc quyền, 2: Phân phối độc quyền<br/>
        /// <see cref="RstDistributionTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(DistributionType))]
        public int? DistributionType { get; set; }

        /// <summary>
        /// Ngày phân phối
        /// </summary>
        [ColumnSnackCase(nameof(StartDate), TypeName = "DATE")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc phân phối
        /// </summary>
        [ColumnSnackCase(nameof(EndDate), TypeName = "DATE")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Trạng thái phân phối: 1: Khởi tạo, 2:Chờ duyệt, 3: Đang phân phối, 4: Tạm dừng, 5: Hết hàng<br/>
        /// <see cref="RstDistributionStatus"/>
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

        /// <summary>
        /// Mô tả 
        /// </summary>
        [ColumnSnackCase(nameof(Description))]
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
