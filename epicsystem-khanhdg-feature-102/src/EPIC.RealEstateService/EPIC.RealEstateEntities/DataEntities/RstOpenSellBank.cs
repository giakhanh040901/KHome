using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Entities;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities.DataEntities;
using Microsoft.EntityFrameworkCore;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Tài khoản ngân hàng nhận tiền của mở bán
    /// </summary>
    [Table("RST_OPEN_SELL_BANK", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(OpenSellId), IsUnique = false, Name = "IX_RST_OPEN_SELL_BANK")]
    public class RstOpenSellBank : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstOpenSellBank).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Id của mở bán
        /// </summary>
        [ColumnSnackCase(nameof(OpenSellId))]
        public int OpenSellId { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng của đối tác
        /// </summary>
        [ColumnSnackCase(nameof(PartnerBankAccountId))]
        public int? PartnerBankAccountId { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng của đại lý
        /// </summary>
        [ColumnSnackCase(nameof(TradingBankAccountId))]
        public int? TradingBankAccountId { get; set; }

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
