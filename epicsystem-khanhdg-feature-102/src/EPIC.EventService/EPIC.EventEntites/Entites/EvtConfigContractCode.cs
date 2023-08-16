using EPIC.Entities;
using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.DataUtils;
using EPIC.Utils.ConstantVariables.Shared;
using static EPIC.Utils.ConstantVariables.Shared.ExcelReport;
using EPIC.Utils;
using EPIC.Entities.DataEntities;

namespace EPIC.EventEntites.Entites
{
    /// <summary>
    /// Cấu trúc mã
    /// </summary>
    [Table("EVT_CONFIG_CONTRACT_CODE", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(Deleted), nameof(TradingProviderId), nameof(Status), IsUnique = false, Name = "IX_EVT_CONFIG_CONTRACT_CODE")]
    public class EvtConfigContractCode : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(EvtConfigContractCode)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }
        public TradingProvider TradingProvider { get; set; }
        public List<EvtConfigContractCodeDetail> ConfigContractCodeDetails { get; set; }

        [Required]
        [MaxLength(128)]
        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }

        /// <summary>
        /// <see cref="Utils.Status"/>
        /// A:Active, D: Inactive
        /// </summary>
        [Required]
        [MaxLength(1)]
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        public string Status { get; set; }

        [MaxLength(512)]
        [ColumnSnackCase(nameof(Description))]
        [Comment("Mô tả")]
        public string Description { get; set; }

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

