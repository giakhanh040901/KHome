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
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Phê duyệt
    /// </summary>
    [Table("RST_APPROVE", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(TradingProviderId), nameof(PartnerId), nameof(Status), IsUnique = false, Name = "IX_RST_APPROVE")]
    public class RstApprove : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(RstApprove)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Người trình duyệt
        /// </summary>
        [ColumnSnackCase(nameof(UserRequestId))]
        public int? UserRequestId { get; set; }

        /// <summary>
        /// Người chỉ định duyệt
        /// </summary>
        [ColumnSnackCase(nameof(UserApproveId))]
        public int? UserApproveId { get; set; }

        /// <summary>
        /// Ngày yêu cầu
        /// </summary>
        [ColumnSnackCase(nameof(RequestDate), TypeName = "DATE")]
        public DateTime? RequestDate { get; set; }

        /// <summary>
        /// Ngày duyệt
        /// </summary>
        [ColumnSnackCase(nameof(ApproveDate), TypeName = "DATE")]
        public DateTime? ApproveDate { get; set; }

        /// <summary>
        /// Ngày hủy
        /// </summary>
        [ColumnSnackCase(nameof(CancelDate), TypeName = "DATE")]
        public DateTime? CancelDate { get; set; }

        /// <summary>
        /// Ghi chú yêu cầu
        /// </summary>
        [ColumnSnackCase(nameof(RequestNote))]
        [MaxLength(512)]
        public string RequestNote { get; set; }

        /// <summary>
        /// Ghi chú duyệt
        /// </summary>
        [ColumnSnackCase(nameof(ApproveNote))]
        [MaxLength(512)]
        public string ApproveNote { get; set; }

        /// <summary>
        /// Ip duyệt
        /// </summary>
        [ColumnSnackCase(nameof(ApproveIp), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string ApproveIp { get; set; }

        /// <summary>
        /// Ghi chú Hủy
        /// </summary>
        [ColumnSnackCase(nameof(CancelNote))]
        [MaxLength(512)]
        public string CancelNote { get; set; }
        /// <summary>
        /// Trạng thái duyệt 1: Trình duyệt, 2 duyệt, 3 Hủy duyệt, 4 Đóng
        /// </summary>
        [Required]
        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

        /// <summary>
        /// Làm việc với các bảng nào (1: RST_PRODUCT, 2: RST_DISTRIBUTION, 3: RST_OPEN_SELL, 4: RST_PRODUCT_ITEM)
        /// <see cref="RstApproveDataTypes"/>
        /// </summary>
        [Required]
        [ColumnSnackCase(nameof(DataType))]
        public int DataType { get; set; }

        /// <summary>
        /// Trạng thái của dữ liệu thay đổi
        /// </summary>
        [ColumnSnackCase(nameof(DataStatus))]
        public int DataStatus { get; set; }

        /// <summary>
        /// Loại hành động: 1 THÊM, 2 CẬP NHẬT, 3 XÓA
        /// <see cref="ActionTypes"/>
        /// </summary>
        [Required]
        [ColumnSnackCase(nameof(ActionType))]
        public int ActionType { get; set; }

        /// <summary>
        /// Id tham chiếu đến bảng thật
        /// </summary>
        [ColumnSnackCase(nameof(ReferId))]
        public long? ReferId { get; set; }

        /// <summary>
        /// Id tham chiếu đến bảng tạm (Nếu có)
        /// </summary>
        [ColumnSnackCase(nameof(ReferIdTemp))]
        public long? ReferIdTemp { get; set; }

        [ColumnSnackCase(nameof(IsCheck), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Epic da duyet (Y, N). Khi dlsc duyet thi epic se thay ban ghi nay. epic vao duyet se update epic check vao bang chinh")]
        public string IsCheck { get; set; }

        [ColumnSnackCase(nameof(UserCheckId))]
        [Comment("Id user epic duyet")]
        public int? UserCheckId { get; set; }

        /// <summary>
        /// Thông tin duyệt - BE
        /// </summary>
        [ColumnSnackCase(nameof(Summary))]
        [MaxLength(512)]
        public string Summary { get; set; }

        /// <summary>
        /// Id đại lý
        /// </summary>
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }

        /// <summary>
        /// Id đối tác
        /// </summary>
        [ColumnSnackCase(nameof(PartnerId))]
        public int? PartnerId { get; set; }

        /// <summary>
        /// Lý do khoá căn (1: Khác)
        /// <see cref="RstLockingReasons"/>
        /// </summary>
        [ColumnSnackCase(nameof(LockingReason))]
        public int? LockingReason { get; set; }

        /// <summary>
        /// Nội dung khoá căn
        /// </summary>
        [ColumnSnackCase(nameof(LockingDescription))]
        [MaxLength(512)]
        public string LockingDescription { get; set; }

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
