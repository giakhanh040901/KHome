using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
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

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Đại lý - Mở bán
    /// </summary>
    [Table("RST_OPEN_SELL", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(TradingProviderId), nameof(ProjectId), nameof(DistributionId), IsUnique = false, Name = "IX_RST_OPEN_SELL")]
    public class RstOpenSell : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstOpenSell).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        /// <summary>
        /// Id Dự án
        /// </summary>
        [ColumnSnackCase(nameof(ProjectId))]
        public int ProjectId { get; set; }

        /// <summary>
        /// Id phân phối
        /// </summary>
        [ColumnSnackCase(nameof(DistributionId))]
        public int DistributionId { get; set; }

        /// <summary>
        /// Ngày mở bán
        /// </summary>
        [ColumnSnackCase(nameof(StartDate), TypeName = "DATE")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc mở bán
        /// </summary>
        [ColumnSnackCase(nameof(EndDate), TypeName = "DATE")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Thời gian giữ thanh toán cọc (giây)
        /// </summary>
        [ColumnSnackCase(nameof(KeepTime))]
        public int? KeepTime { get; set; }

        /// <summary>
        /// Mô tả về mở bán
        /// </summary>
        [ColumnSnackCase(nameof(Description), TypeName = "NVARCHAR2")]
        [MaxLength(1024)]
        public string Description { get; set; }
        /// <summary>
        /// Trạng thái của phân phối mở bán
        /// 1: Khởi tạo, 2:Chờ duyệt, 3: Đang bán, 4: Tạm dừng, 5: Hết hàng, 6 Hủy duyệt, 7 Dừng bán(Dừng hẳn không được mở lại)<br/>
        /// <see cref="RstDistributionStatus"/>
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

        /// <summary>
        /// Hotline
        /// </summary>
        [ColumnSnackCase(nameof(Hotline), TypeName = "VARCHAR2")]
        [MaxLength(20)]
        public string Hotline { get; set; }

        /// <summary>
        /// Có nổi bật hay không: Y/N
        /// </summary>
        [ColumnSnackCase(nameof(IsOutstanding), TypeName = "VARCHAR2")]
        [Required]
        [MaxLength(1)]
        public string IsOutstanding { get; set; }

        /// <summary>
        /// Bật tắt show App (Y/N)
        /// </summary>
        [ColumnSnackCase(nameof(IsShowApp), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        public string IsShowApp { get; set; }

        /// <summary>
        /// Tổng số lượt xem của mở bán
        /// </summary>
        [ColumnSnackCase(nameof(ViewCount))]
        public int ViewCount { get; set; }

        /// <summary>
        /// 1: Ngân hàng đại lý, 2: Ngân hàng đại lý đối tác, 3 tất cả
        /// </summary>
        [ColumnSnackCase(nameof(FromType))]
        public int FromType { get; set; }

        /// <summary>
        /// Chức năng đăng ký làm cộng tác viên bán hàng.
        /// Khi bật lên thì App sẽ hiện chức năng đăng ký làm CTV bán hàng
        /// Nếu bật mà nhà đầu tư đã là sale của đại lý thì cũng ko hiện trên App
        /// </summary>
        [ColumnSnackCase(nameof(IsRegisterSale))]
        public bool IsRegisterSale { get; set; }

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
