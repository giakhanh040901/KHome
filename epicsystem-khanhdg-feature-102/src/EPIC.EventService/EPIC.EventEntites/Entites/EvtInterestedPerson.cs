using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Entities.DataEntities;

namespace EPIC.EventEntites.Entites
{
    /// <summary>
    /// Người quan tâm sau khi xem chi tiết event
    /// </summary>
    [Table("EVT_INTERESTED_PERSON", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(EventId), IsUnique = false, Name = "IX_EVT_INTERESTED_PERSON")]
    public class EvtInterestedPerson
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(EvtInterestedPerson).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        /// <summary>
        /// Id sự kiện
        /// </summary>
        [ColumnSnackCase(nameof(EventId))]
        public int EventId { get; set; }
        public EvtEvent Event { get; set; }
        /// <summary>
        /// ID người quan tâm
        /// </summary>
        [ColumnSnackCase(nameof(InvestorId))]
        public int InvestorId { get; set; }
        public Investor Investor { get; set; }
    }
}
