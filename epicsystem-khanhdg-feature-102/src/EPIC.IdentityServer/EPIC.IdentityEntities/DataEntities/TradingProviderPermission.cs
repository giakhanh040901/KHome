using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Shared;

namespace EPIC.IdentityEntities.DataEntities
{
    [Table("P_TRADING_PROVIDER_PERMISSION", Schema = DbSchemas.EPIC)]
    public class TradingProviderPermission
    {
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(PermissionKey))]
        public string PermissionKey { get; set; }

        [ColumnSnackCase(nameof(PermissionType))]
        public int PermissionType { get; set; }

        [ColumnSnackCase(nameof(PermissionInWeb))]
        public int PermissionInWeb { get; set; }


        #region audit
        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(50)]
        public string CreatedBy { get; set; }
        #endregion
    }
}
