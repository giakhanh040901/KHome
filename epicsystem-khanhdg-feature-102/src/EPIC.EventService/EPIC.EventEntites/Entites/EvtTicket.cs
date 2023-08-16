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
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using Microsoft.EntityFrameworkCore;

namespace EPIC.EventEntites.Entites
{
    /// <summary>
    /// Vé
    /// </summary>
    [Table("EVT_TICKET", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(Deleted), nameof(EventDetailId), nameof(Status), IsUnique = false, Name = "IX_EVT_TICKET")]
    public class EvtTicket : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(EvtTicket).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        /// <summary>
        /// Id khung giờ
        /// </summary>
        [ColumnSnackCase(nameof(EventDetailId))]
        public int EventDetailId { get; set; }
        public EvtEventDetail EventDetail { get; set; }
        /// <summary>
        /// Tên loại
        /// </summary>
        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }
        /// <summary>
        /// Free vé hay không
        /// </summary>
        [ColumnSnackCase(nameof(IsFree))]
        public bool IsFree { get; set; }
        /// <summary>
        /// Giá vé
        /// </summary>
        [ColumnSnackCase(nameof(Price))]
        public decimal Price { get; set; }
        /// <summary>
        /// Số lượng vé
        /// </summary>
        [ColumnSnackCase(nameof(Quantity))]
        public int Quantity { get; set; }
        /// <summary>
        /// Số lượng mua tối thiểu trong 1 lần
        /// </summary>
        [ColumnSnackCase(nameof(MinBuy))]
        public int MinBuy { get; set; }
        /// <summary>
        /// Số lần mua tối đa trong 1 lần
        /// </summary>
        [ColumnSnackCase(nameof(MaxBuy))]
        public int MaxBuy { get; set; }
        /// <summary>
        /// Ngày bán
        /// </summary>
        [ColumnSnackCase(nameof(StartSellDate))]
        public DateTime StartSellDate { get; set; }
        /// <summary>
        /// Ngày kết thúc bán
        /// </summary>
        [ColumnSnackCase(nameof(EndSellDate))]
        public DateTime EndSellDate { get; set; }
                /// <summary>
        /// Show app
        /// </summary>
        [ColumnSnackCase(nameof(IsShowApp), TypeName = "VARCHAR2")]
        [Required]
        [MaxLength(1)]
        public string IsShowApp { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnSnackCase(nameof(Description))]
        public string Description { get; set; }
        [ColumnSnackCase(nameof(ContentType))]
        public string ContentType { get; set; }
        [ColumnSnackCase(nameof(OverviewContent))]
        public string OverviewContent { get; set; }
        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

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

        public List<EvtOrderDetail> OrderDetails { get; } = new();
        public List<EvtOrderTicketDetail> OrderTicketDetails { get; } = new();
        public List<EvtTicketMedia> TicketMedias { get; } = new();

    }
}
