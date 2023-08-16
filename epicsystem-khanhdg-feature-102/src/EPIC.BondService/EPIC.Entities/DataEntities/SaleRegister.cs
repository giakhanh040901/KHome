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
    [Table("EP_CORE_SALE_REGISTER", Schema = DbSchemas.EPIC)]
    public class SaleRegister
    {
        public static string SEQ { get; } = $"SEQ_CORE_SALE_REGISTER";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [ColumnSnackCase(nameof(InvestorId))]
        public int? InvestorId { get; set; }
        [ColumnSnackCase(nameof(InvestorBankAccId))]
        public int? InvestorBankAccId { get; set; }
        [ColumnSnackCase(nameof(SaleManagerId))]
        public int? SaleManagerId { get; set; }
        [ColumnSnackCase(nameof(Status))]
        public int? Status { get; set; }
        [ColumnSnackCase(nameof(IpAddress))]
        public string IpAddress { get; set; }
        [ColumnSnackCase(nameof(CancelDate))]
        public DateTime? CancelDate { get; set; }
        [ColumnSnackCase(nameof(DirectionDate))]
        public DateTime? DirectionDate { get; set; }
        [ColumnSnackCase(nameof(CreatedDate))]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        [ColumnSnackCase(nameof(ModifiedDate))]
        public DateTime? ModifiedDate { get; set; }

        [ColumnSnackCase(nameof(ModifiedBy))]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [Required]
        [ColumnSnackCase(nameof(Deleted))]
        [MaxLength(1)]
        public string Deleted { get; set; }
    }
}
