using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.DataEntities
{
    [Table("EP_CORE_BUSINESS_CUS_PARTNER", Schema = DbSchemas.EPIC)]
    public class BusinessCustomerPartner
    {
        public static string SEQ { get; } = $"{DbSchemas.EPIC}.SEQ_BUSINESS_CUSTOMER_PARTNER";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(BusinessCustomerId))]
        public int? BusinessCustomerId { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int? PartnerId { get; set; }

        [ColumnSnackCase(nameof(CreateDate))]
        public DateTime? CreateDate { get; set; }

        [ColumnSnackCase(nameof(CreateBy))]
        public string CreateBy { get; set; }

        [ColumnSnackCase(nameof(Deleted))]
        public string Deleted { get; set; }
    }
}
