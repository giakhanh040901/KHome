using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.LoyaltyEntities.DataEntities
{
    [Table("LOY_LUCKY_SCENARIO_DETAIL", Schema = DbSchemas.EPIC_LOYALTY)]
    [Index(nameof(Deleted), nameof(LuckyScenarioId), IsUnique = false, Name = "IX_LOY_LUCKY_SCENARIO_DETAIL")]
    public class LoyLuckyScenarioDetail : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(LoyLuckyScenarioDetail).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(LuckyScenarioId))]
        public int LuckyScenarioId { get; set; }
        public LoyLuckyScenario LoyLuckyScenario { get; set; }

        /// <summary>
        /// Thứ tự sắp xếp
        /// </summary>
        [ColumnSnackCase(nameof(SortOrder))]
        public int SortOrder { get; set; }

        /// <summary>
        /// Quà tặng/ Voucher
        /// </summary>
        [ColumnSnackCase(nameof(VoucherId))]
        public int? VoucherId { get; set; }

        /// <summary>
        /// Tên quà tặng nếu không chọn voucher
        /// </summary>
        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary>
        [ColumnSnackCase(nameof(Quantity))]
        public int? Quantity { get; set; }

        /// <summary>
        /// Xác suất may mắn
        /// </summary>
        [ColumnSnackCase(nameof(Probability))]
        public double? Probability { get; set; }

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
