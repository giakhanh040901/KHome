using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.RealEstate;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities.DataEntities;
using Microsoft.EntityFrameworkCore;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Chi tiết mở bán
    /// </summary>
    [Table("RST_OPEN_SELL_DETAIL", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(OpenSellId), nameof(DistributionProductItemId), IsUnique = false, Name = "IX_RST_OPEN_SELL_DETAIL")]
    public class RstOpenSellDetail : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstOpenSellDetail).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Id Phân phối mở bán
        /// </summary>
        [ColumnSnackCase(nameof(OpenSellId))]
        public int OpenSellId { get; set; }

        /// <summary>
        /// Id căn từ DistributionProductItemId lần lên ProductItem
        /// </summary>
        [ColumnSnackCase(nameof(DistributionProductItemId))]
        public int DistributionProductItemId { get; set; }

        /// <summary>
        /// Có hiện giá bán không? (Y/N) 
        /// Không hiện (N) thì có chọn liên hệ để xem thông tin giá
        /// </summary>
        [ColumnSnackCase(nameof(IsShowPrice), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        public string IsShowPrice { get; set; }

        /// <summary>
        /// Loại liên hệ khi không hiện giá: 1: Hotline, 2: Khác
        /// </summary>
        [ColumnSnackCase(nameof(ContactType))]
        public int? ContactType { get; set; }

        /// <summary>
        /// Số điện thoại liên hệ khi không hiện giá
        /// </summary>
        [ColumnSnackCase(nameof(ContactPhone), TypeName = "VARCHAR2")]
        [MaxLength(20)]
        public string ContactPhone { get; set; }

        /// <summary>
        /// Trạng thái (1: Khởi tạo (có thể là chưa mở bán hoặc đang mở bán) 2: Giữ chỗ, 3: Khóa căn, 4: Đã cọc, 5: Đã bán)  <br/>
        /// <see cref="RstProductItemStatus"/>
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

        /// <summary>
        /// Bật tắt show App (Y/N)
        /// </summary>
        [ColumnSnackCase(nameof(IsShowApp), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        public string IsShowApp { get; set; }

        /// <summary>
        /// Có khoá hay không(Khi căn hộ đang ở trạng thái khởi tạo), Khóa thì không cho bán nữa
        /// </summary>
        [Required]
        [MaxLength(1)]
        [ColumnSnackCase(nameof(IsLock), TypeName = "VARCHAR2")]
        public string IsLock { get; set; }

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
