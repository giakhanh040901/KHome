using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.DataEntities
{
    [Table("EP_TRADING_MSB_PREFIX_ACCOUNT", Schema = DbSchemas.EPIC)]
    public class TradingMsbPrefixAccount : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(TradingMsbPrefixAccount)).ToSnakeUpperCase()}";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ColumnSnackCase(nameof(Id))]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(TradingBankAccountId))]
        public int TradingBankAccountId { get; set; }

        [ColumnSnackCase(nameof(PrefixMsb), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string PrefixMsb { get; set; }

        [ColumnSnackCase(nameof(MId))]
        [MaxLength(50)]
        public string MId { get; set; }

        [ColumnSnackCase(nameof(TId))]
        [MaxLength(50)]
        public string TId { get; set; }

        [ColumnSnackCase(nameof(AccessCode))]
        [MaxLength(50)]
        public string AccessCode { get; set; }

        [ColumnSnackCase(nameof(TIdWithoutOtp))]
        [MaxLength(50)]
        public string TIdWithoutOtp { get; set; }

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
