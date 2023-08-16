using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.DataUtils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;

namespace EPIC.EventEntites.Entites
{
    /// <summary>
    /// Chi tiết cấu trúc mã
    /// </summary>
    [Table("EVT_CONFIG_CONTRACT_CODE_DETAIL", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(ConfigContractCodeId), IsUnique = false, Name = "IX_EVT_CONFIG_CONTRACT_CODE_DETAIL")]
    public class EvtConfigContractCodeDetail
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(EvtConfigContractCodeDetail)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(ConfigContractCodeId))]
        public int ConfigContractCodeId { get; set; }
        public EvtConfigContractCode ConfigContractCode { get; set; }

        [ColumnSnackCase(nameof(SortOrder))]
        public int SortOrder { get; set; }

        [Required]
        [MaxLength(50)]
        [ColumnSnackCase(nameof(Key), TypeName = "VARCHAR2")]
        public string Key { get; set; }

        [MaxLength(20)]
        [ColumnSnackCase(nameof(Value), TypeName = "VARCHAR2")]
        public string Value { get; set; }
    }
}
