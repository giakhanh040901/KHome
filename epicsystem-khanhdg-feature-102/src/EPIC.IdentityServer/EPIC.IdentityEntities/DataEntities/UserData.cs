using EPIC.Entities.DataEntities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.IdentityEntities.DataEntities
{
    [Table("P_USER_DATA", Schema = DbSchemas.EPIC)]
    [Index(nameof(UserType), nameof(PartnerId), nameof(TradingProviderId), IsUnique = false, Name = "IX_P_USER_DATA")]
    public class UserData
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(UserData).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(UserId))]
        public int UserId { get; set; }
        public Users User { get; set; }

        /// <summary>
        /// Loại user <br/>
        /// <see cref="UserTypes"/>
        /// </summary>
        [Required]
        [MaxLength(1)]
        [ColumnSnackCase(nameof(UserType), TypeName = "VARCHAR2")]
        public string UserType { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int? PartnerId { get; set; }
        public Partner Partner { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }
        public TradingProvider TradingProvider { get; set; }
    }
}
