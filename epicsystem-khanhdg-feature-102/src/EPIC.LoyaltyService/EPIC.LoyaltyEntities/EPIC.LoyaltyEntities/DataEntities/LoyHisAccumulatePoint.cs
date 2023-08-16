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
    [Table("LOY_HIS_ACCUMULATE_POINT", Schema = DbSchemas.EPIC_LOYALTY)]
    //[Index(nameof(Deleted), nameof(PartnerId), nameof(Status), IsUnique = false, Name = "IX_RST_OWNER")]
    public class LoyHisAccumulatePoint : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(LoyHisAccumulatePoint).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(InvestorId))]
        public int InvestorId { get; set; }

        /// <summary>
        /// Số điểm
        /// </summary>
        [ColumnSnackCase(nameof(Point))]
        public int Point { get; set; }

        /// <summary>
        /// Tích điểm hay tiêu điểm
        /// 1: Tích điểm, 2: Tiêu điểm<br/>
        /// <see cref="LoyPointTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(PointType))]
        public int PointType { get; set; }

        /// <summary>
        /// Lý do
        /// <see cref="LoyHisAccumulatePointReasons"/>
        /// </summary>
        [ColumnSnackCase(nameof(Reason))]
        public int? Reason { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnSnackCase(nameof(Description), TypeName = "VARCHAR2")]
        [MaxLength(2000)]
        public string Description { get; set; }

        /// <summary>
        /// Id Người cài đặt
        /// </summary>
        [ColumnSnackCase(nameof(SettingUser))]
        public int? SettingUser { get; set; }

        /// <summary>
        /// Ngày áp dụng thực tế
        /// </summary>
        [ColumnSnackCase(nameof(ApplyDate), TypeName = "DATE")]
        public DateTime? ApplyDate { get; set; }

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
        /// Điểm hiện tại của khách hàng tại thời điểm yêu cầu tích điểm/tiêu điểm
        /// </summary>
        [ColumnSnackCase(nameof(CurrentPoint))]
        public int? CurrentPoint { get; set; }

        /// <summary>
        /// Ngày tiếp nhận
        /// </summary>
        [ColumnSnackCase(nameof(PendingDate), TypeName = "DATE")]
        public DateTime? PendingDate { get; set; }

        /// <summary>
        /// Ngày đang giao
        /// </summary>
        [ColumnSnackCase(nameof(DeliveryDate), TypeName = "DATE")]
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Ngày nhận
        /// </summary>
        [ColumnSnackCase(nameof(ReceivedDate), TypeName = "DATE")]
        public DateTime? ReceivedDate { get; set; }

        /// <summary>
        /// Ngày hoàn thành
        /// </summary>
        [ColumnSnackCase(nameof(FinishedDate), TypeName = "DATE")]
        public DateTime? FinishedDate { get; set; }

        /// <summary>
        /// Ngày hủy
        /// </summary>
        [ColumnSnackCase(nameof(CanceledDate), TypeName = "DATE")]
        public DateTime? CanceledDate { get; set; }

        /// <summary>
        /// Người tiếp nhận
        /// </summary>
        [ColumnSnackCase(nameof(PendingBy))]
        [MaxLength(50)]
        public string PendingBy { get; set; }

        /// <summary>
        /// Người giao
        /// </summary>
        [ColumnSnackCase(nameof(DeliveryBy))]
        [MaxLength(50)]
        public string DeliveryBy { get; set; }

        /// <summary>
        /// Người nhận
        /// </summary>
        [ColumnSnackCase(nameof(ReceivedBy))]
        [MaxLength(50)]
        public string ReceivedBy { get; set; }

        /// <summary>
        /// Người hoàn thành
        /// </summary>
        [ColumnSnackCase(nameof(FinishedBy))]
        [MaxLength(50)]
        public string FinishedBy { get; set; }

        /// <summary>
        /// Người hủy
        /// </summary>
        [ColumnSnackCase(nameof(CanceledBy))]
        [MaxLength(50)]
        public string CanceledBy { get; set; }

        /// <summary>
        /// Nguồn
        /// </summary>
        [ColumnSnackCase(nameof(Source))]
        public int? Source { get; set; }

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
