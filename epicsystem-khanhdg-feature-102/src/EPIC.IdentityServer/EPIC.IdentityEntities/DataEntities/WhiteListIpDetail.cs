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
    [Table("EP_WHITE_LIST_IP_DETAIL", Schema = DbSchemas.EPIC)]
    public class WhiteListIpDetail
    {
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(WhiteListIpId))]
        public int WhiteListIpId { get; set; }

        [ColumnSnackCase(nameof(IpAddressStart))]
        public string IpAddressStart { get; set; }

        [ColumnSnackCase(nameof(IpAddressEnd))]
        public string IpAddressEnd { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }

        #region audit
        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(Deleted), TypeName = "VARCHAR2")]
        [Required]
        [MaxLength(1)]
        public string Deleted { get; set; }
        #endregion
    }
}
