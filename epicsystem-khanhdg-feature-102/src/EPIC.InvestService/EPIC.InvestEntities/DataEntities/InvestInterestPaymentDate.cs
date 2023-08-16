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
    [Table("EP_INV_INTEREST_PAYMENT_DATE", Schema = DbSchemas.EPIC)]
    public class InvestInterestPaymentDate
    {
        public static string SEQ = $"SEQ_INV_INTEREST_PAYMENT_DATE";
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [ColumnSnackCase(nameof(OrderId))]
        public int OrderId { get; set; }
        [ColumnSnackCase(nameof(PeriodIndex))]
        public int PeriodIndex { get; set; }
        [ColumnSnackCase(nameof(PayDate))]
        public DateTime PayDate { get; set; }
    }
}
