using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Utils;
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
    /// Chi tiết Sổ lệnh
    /// </summary>
    [Table("EVT_ORDER_DETAIL", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(OrderId), nameof(TicketId), IsUnique = false, Name = "IX_EVT_ORDER_DETAIL")]
    public class EvtOrderDetail : IModifiedBy
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(EvtOrderDetail).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        /// <summary>
        /// Id lệnh
        /// </summary>
        [ColumnSnackCase(nameof(OrderId))]
        public int OrderId { get; set; }
        public EvtOrder Order { get; set; }
        /// <summary>
        /// ID loại vé
        /// </summary>
        [ColumnSnackCase(nameof(TicketId))]
        public int TicketId { get; set; }
        public EvtTicket Ticket { get; set; }
        /// <summary>
        /// Số lượng vé
        /// </summary>
        [ColumnSnackCase(nameof(Quantity))]
        public int Quantity { get; set; }
        /// <summary>
        /// Đơn giá của vé
        /// </summary>
        [ColumnSnackCase(nameof(Price))]
        public decimal Price { get; set; }
        [ColumnSnackCase(nameof(ModifiedDate), TypeName = "DATE")]
        public DateTime? ModifiedDate { get; set; }

        [ColumnSnackCase(nameof(ModifiedBy))]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public List<EvtOrderTicketDetail> OrderTicketDetails { get; } = new();
    }
}
