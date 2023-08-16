using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Utils;
using EPIC.Utils.Attributes;
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

namespace EPIC.LoyaltyEntities.DataEntities
{
    /// <summary>
    /// Khách hàng tham gia chương trình
    /// </summary>
    [Table("LOY_LUCKY_PROGRAM_INVESTOR", Schema = DbSchemas.EPIC_LOYALTY)]
    [Index(nameof(Deleted), nameof(TradingProviderId), nameof(LuckyProgramId), nameof(InvestorId), IsUnique = false, Name = "IX_LOY_LUCKY_PROGRAM_INVESTOR")]
    public class LoyLuckyProgramInvestor : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(LoyLuckyProgramInvestor).ToSnakeUpperCase()}";

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
        [ColumnSnackCase(nameof(LuckyProgramId))]
        public int LuckyProgramId { get; set; }
        public LoyLuckyProgram LuckyProgram { get; set; }

        /// <summary>
        /// Id khách hàng
        /// </summary>
        [ColumnSnackCase(nameof(InvestorId))]
        public int InvestorId { get; set; }

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
