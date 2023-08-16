using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.CoreEntities.DataEntities
{
    [Table("EP_XPT_TOKEN", Schema = DbSchemas.EPIC)]
    public class XptToken
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(XptToken).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Token sinh ngẫu nhiên 20 ký tự
        /// </summary>
        [ColumnSnackCase(nameof(Token))]
        public string Token { get; set; }

        /// <summary>
        /// Thời gian hết hạn
        /// </summary>
        [ColumnSnackCase(nameof(ExpiredDate))]
        public DateTime? ExpiredDate { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [ColumnSnackCase(nameof(CreatedDate))]
        public DateTime CreatedDate { get; set; }
    }
}
