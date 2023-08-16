using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Media;
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
    /// Hình ảnh của vé
    /// </summary>
    [Table("EVT_TICKET_MEDIA", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(TicketId), IsUnique = false, Name = "IX_EVT_TICKET_MEDIA")]
    public class EvtTicketMedia : ICreatedBy
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(EvtTicketMedia).ToSnakeUpperCase()}";
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [ColumnSnackCase(nameof(TicketId))]
        public int TicketId { get; set; }
        public EvtTicket Ticket { get; set; }
        /// <summary>
        /// IMAGE: ảnh, VIDEO: video
        /// <see cref="MediaTypes"/>
        /// </summary>
        [MaxLength(256)]
        [ColumnSnackCase(nameof(MediaType), TypeName = "VARCHAR2")]
        public string MediaType { get; set; }
        /// <summary>
        /// Đường dẫn file ảnh
        /// </summary>
        //[Required]
        [MaxLength(512)]
        [ColumnSnackCase(nameof(UrlImage), TypeName = "VARCHAR2")]
        public string UrlImage { get; set; }

        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(50)]
        public string CreatedBy { get; set; }
    }
}
