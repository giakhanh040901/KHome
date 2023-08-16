using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.DataEntities
{
    [Table("EP_INV_BLOCKADE_LIBERATION", Schema = DbSchemas.EPIC)]
    public class BlockadeLiberation
    {
        public static string SEQ { get; } = $"SEQ_INV_BLOCKADE_LIBERATION"; 

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [ColumnSnackCase(nameof(Type))]
        public int? Type { get; set; }
        [ColumnSnackCase(nameof(BlockadeDescription))]
        public string BlockadeDescription { get; set; }
        [ColumnSnackCase(nameof(BlockadeDate))]
        public DateTime? BlockadeDate { get; set; }
        [ColumnSnackCase(nameof(OrderId))]
        public long OrderId { get; set; }
        public InvOrder Order { get; set; }
        [ColumnSnackCase(nameof(Blockader))]
        public string Blockader { get; set; }
        [ColumnSnackCase(nameof(BlockadeTime))]
        public DateTime? BlockadeTime { get; set; }
        [ColumnSnackCase(nameof(LiberationDescription))]
        public string LiberationDescription { get; set; }
        [ColumnSnackCase(nameof(LiberationDate))]
        public DateTime? LiberationDate { get; set; }
        [ColumnSnackCase(nameof(Liberator))]
        public string Liberator { get; set; }
        [ColumnSnackCase(nameof(LiberationTime))]
        public DateTime? LiberationTime { get; set; }
        [ColumnSnackCase(nameof(Status))]
        public int? Status { get; set; }
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }
        [ColumnSnackCase(nameof(CreatedBy))]
        public string CreatedBy { get; set; }
        [NotMapped]
        public string ContractCodeGen { get; set; }


    }
}
