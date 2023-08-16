using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.Attributes;
using EPIC.Utils.DataUtils;
using System.ComponentModel.DataAnnotations;
using EPIC.Entities;
using EPIC.Entities.DataEntities;

namespace EPIC.EventEntites.Entites
{
    /// <summary>
    /// Chọn investor làm admin của sự kiện
    /// </summary>
    [Table("EVT_ADMIN_EVENT", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(EventId), nameof(InvestorId), IsUnique = false, Name = "IX_EVT_ADMIN_EVENT")]
    public class EvtAdminEvent : ICreatedBy
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(EvtAdminEvent).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [ColumnSnackCase(nameof(EventId))]
        public int EventId { get; set; }
        public EvtEvent Event { get; set; }
        [ColumnSnackCase(nameof(InvestorId))]
        public int InvestorId { get; set; }
        public Investor Investor { get; set; }
        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; }
        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(50)]
        public string CreatedBy { get; set; }
    }
}
