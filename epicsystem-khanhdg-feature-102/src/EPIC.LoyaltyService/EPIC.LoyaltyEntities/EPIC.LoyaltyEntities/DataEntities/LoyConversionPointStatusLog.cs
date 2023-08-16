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
    [Table("LOY_CONVERSION_POINT_STATUS_LOG", Schema = DbSchemas.EPIC_LOYALTY)]
    [Index(nameof(Deleted), nameof(ConversionPointId), IsUnique = false, Name = "IX_LOY_CONVERSION_POINT_STATUS_LOG")]
    public class LoyConversionPointStatusLog
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(LoyConversionPointStatusLog).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(ConversionPointId))]
        public int ConversionPointId { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        [ColumnSnackCase(nameof(Note), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        public string Note { get; set; }

        /// <summary>
        /// Trạng thái 1: Khởi tạo, 2. Tiếp nhận Y/C, 3. Đang giao, 4.Hoàn thành, 5.Hủy yêu cầu
        /// <see cref="LoyConversionPointStatus"/>
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

        /// <summary>
        /// Nguồn
        /// </summary>
        [ColumnSnackCase(nameof(Source))]
        public int? Source { get; set; }

        #region audit
        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        [ColumnSnackCase(nameof(Deleted), TypeName = "VARCHAR2")]
        [Required]
        [MaxLength(1)]
        public string Deleted { get; set; }
        #endregion
    }
}
