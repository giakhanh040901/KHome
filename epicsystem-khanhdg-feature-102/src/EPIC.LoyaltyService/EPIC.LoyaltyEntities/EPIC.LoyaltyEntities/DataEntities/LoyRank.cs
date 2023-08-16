using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.DataEntities
{
    [Table("LOY_RANK", Schema = DbSchemas.EPIC_LOYALTY)]
    public class LoyRank
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(LoyRank).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Tên hạng
        /// </summary>
        [ColumnSnackCase(nameof(Name))]
        [MaxLength(500)]
        public string Name { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnSnackCase(nameof(Description))]
        [MaxLength(4000)]
        public string Description { get; set; }

        /// <summary>
        /// Điểm bắt đầu
        /// </summary>
        [ColumnSnackCase(nameof(PointStart))]
        public int? PointStart { get; set; }

        /// <summary>
        /// Điểm kết thúc
        /// </summary>
        [ColumnSnackCase(nameof(PointEnd))]
        public int? PointEnd { get; set; }

        /// <summary>
        /// Trạng thái (1: Khởi tạo; 2: Kích hoạt; 3: Hủy kích hoạt)
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public int? Status { get; set; }

        /// <summary>
        /// Ngày kích hoạt
        /// </summary>
        [ColumnSnackCase(nameof(ActiveDate), TypeName = "DATE")]
        public DateTime? ActiveDate { get; set; }

        /// <summary>
        /// Ngày hủy kích hoạt
        /// </summary>
        [ColumnSnackCase(nameof(DeactiveDate), TypeName = "DATE")]
        public DateTime? DeactiveDate { get; set; }

        /// <summary>
        /// Đại lý sơ cấp
        /// </summary>
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }

        #region audit
        /// <summary>
        /// Ngày tạo
        /// </summary>
        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Ngày xóa
        /// </summary>
        [ColumnSnackCase(nameof(DeletedDate), TypeName = "DATE")]
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Người xóa
        /// </summary>
        [ColumnSnackCase(nameof(DeletedBy))]
        [MaxLength(100)]
        public string DeletedBy { get; set; }

        /// <summary>
        /// Xóa
        /// </summary>
        [ColumnSnackCase(nameof(Deleted), TypeName = "VARCHAR2")]
        [Required]
        [MaxLength(1)]
        public string Deleted { get; set; }
        #endregion
    }
}
