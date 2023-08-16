using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace EPIC.LoyaltyEntities.DataEntities
{
    [Table("LOY_POINT_INVESTOR", Schema = DbSchemas.EPIC_LOYALTY)]
    //[Index(nameof(Deleted), nameof(PartnerId), nameof(Status), IsUnique = false, Name = "IX_RST_OWNER")]
    public class LoyPointInvestor
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(LoyPointInvestor).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(InvestorId))]
        public int InvestorId { get; set; }

        /// <summary>
        /// Điểm tích lũy
        /// </summary>
        [ColumnSnackCase(nameof(TotalPoint))]
        public int? TotalPoint { get; set; }

        /// <summary>
        /// Điểm hiện tại
        /// </summary>
        [ColumnSnackCase(nameof(CurrentPoint))]
        public int? CurrentPoint { get; set; }

        /// <summary>
        /// Số điểm dự kiến (Sau khi các lệnh tích điểm, tiêu điểm về trạng thái hoàn thành hết)
        /// </summary>
        [ColumnSnackCase(nameof(TempPoint))]
        public int? TempPoint { get; set; }

        /// <summary>
        /// Đại lý sơ cấp
        /// </summary>
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }

        #region audit
        /// <summary>
        /// Ngày tích hoặc tiêu điểm
        /// </summary>
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
