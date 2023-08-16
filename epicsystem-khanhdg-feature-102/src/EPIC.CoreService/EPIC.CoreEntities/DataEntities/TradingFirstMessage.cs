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
    [Table("EP_CORE_TRADING_FIRST_MSG", Schema = DbSchemas.EPIC)]
    [Comment("Tin nhắn tự động đầu tiên khi start chat theo đại lý")]
    public class TradingFirstMessage
    {
        public static string SEQ { get; } = $"SEQ_CORE_TRADING_FIRST_MSG";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ColumnSnackCase(nameof(Id))]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(Message), TypeName = "CLOB")]
        [Comment("Tin nhắn")]
        public string Message { get; set; }

        [ColumnSnackCase(nameof(Deleted), TypeName = "VARCHAR2")]
        [DefaultValue(YesNo.NO)]
        [MaxLength(1)]
        public string Deleted { get; set; }

    }
}
