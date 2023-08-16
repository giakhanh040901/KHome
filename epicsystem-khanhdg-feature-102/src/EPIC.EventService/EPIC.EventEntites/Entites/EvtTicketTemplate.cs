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
    [Table("EVT_TICKET_TEMPLATE", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(Deleted), nameof(EventId), nameof(Status), IsUnique = false, Name = "IX_EVT_TICKET_TEMPLATE")]
    public class EvtTicketTemplate : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(EvtTicketTemplate).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(EventId))]
        public int EventId { get; set; }
        public EvtEvent Event { get; set; }

        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }

        [ColumnSnackCase(nameof(FileUrl))]
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
