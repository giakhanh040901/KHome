using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities.DataEntities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.LoyaltyEntities.DataEntities
{
    /// <summary>
    /// Chi tiết tham gia chương trình
    /// </summary>
    [Table("LOY_LUCKY_PROGRAM_INVESTOR_DETAIL", Schema = DbSchemas.EPIC_LOYALTY)]
    [Index(nameof(Deleted), nameof(TradingProviderId), nameof(LuckyProgramInvestorId), nameof(LuckyScenarioId), IsUnique = false, Name = "IX_LOY_LUCKY_PROGRAM_INVESTOR_DETAIL")]
    public class LoyLuckyProgramInvestorDetail
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(LoyLuckyProgramInvestorDetail).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        /// <summary>
        /// Id đại lý
        /// </summary>
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        /// <summary>
        /// Id chương trình
        /// </summary>
        [ColumnSnackCase(nameof(LuckyProgramInvestorId))]
        public int LuckyProgramInvestorId { get; set; }
        public LoyLuckyProgramInvestor LuckyProgramInvestor { get;set; }

        /// <summary>
        /// Id kịch bản trúng thưởng
        /// </summary>
        [ColumnSnackCase(nameof(LuckyScenarioId))]
        public int LuckyScenarioId { get; set; }

        /// <summary>
        /// Id chúng thưởng/ Chi tiết trong kịch bản trúng thưởng
        /// </summary>
        [ColumnSnackCase(nameof(LuckyScenarioDetailId))]
        public int LuckyScenarioDetailId { get; set; }

        /// <summary>
        /// Tên voucher
        /// </summary>
        [ColumnSnackCase(nameof(VoucherId))]
        public int? VoucherId { get; set; }

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
