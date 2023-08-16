using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.LoyaltyEntities.DataEntities
{
    [Table("LOY_CONVERSION_POINT", Schema = DbSchemas.EPIC_LOYALTY)]
    [Index(nameof(Deleted), nameof(TradingProviderId), nameof(InvestorId), nameof(Status), IsUnique = false, Name = "IX_LOY_CONVERSION_POINT")]
    public class LoyConversionPoint : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(LoyConversionPoint).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(InvestorId))]
        public int InvestorId { get; set; }

        /// <summary>
        /// Đại lý sơ cấp
        /// </summary>
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }

        /// <summary>
        /// Loại yêu cầu: 1: Đổi Voucher, 2: Đổi điểm
        /// <see cref="LoyRequestTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(RequestType))]
        public int RequestType { get; set; }

        /// <summary>
        /// Loại cấp phát: 1: Khách hàng đổi điểm (Mặc định khi tạo trên App), 2: Tặng khách hàng
        /// <see cref="LoyAllocationTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(AllocationType))]
        public int AllocationType { get; set; }

        /// <summary>
        /// Có trừ điểm không? Mặc định là có
        /// <see cref="YesNo"/>
        /// </summary>
        public string IsMinusPoint { get; set; }

        /// <summary>
        /// Ngày yêu cầu
        /// </summary>
        [ColumnSnackCase(nameof(RequestDate))]
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Điểm hiện tại của khách hàng tại thời điểm yêu cầu đổi điểm
        /// </summary>
        [ColumnSnackCase(nameof(CurrentPoint))]
        public int? CurrentPoint { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnSnackCase(nameof(Description), TypeName = "VARCHAR2")]
        [MaxLength(2000)]
        public string Description { get; set; }

        /// <summary>
        /// Trạng thái: 1: Khởi tạo, 2. Tiếp nhận Y/C, 3. Đang giao, 4.Hoàn thành, 5.Hủy yêu cầu
        /// <see cref="LoyConversionPointStatus"/>
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public int? Status { get; set; }

        /// <summary>
        /// Nguồn
        /// </summary>
        [ColumnSnackCase(nameof(Source))]
        public int? Source { get; set; }

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
