//using EPIC.Utils;
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
    [Table("EP_INVESTOR_BANK_ACCOUNT_TEMP", Schema = DbSchemas.EPIC)]
    public class InvestorBankAccountTemp
    {
        public static string SEQ { get; } = $"SEQ_INVESTOR_BANK_ACCOUNT_TEMP";
        [Key]
		[Column("ID")]
        public int Id { get; set; }

        [Column("INVESTOR_ID")]
        public int InvestorId { get; set; }

        [Column("INVESTOR_GROUP_ID")]
        public int InvestorGroupId { get; set; }

        [Column("BANK_ID")]
        public int BankId { get; set; }

        [Column("BANK_ACCOUNT")]
        public string BankAccount { get; set; }

        [Column("BANK_CODE")]
        public string BankCode { get; set; }

        [Column("BANK_NAME")]
        public string BankName { get; set; }

        [Column("BANK_BRANCH")]
        public string BankBranch { get; set; }
       
        [Column("IS_DEFAULT")]
        public string IsDefault { get; set; }

        [Column("CREATED_DATE")]
        public DateTime CreatedDate { get; set; }
        
        [Column("CREATED_BY")]
        public string CreatedBy { get; set; }

        [Column("DELETED")]
        public string Deleted { get; set; }

        [Column("OWNER_ACCOUNT")]
        public string OwnerAccount { get; set; }

        [Column("REFER_ID")]
        public int? ReferId { get; set; }
    }
}
