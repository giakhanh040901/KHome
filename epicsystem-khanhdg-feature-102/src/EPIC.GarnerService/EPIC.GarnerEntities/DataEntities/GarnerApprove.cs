using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_APPROVE", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Module duyet, phan thanh 2 loai duyet update bang tam vao bang that va duyet update trang thai")]
    public class GarnerApprove : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(GarnerApprove)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(UserRequestId))]
        [Comment("Nguoi trinh duyet")]
        public int? UserRequestId { get; set; }

        [ColumnSnackCase(nameof(UserApproveId))]
        [Comment("Nguoi chi dinh duyet")]
        public int? UserApproveId { get; set; }

        [ColumnSnackCase(nameof(RequestDate), TypeName = "DATE")]
        [Comment("Ngay yeu cau")]
        public DateTime? RequestDate { get; set; }

        [ColumnSnackCase(nameof(ApproveDate), TypeName = "DATE")]
        [Comment("Ngay duyet")]
        public DateTime? ApproveDate { get; set; }

        [ColumnSnackCase(nameof(CancelDate), TypeName = "DATE")]
        [Comment("Ngay huy")]
        public DateTime? CancelDate { get; set; }

        [ColumnSnackCase(nameof(RequestNote))]
        [MaxLength(512)]
        [Comment("Ghi chu yeu cau")]
        public string RequestNote { get; set; }

        [ColumnSnackCase(nameof(ApproveNote))]
        [MaxLength(512)]
        [Comment("Ghi chu duyet")]
        public string ApproveNote { get; set; }

        [ColumnSnackCase(nameof(CancelNote))]
        [MaxLength(512)]
        [Comment("Ghi chu huy")]
        public string CancelNote { get; set; }

        [ColumnSnackCase(nameof(OpenNote))]
        [MaxLength(512)]
        [Comment("Ghi chu mo")]
        public string OpenNote { get; set; }

        [ColumnSnackCase(nameof(CloseNote))]
        [MaxLength(512)]
        [Comment("Ghi chu dong")]
        public string CloseNote { get; set; }

        [Required]
        [ColumnSnackCase(nameof(Status))]
        [Comment("Trang thai duyet (1: trinh, 2: duyet, 3: huy, 4 : dong), khi huy thi khong cho lam gi nua")]
        public int Status { get; set; }

        [Required]
        [ColumnSnackCase(nameof(DataType))]
        [Comment("Lam viec voi bang nao (1: GAN_GARNER_PRODUCT, 2: GAN_DISTRIBUTION, 3: GAN_ORDER(RENEWALS_REQUEST) (tai tuc), bang nao chua co thi comment vao day)")]
        public int DataType { get; set; }

        [ColumnSnackCase(nameof(DataStatus))]
        [Comment("Trang thai thay doi dang so")]
        public int DataStatus { get; set; }

        [ColumnSnackCase(nameof(DataStatusStr), TypeName = "VARCHAR2")]
        [Comment("Trang thai thay doi dang chu")]
        [MaxLength(1)]
        public string DataStatusStr { get; set; }

        [Required]
        [ColumnSnackCase(nameof(ActionType))]
        [Comment("Loai hanh dong (1: Them, 2: Cap nhat, 3: Xoa)")]
        public int ActionType { get; set; }

        [ColumnSnackCase(nameof(ReferId))]
        [Comment("Id tham chieu den bang that")]
        public long? ReferId { get; set; }

        [ColumnSnackCase(nameof(ReferIdTemp))]
        [Comment("Id tham chieu den bang tam")]
        public long? ReferIdTemp { get; set; }

        [ColumnSnackCase(nameof(IsCheck), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Epic da duyet (Y, N). Khi dlsc duyet thi epic se thay ban ghi nay. epic vao duyet se update epic check vao bang chinh")]
        public string IsCheck { get; set; }

        [ColumnSnackCase(nameof(UserCheckId))]
        [Comment("Id user epic duyet")]
        public int? UserCheckId { get; set; }

        [ColumnSnackCase(nameof(OpenDate), TypeName = "DATE")]
        public DateTime? OpenDate { get; set; }

        [ColumnSnackCase(nameof(CloseDate), TypeName = "DATE")]
        public DateTime? CloseDate { get; set; }

        [ColumnSnackCase(nameof(Summary))]
        [MaxLength(512)]
        [Comment("Thong tin duyet - backend tu sinh")]
        public string Summary { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        [Comment("Id dai ly")]
        public int? TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        [Comment("Id doi tac")]
        public int? PartnerId { get; set; }

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
