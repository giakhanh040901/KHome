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

namespace EPIC.Entities.DataEntities
{
    [Table("EP_CORE_BANK", Schema = DbSchemas.EPIC)]
    public class CoreBank : EntityTypeMapper
    {
        [Key]
        [ColumnSnackCase(nameof(BankId))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BankId { get; set; }

        [ColumnSnackCase(nameof(Logo))]
        public string Logo { get; set; }

        [ColumnSnackCase(nameof(BankName))]
        public string BankName { get; set; }

        [ColumnSnackCase(nameof(FullBankName))]
        public string FullBankName { get; set; }

        [ColumnSnackCase(nameof(BankCode))]
        public string BankCode { get; set; }

        [ColumnSnackCase(nameof(Bin))]
        public string Bin { get; set; }
        public List<BusinessCustomerBank> BusinessCustomerBanks { get; set; } = new();
    }
}
