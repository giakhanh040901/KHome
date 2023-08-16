using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils;

namespace EPIC.EventEntites.Entites
{
    /// <summary>
    /// Thanh toán sổ lệnh
    /// </summary>
    [Table("EVT_ORDER_PAYMENT", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(Deleted), nameof(TradingProviderId), nameof(OrderId), nameof(TradingBankAccountId), nameof(TranClassify),
        nameof(PaymentType), nameof(Status), IsUnique = false, Name = "IX_EVT_ORDER_PAYMENT")]
    public class EvtOrderPayment : IFullAudited
    {
        public static string SEQ = $"SEQ_{nameof(EvtOrderPayment).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(OrderId))]
        public int OrderId { get; set; }
        public EvtOrder Order { get; set; }
        /// <summary>
        /// Tài khoản thụ hưởng đại lý
        /// </summary>
        [ColumnSnackCase(nameof(TradingBankAccountId))]
        public int TradingBankAccountId { get; set; }

        /// <summary>
        /// Số giao dịch
        /// </summary>
        [ColumnSnackCase(nameof(OrderNo), TypeName = "VARCHAR2")]
        [MaxLength(100)]
        public string OrderNo { get; set; }

        /// <summary>
        /// Ngày giao dịch nhập khi tạo
        /// </summary>
        [ColumnSnackCase(nameof(TranDate), TypeName = "DATE")]
        public DateTime? TranDate { get; set; }

        /// <summary>
        /// Loại giao dịch (1: Thanh toán mua vé)
        /// <see cref="TranClassifies"/>
        /// </summary>
        [ColumnSnackCase(nameof(TranClassify))]
        public int TranClassify { get; set; }

        /// <summary>
        /// Loại hình thanh toán (1: Tiền mặt, 2: Chuyển khoản)
        /// <see cref="PaymentTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(PaymentType))]
        public int PaymentType { get; set; }

        /// <summary>
        /// Số tiền
        /// </summary>
        [ColumnSnackCase(nameof(PaymentAmount), TypeName = "NUMBER(18, 6)")]
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnSnackCase(nameof(Description), TypeName = "VARCHAR2")]
        [MaxLength(512)]
        public string Description { get; set; }

        /// <summary>
        /// Trạng thái thanh toán (1: Khởi tạo, 2: Đã thanh toán (phe duyet), 3: Huỷ thanh toán (huy duyet))
        /// <see cref="OrderPaymentStatus"/>
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

        /// <summary>
        /// Người duyệt
        /// </summary>
        [ColumnSnackCase(nameof(ApproveBy), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string ApproveBy { get; set; }

        /// <summary>
        /// Ngày duyệt
        /// </summary>
        [ColumnSnackCase(nameof(ApproveDate), TypeName = "DATE")]
        public DateTime? ApproveDate { get; set; }

        /// <summary>
        /// Người huỷ
        /// </summary>
        [ColumnSnackCase(nameof(CancelBy), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string CancelBy { get; set; }

        /// <summary>
        /// Ngày huỷ
        /// </summary>
        [ColumnSnackCase(nameof(CancelDate), TypeName = "DATE")]
        public DateTime? CancelDate { get; set; }

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
