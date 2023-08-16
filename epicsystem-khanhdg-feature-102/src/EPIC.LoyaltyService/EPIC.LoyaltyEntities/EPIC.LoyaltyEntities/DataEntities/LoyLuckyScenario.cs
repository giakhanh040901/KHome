using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.LoyaltyEntities.DataEntities
{
    /// <summary>
    /// Kịch bản vòng quay may mắn
    /// </summary>
    [Table("LOY_LUCKY_SCENARIO", Schema = DbSchemas.EPIC_LOYALTY)]
    [Index(nameof(Deleted), nameof(LuckyProgramId), IsUnique = false, Name = "IX_LOY_LUCKY_SCENARIO")]
    public class LoyLuckyScenario : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(LoyLuckyScenario).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(LuckyProgramId))]
        public int LuckyProgramId { get; set; }
        public LoyLuckyProgram LoyLuckyProgram { get; set; }

        /// <summary>
        /// Loại kịch bản: 1 Vòng quay may mắn, 2: Tích điểm đổi quà, 3: Rơi quà may mắn
        /// <see cref="LoyLuckyScenarioTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(LuckyScenarioType))]
        public int LuckyScenarioType { get; set; }

        /// <summary>
        /// Tên kịch bản
        /// </summary>
        [Required]
        [ColumnSnackCase(nameof(Name))]
        [MaxLength(512)]
        public string Name { get; set; }

        /// <summary>
        /// Ảnh đại diện kịch bản
        /// </summary>
        [ColumnSnackCase(nameof(AvatarImageUrl))]
        public string AvatarImageUrl { get; set; }

        /// <summary>
        /// Trạng thái A/D
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public string Status { get; set; }

        /// <summary>
        /// Số lượng giải thưởng
        /// </summary>
        [ColumnSnackCase(nameof(PrizeQuantity))]
        public int? PrizeQuantity { get; set; }

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
        public List<LoyLuckyScenarioDetail> LoyLuckyScenarioDetails { get; } = new();
    }
}
