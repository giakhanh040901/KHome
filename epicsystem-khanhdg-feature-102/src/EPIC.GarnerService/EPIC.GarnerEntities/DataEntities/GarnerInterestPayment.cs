using EPIC.Entities;
using EPIC.EntitiesBase.Interfaces.Audit;
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

namespace EPIC.GarnerEntities.DataEntities
{
    /// <summary>
    /// Chi trả lợi tức định kỳ
    /// </summary>
    [Table("GAN_INTEREST_PAYMENT", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Chi tra loi tuc")]
    public class GarnerInterestPayment : IFullAudited, IApproveAudit, IApproveIpAudit
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(GarnerInterestPayment)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(PolicyId))]
        public int PolicyId { get; set; }

        [ColumnSnackCase(nameof(DistributionId))]
        public int DistributionId { get; set; }

        [ColumnSnackCase(nameof(CifCode))]
        public string CifCode { get; set; }

        [ColumnSnackCase(nameof(AmountMoney))]
        [Comment("So tien phai chi tra")]
        public decimal AmountMoney { get; set; }

        [ColumnSnackCase(nameof(PayDate), TypeName = "DATE")]
        [Comment("Ngay chi tra theo cong thuc tinh ra trong chinh sach cua lenh")]
        public DateTime PayDate { get; set; }

        [ColumnSnackCase(nameof(PeriodIndex))]
        [Comment("Kỳ trả lợi nhuận")]
        public decimal PeriodIndex { get; set; }

        [ColumnSnackCase(nameof(Status))]
        [Comment("Trang thai chi tra (1: da lap chua chi tra, 2: da chi tra(tự động), 3: Hủy, 4: da chi tra (Chi thủ công), 5 Chờ phản hồi)")]
        public int Status { get; set; }

        [Comment("Trạng thái Bank: 1 CHỜ PHẢN HỒI, 2: THÀNH CÔNG, 3: THẤT BẠI")]
        [ColumnSnackCase(nameof(StatusBank))]
        public int? StatusBank { get; set; }

        [ColumnSnackCase(nameof(ApproveBy))]
        [MaxLength(50)]
        [Comment("Nguoi duyet")]
        public string ApproveBy { get; set; }

        [ColumnSnackCase(nameof(ApproveDate), TypeName = "DATE")]
        [Comment("Ngay duyet")]
        public DateTime? ApproveDate { get; set; }

        /// <summary>
        /// Nội dung duyệt
        /// <see cref="ApproveNoteWithoutPayments"/>
        /// </summary>
        [ColumnSnackCase(nameof(ApproveNote))]
        public int? ApproveNote { get; set; }

        [ColumnSnackCase(nameof(ApproveIp), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string ApproveIp { get; set; }

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
