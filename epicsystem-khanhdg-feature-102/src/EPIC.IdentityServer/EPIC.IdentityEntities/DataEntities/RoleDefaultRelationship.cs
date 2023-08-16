using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;

namespace EPIC.IdentityEntities.DataEntities
{
    [Table("P_ROLE_DEFAULT_RELATIONSHIP", Schema = DbSchemas.EPIC)]
    public class RoleDefaultRelationship
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RoleDefaultRelationship).ToSnakeUpperCase()}";
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(RoleId))]
        public int RoleId { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int? PartnerId { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }

        /// <summary>
        /// Trạng thái A/D
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public string Status { get; set; }

        #region audit
        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

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
        public string Deleted { get; set; } = YesNo.NO;
        #endregion
    }
}
