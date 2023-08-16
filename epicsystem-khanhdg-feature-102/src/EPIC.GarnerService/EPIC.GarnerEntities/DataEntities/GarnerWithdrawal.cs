using EPIC.Entities;
using EPIC.EntitiesBase.Interfaces.Audit;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_WITHDRAWAL", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Rut tien")]
    public class GarnerWithdrawal : IFullAudited, IApproveAudit, ICancelAudit
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(GarnerWithdrawal)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [ColumnSnackCase(nameof(CifCode))]
        [MaxLength(50)]
        [Comment("Ma cif khach ca nhan, hoac doanh nghiep")]
        public string CifCode { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(DistributionId))]
        [Comment("Id ban theo ky han")]
        public int DistributionId { get; set; }

        [ColumnSnackCase(nameof(PolicyId))]
        [Comment("Id chinh sach")]
        public int PolicyId { get; set; }

        [ColumnSnackCase(nameof(BankAccountId))]
        [Comment("Id ngân hàng nhận tiền (theo ngân hàng của nhà đầu tư hay doanh nghiệp dựa vào cifcode)")]
        public int BankAccountId { get; set; }

        [ColumnSnackCase(nameof(AmountMoney))]
        [Comment("So tien rut")]
        public decimal AmountMoney { get; set; }

        [ColumnSnackCase(nameof(Source))]
        [Comment("Nguon tao yeu cau rut von (1: tren app, 2: tren cms)")]
        public int Source { get; set; }

        [ColumnSnackCase(nameof(WithdrawalDate))]
        [Comment("Ngay tinh rut tien (next work day)")]
        public DateTime WithdrawalDate { get; set; }

        [ColumnSnackCase(nameof(Status))]
        [Comment("1: yeu cau, 2: da chi tra, 3: huy yeu cau")]
        public int Status { get; set; }

        [ColumnSnackCase(nameof(StatusBank))]
        [Comment("Trạng thái từ Bank: 1: CHỜ PHẢN HỒI, 2 THÀNH CÔNG, 3 THẤT BẠI")]
        public int StatusBank { get; set; }

        [ColumnSnackCase(nameof(ApproveBy))]
        [MaxLength(50)]
        public string ApproveBy { get; set; }

        [ColumnSnackCase(nameof(ApproveDate), TypeName = "DATE")]
        public DateTime? ApproveDate { get; set; }

        /// <summary>
        /// Nội dung duyệt
        /// <see cref="ApproveNoteWithoutPayments"/>
        /// </summary>
        [ColumnSnackCase(nameof(ApproveNote))]
        public int? ApproveNote { get; set; }

        [ColumnSnackCase(nameof(CancelBy))]
        [MaxLength(50)]
        public string CancelBy { get; set; }

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
