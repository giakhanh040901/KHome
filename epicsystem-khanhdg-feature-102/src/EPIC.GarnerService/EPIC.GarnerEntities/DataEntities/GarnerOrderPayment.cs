using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Invest;

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_ORDER_PAYMENT", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Thanh toan")]
    public class GarnerOrderPayment : IFullAudited
    {
        public static string SEQ = $"SEQ_{nameof(GarnerOrderPayment).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(OrderId))]
        public long OrderId { get; set; }

        [ColumnSnackCase(nameof(TradingBankAccId))]
        public int? TradingBankAccId { get; set; }

        [ColumnSnackCase(nameof(TranDate), TypeName = "DATE")]
        [Comment("Ngay giao dich (tu nhap khi tao order)")]
        public DateTime? TranDate { get; set; }

        [ColumnSnackCase(nameof(TranType))]
        [Comment("Kieu giao dich (1: thu, 2: chi")]
        public int TranType { get; set; }

        [ColumnSnackCase(nameof(TranClassify))]
        [Comment("Loai giao dich (1: thanh toan hop dong, 2: Chi tra loi nhuan, 3: Rut von)")]
        public int TranClassify { get; set; }

        [ColumnSnackCase(nameof(PaymentType))]
        [Comment("Loai hinh thanh toan (1: tien mat, 2: chuyen khoan)")]
        public int PaymentType { get; set; }

        [ColumnSnackCase(nameof(PaymentAmount))]
        [Comment("So tien")]
        public decimal PaymentAmount { get; set; }

        [ColumnSnackCase(nameof(Description), TypeName = "VARCHAR2")]
        [MaxLength(512)]
        [Comment("Mo ta")]
        public string Description { get; set; }

        [ColumnSnackCase(nameof(Status))]
        [Comment("Trang thai thanh toan (1: nhap, 2: da thanh toan (phe duyet), 3: huy thanh toan (huy duyet))")]
        public int Status { get; set; }

        [ColumnSnackCase(nameof(ApproveBy), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        [Comment("Nguoi duyet")]
        public string ApproveBy { get; set; }

        [ColumnSnackCase(nameof(ApproveDate), TypeName = "DATE")]
        [Comment("Ngay duyet")]
        public DateTime? ApproveDate { get; set; }

        [ColumnSnackCase(nameof(CancelBy), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        [Comment("Nguoi huy")]
        public string CancelBy { get; set; }

        [ColumnSnackCase(nameof(CancelDate), TypeName = "DATE")]
        [Comment("Ngay huy")]
        public DateTime? CancelDate { get; set; }

        [ColumnSnackCase(nameof(OrderNo), TypeName = "VARCHAR2")]
        [MaxLength(100)]
        [Comment("So giao dich")]
        public string OrderNo { get; set; }

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
