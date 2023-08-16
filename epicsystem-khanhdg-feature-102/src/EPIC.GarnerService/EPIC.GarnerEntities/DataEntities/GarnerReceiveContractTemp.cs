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

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_RECEIVE_CONTRACT_TEMP", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("bang mau giao nhan hop dong ben garner")]
    public class GarnerReceiveContractTemp : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(GarnerReceiveContractTemp)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(Code))]
        public string Code { get; set; }

        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }

        [Required]
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(FileUrl))]
        public string FileUrl { get; set; }

        [Required]
        [ColumnSnackCase(nameof(DistributionId))]
        public int DistributionId { get; set; }

        [ColumnSnackCase(nameof(Status))]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(CreatedDate))]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        public string CreatedBy { get; set; }

        [ColumnSnackCase(nameof(ModifiedDate))]
        public DateTime? ModifiedDate { get; set; }

        [ColumnSnackCase(nameof(ModifiedBy))]
        public string ModifiedBy { get; set; }

        [ColumnSnackCase(nameof(Deleted))]
        public string Deleted { get; set; }
    }
}
