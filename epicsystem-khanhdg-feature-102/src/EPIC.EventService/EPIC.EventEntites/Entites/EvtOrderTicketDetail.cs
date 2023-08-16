using EPIC.Entities;
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
    /// Chi tiết các vé trong sổ lệnh
    /// </summary>
    [Table("EVT_ORDER_TICKET_DETAIL", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(OrderDetailId), nameof(Status), nameof(TicketId), IsUnique = false, Name = "IX_EVT_ORDER_TICKET_DETAIL")]
    public class EvtOrderTicketDetail : IModifiedBy
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(EvtOrderTicketDetail).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        /// <summary>
        /// Id chi tiết lệnh
        /// </summary>
        [ColumnSnackCase(nameof(OrderDetailId))]
        public int OrderDetailId { get; set; }
        public EvtOrderDetail OrderDetail { get; set; }

        /// <summary>
        /// Id loại vé
        /// </summary>
        [ColumnSnackCase(nameof(TicketId))]
        public int TicketId { get; set; }
        public EvtTicket Ticket { get; set; }

        /// <summary>
        /// hinh thuc checkin
        /// </summary>
        [ColumnSnackCase(nameof(CheckInType))]
        public int? CheckInType { get; set; }

        /// <summary>
        /// hinh thuc checkout
        /// </summary>
        [ColumnSnackCase(nameof(CheckOutType))]
        public int? CheckOutType { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }
        /// <summary>
        /// Mã vé (để sinh QR)
        /// </summary>
        [ColumnSnackCase(nameof(TicketCode))]
        public string TicketCode { get; set; }

        [ColumnSnackCase(nameof(ModifiedDate), TypeName = "DATE")]
        public DateTime? ModifiedDate { get; set; }

        [ColumnSnackCase(nameof(ModifiedBy))]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }
        [ColumnSnackCase(nameof(CheckIn), TypeName = "DATE")]
        public DateTime? CheckIn { get; set; }
        [ColumnSnackCase(nameof(CheckOut), TypeName = "DATE")]
        public DateTime? CheckOut { get; set; }

        /// <summary>
        /// Vé đã fill thông tin
        /// </summary>
        [MaxLength(512)]
        [ColumnSnackCase(nameof(TicketFilledUrl))]
        public string TicketFilledUrl { get; set; }
    }
}
