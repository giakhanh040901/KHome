using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.CoreEntities.DataEntities
{
    [Table("EP_XPT_TOKEN_DATA_TYPE", Schema = DbSchemas.EPIC)]
    public class XptTokenDataType
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(XptTokenDataType).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TokenId))]
        public int TokenId { get; set; }

        /// <summary>
        /// Loại dữ liệu
        /// </summary>
        [ColumnSnackCase(nameof(DataType))]
        public string DataType { get; set; }
    }
}
