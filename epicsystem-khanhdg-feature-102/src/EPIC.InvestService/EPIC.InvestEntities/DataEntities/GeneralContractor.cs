using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Utils;
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
    [Table("EP_INV_GENERAL_CONTRACTOR", Schema = DbSchemas.EPIC)]
    public class GeneralContractor : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_INV_GENERAL_CONTRACTOR";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [ColumnSnackCase(nameof(BusinessCustomerId))]
        public int BusinessCustomerId { get; set; }
        public BusinessCustomer BusinessCustomer { get; set; }
        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }
        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }
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
        public List<Project> Projects { get; } = new();
    }
}
