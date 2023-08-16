using EPIC.Entities;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Shared;

namespace EPIC.InvestEntities.DataEntities
{
    [Table("EP_INV_ORDER_PAYMENT", Schema = DbSchemas.EPIC)]
    public class OrderPayment : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_INV_ORDER_PAYMENT";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(OrderId))]
        public long OrderId { get; set; }

        [ColumnSnackCase(nameof(TradingBankAccId))]
        public int TradingBankAccId { get; set; }

        [MaxLength(100)]
        [ColumnSnackCase(nameof(PaymentNo))]
        public string PaymentNo { get; set; }

        [ColumnSnackCase(nameof(TranDate), TypeName = "DATE")]
        public DateTime? TranDate { get; set; }

        [ColumnSnackCase(nameof(TranType))]
        public int? TranType { get; set; }

        [ColumnSnackCase(nameof(TranClassify))]
        public int? TranClassify { get; set; }

        [ColumnSnackCase(nameof(PaymentType))]
        public int? PaymentType { get; set; }

        [ColumnSnackCase(nameof(PaymentAmnount))]
        public decimal? PaymentAmnount { get; set; }

        [ColumnSnackCase(nameof(Description))]
        public string Description { get; set; }

        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

        [ColumnSnackCase(nameof(ApproveBy))]
        public string ApproveBy { get; set; }

        [ColumnSnackCase(nameof(ApproveDate), TypeName = "DATE")]
        public DateTime? ApproveDate { get; set; }

        #region audit
        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; }

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
        public string Deleted { get; set; }
        #endregion
    }
}
