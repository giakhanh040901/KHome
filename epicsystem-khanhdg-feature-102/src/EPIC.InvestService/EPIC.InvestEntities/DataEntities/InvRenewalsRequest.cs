using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.DataEntities
{
    [Table("EP_INV_RENEWALS_REQUEST", Schema = DbSchemas.EPIC)]
    public class InvRenewalsRequest
    {
        public static string SEQ { get; } = $"SEQ_INV_RENEWALS_REQUEST";
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [ColumnSnackCase(nameof(OrderId))]
        public long OrderId { get; set; }
        public InvOrder Order { get; set; }
        [ColumnSnackCase(nameof(SettlementMethod))]
        public int SettlementMethod { get; set; }
        [ColumnSnackCase(nameof(RenewalsPolicyDetailId))]
        public int RenewalsPolicyDetailId { get; set; }
        [ColumnSnackCase(nameof(Status))]
        public int Status {get; set; }

        [ColumnSnackCase(nameof(Deleted))]
        public string Deleted { get; set; }
    }
}
