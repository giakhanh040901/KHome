using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    [Table("EP_CORE_COLLAB_CONTRACT_TEMP", Schema = DbSchemas.EPIC)]
    public class CollabContractTemp : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_CORE_COLLAB_CONTRACT_TEMP";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }
        [ColumnSnackCase(nameof(Title))]
        public string Title { get; set; }
        [ColumnSnackCase(nameof(FileUrl))]
        public string FileUrl { get; set; }
        [ColumnSnackCase(nameof(Status))]
        public string Status { get; set; }
        [ColumnSnackCase(nameof(Type))]
        public string Type { get; set; }
        [ColumnSnackCase(nameof(CreatedBy))]
        public string CreatedBy { get; set; }
        [ColumnSnackCase(nameof(CreatedDate))]
        public DateTime? CreatedDate { get; set; }
        [ColumnSnackCase(nameof(ModifiedBy))]
        public string ModifiedBy { get; set; }
        [ColumnSnackCase(nameof(ModifiedDate))]
        public DateTime? ModifiedDate { get; set; }
        [ColumnSnackCase(nameof(Deleted))]
        public string Deleted { get; set; }
    }
}
