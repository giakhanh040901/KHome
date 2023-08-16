using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    [Table("EP_CORE_SALE", Schema = DbSchemas.EPIC)]
    [Comment("Sale")]
    public class Sale : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_CORE_SALE";

        [Key]
        [ColumnSnackCase(nameof(SaleId))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SaleId { get; set; }

        [ColumnSnackCase(nameof(InvestorId))]
        public int? InvestorId { get; set; }
        public Investor Investor { get; set; }  

        [ColumnSnackCase(nameof(BusinessCustomerId))]
        public int? BusinessCustomerId { get; set; }
        public BusinessCustomer BusinessCustomer { get; set; }

        [ColumnSnackCase(nameof(AutoDirectional))]
        public string AutoDirectional { get; set; }

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
