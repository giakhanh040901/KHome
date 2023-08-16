using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.DataEntities
{
    [Table("EP_TRADING_PROVIDER_CONFIG", Schema = DbSchemas.EPIC)]
    [Index(nameof(Deleted), nameof(TradingProviderId), IsUnique = false, Name = "IX_EP_TRADING_PROVIDER_CONFIG")]
    public class TradingProviderConfig : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(TradingProviderConfig).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(Key))]
        [MaxLength(128)]
        public string Key { get; set; }

        [ColumnSnackCase(nameof(Value))]
        [MaxLength(128)]
        public string Value { get; set; }

        /// <summary>
        /// Id background job hangfire
        /// </summary>
        [MaxLength(256)]
        [ColumnSnackCase(nameof(BackgroundJobId), TypeName = "VARCHAR2")]
        public string BackgroundJobId { get; set; }

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
        [DefaultValue(YesNo.NO)]
        [MaxLength(1)]
        public string Deleted { get; set; }
        #endregion
    }
}
