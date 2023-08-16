using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.InvestEntities.DataEntities
{
    [Table("EP_INV_CONFIG_CONTRACT_CODE_DETAIL", Schema = DbSchemas.EPIC)]
    public class InvestConfigContractCodeDetail
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(InvestConfigContractCodeDetail)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(ConfigContractCodeId))]
        public int ConfigContractCodeId { get; set; }

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
