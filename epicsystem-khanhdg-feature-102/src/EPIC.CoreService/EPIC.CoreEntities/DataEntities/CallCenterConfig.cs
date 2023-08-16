using EPIC.Entities.DataEntities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.CoreEntities.DataEntities
{
    [Table("EP_CALL_CENTER_CONFIG", Schema = DbSchemas.EPIC)]
    [Index(nameof(TradingProviderId), nameof(UserId), IsUnique = false, Name = "IX_CALL_CENTER_CONFIG")]
    public class CallCenterConfig
    {
        public static string SEQ { get; } = $"SEQ_{nameof(CallCenterConfig).ToSnakeUpperCase()}";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ColumnSnackCase(nameof(Id))]
        public int Id { get; set; }

        /// <summary>
        /// Id đại lý nếu không có id đại lý thì là root
        /// </summary>
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }
        public TradingProvider TradingProvider { get; set; }

        [ColumnSnackCase(nameof(UserId))]
        public int UserId { get; set; }

        [ColumnSnackCase(nameof(SortOrder))]
        public int SortOrder { get; set; }
    }
}
