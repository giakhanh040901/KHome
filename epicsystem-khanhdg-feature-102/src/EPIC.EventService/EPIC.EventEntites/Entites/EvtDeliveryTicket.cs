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
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;

namespace EPIC.EventEntites.Entites
{
    [Table("EVT_DELIVERY_TICKET_TEMPLATE", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(Deleted), nameof(EventId), nameof(Status), IsUnique = false, Name = "IX_EVT_DELIVERY_TICKET_TEMPLATE")]
    public class EvtDeliveryTicketTemplate : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(EvtDeliveryTicketTemplate).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(EventId))]
        public int EventId { get; set; }
        public EvtEvent Event { get; set; }

        [ColumnSnackCase(nameof(Name))]
        [MaxLength(256)]
        public string Name { get; set; }

        [ColumnSnackCase(nameof(FileUrl))]
        [MaxLength(1024)]
        public string FileUrl { get; set; }

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
    }
}
