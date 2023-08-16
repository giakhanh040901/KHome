using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EPIC.LoyaltyEntities.DataEntities
{
    [Table("LOY_CONVERSION_POINT_DETAIL", Schema = DbSchemas.EPIC_LOYALTY)]
    [Index(nameof(Deleted), nameof(ConversionPointId), nameof(VoucherId), IsUnique = false, Name = "IX_LOY_CONVERSION_POINT_DETAIL")]
    public class LoyConversionPointDetail : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(LoyConversionPointDetail).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(ConversionPointId))]
        public int ConversionPointId { get; set; }

        /// <summary>
        /// Id Voucher
        /// </summary>
        [ColumnSnackCase(nameof(VoucherId))]
        public int VoucherId { get; set; }

        /// <summary>
        /// Điểm quy đổi theo Voucher
        /// </summary>
        [ColumnSnackCase(nameof(Point))]
        public int Point { get; set; }

        /// <summary>
        /// Số lượng Voucher yêu cầu
        /// </summary>
        [ColumnSnackCase(nameof(Quantity))]
        public int Quantity { get; set; }

        /// <summary>
        /// Tổng điểm đổi
        /// </summary>
        [ColumnSnackCase(nameof(TotalConversionPoint))]
        public int TotalConversionPoint { get; set; }

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
