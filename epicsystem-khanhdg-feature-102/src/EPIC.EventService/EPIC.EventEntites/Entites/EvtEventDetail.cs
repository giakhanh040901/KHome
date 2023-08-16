using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Utils.Attributes;
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

namespace EPIC.EventEntites.Entites
{
    /// <summary>
    /// Chi tiết Sự kiện (các khung giờ của sự kiện)
    /// </summary>
    [Table("EVT_EVENT_DETAIL", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(Deleted), nameof(EventId), nameof(Status), IsUnique = false, Name = "IX_EVT_EVENT_DETAIL")]
    public class EvtEventDetail : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(EvtEventDetail).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [ColumnSnackCase(nameof(EventId))]
        public int EventId { get; set; }
        public EvtEvent Event { get; set; }
        [ColumnSnackCase(nameof(StartDate))]
        public DateTime? StartDate { get; set; }
        [ColumnSnackCase(nameof(EndDate))]
        public DateTime? EndDate { get; set; }

        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

        /// <summary>
        /// Thời gian chờ thanh toán
        /// </summary>
        [ColumnSnackCase(nameof(PaymentWaittingTime))]
        public int? PaymentWaittingTime { get; set; }
        /// <summary>
        /// Hiển thị số vé còn lại trên app
        /// </summary>
        [ColumnSnackCase(nameof(IsShowRemaingTicketApp))]
        [Required]
        public bool IsShowRemaingTicketApp { get; set; }


        [ColumnSnackCase(nameof(UpcomingJobId))]
        public string UpcomingJobId { get; set; }
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

        public List<EvtTicket> Tickets { get;} = new();
        public List<EvtOrder> Orders { get;} = new();
    }
}
