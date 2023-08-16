using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentEntities.DataEntities
{
    [Table("EP_MSB_REQUEST_PAYMENT", Schema = DbSchemas.EPIC)]
    [Comment("Yeu cau lo chi MSB")]
    public class MsbRequestPayment : ICreatedBy
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(MsbRequestPayment)).ToSnakeUpperCase()}";

        #region Id
        /// <summary>
        /// Id lô
        /// </summary>
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Là requestId sang bên bank")]
        public long Id { get; set; }

        [Required]
        [ColumnSnackCase(nameof(TradingProdiverId))]
        [Comment("Id đại lý")]
        public int TradingProdiverId { get; set; }
        #endregion

        [Required]
        [ColumnSnackCase(nameof(ProductType))]
        [Comment("Loại sản phẩm (1: Garner, 2: Invest, 3: Company shares,...)")]
        public int ProductType { get; set; }

        [Required]
        [ColumnSnackCase(nameof(RequestType))]
        [Comment("Lại yêu cầu (1. Chi trả lợi tức, 2. Rút vốn)")]
        public int RequestType { get; set; }

        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        [ColumnSnackCase(nameof(CreatedBy))]
        public string CreatedBy { get; set; }
    }
}
