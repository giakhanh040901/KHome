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
using EPIC.Entities;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Attributes;
using System.ComponentModel.DataAnnotations;

namespace EPIC.EventEntites.Entites
{
    /// <summary>
    /// Tài khoản nhận tiền của sự kiện
    /// </summary>
    [Table("EVT_EVENT_BANK_ACCOUNT", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(Deleted), nameof(Status), nameof(EventId), nameof(BusinessCustomerBankAccId), IsUnique = false, Name = "IX_EVT_EVENT_BANK_ACCOUNT")]
    public class EvtEventBankAccount : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(EvtEventBankAccount).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(EventId))]
        public int EventId { get; set; }
        public EvtEvent Event { get; }

        [ColumnSnackCase(nameof(BusinessCustomerBankAccId))]
        public int BusinessCustomerBankAccId { get; set; }

        [MaxLength(1)]
        [Required]
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        public string Status { get; set; }

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
