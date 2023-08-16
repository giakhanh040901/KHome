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
    [Table("LOY_ACCUMULATE_POINT_STATUS_LOG", Schema = DbSchemas.EPIC_LOYALTY)]
    //[Index(nameof(Deleted), nameof(PartnerId), nameof(Status), IsUnique = false, Name = "IX_RST_OWNER")]
    public class LoyAccumulatePointStatusLog
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(LoyAccumulatePointStatusLog).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(HisAccumulatePointId))]
        public int HisAccumulatePointId { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        [ColumnSnackCase(nameof(Note), TypeName = "VARCHAR2")]
        [MaxLength(1000)]
        public string Note { get; set; }

        /// <summary>
        /// Trạng thái lệnh tích điểm/tiêu điểm (4: Hoàn thành; 5: Hủy; 6: Khởi tạo)
        /// <see cref="LoyHisAccumulatePointStatus"/>
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public int? Status { get; set; }

        /// <summary>
        /// Trạng thái yêu cầu đổi điểm (1: Chờ duyệt; 2: Đang giao; 4: Hoàn thành; 5: Hủy; 6: Khởi tạo)
        /// <see cref="LoyExchangePointStatus"/>
        /// </summary>
        [ColumnSnackCase(nameof(ExchangedPointStatus))]
        public int? ExchangedPointStatus { get; set; }

        /// <summary>
        /// Nguồn
        /// </summary>
        [ColumnSnackCase(nameof(Source))]
        public int? Source { get; set; }

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
