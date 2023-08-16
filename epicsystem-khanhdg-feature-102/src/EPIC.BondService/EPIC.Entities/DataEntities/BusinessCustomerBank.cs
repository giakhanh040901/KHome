using DocumentFormat.OpenXml.Office2010.Excel;
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
    [Table("EP_CORE_BUSINESS_CUSTOMER_BANK", Schema = DbSchemas.EPIC)]
    [Comment("Tai khoan ngan hang cua doanh nghiep")]
    public class BusinessCustomerBank : IFullAudited
    {
        public static string SEQ { get; } = "SEQ_BUSINESS_CUS_BANK";

        [Key]
        [ColumnSnackCase(nameof(BusinessCustomerBankAccId))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusinessCustomerBankAccId { get; set; }

        [ColumnSnackCase(nameof(BusinessCustomerId))]
        [Comment("Id Doanh ngiep")]
        public int BusinessCustomerId { get; set; }
        public BusinessCustomer BusinessCustomer { get; set; }

        [ColumnSnackCase(nameof(BankAccName), TypeName = "VARCHAR2")]
        [Comment("Dong khong cho dat lenh")]
        public string BankAccName { get; set; }

        [ColumnSnackCase(nameof(BankAccNo), TypeName = "VARCHAR2")]
        [Comment("So tai khoan")]
        public string BankAccNo { get; set; }


        [ColumnSnackCase(nameof(BankName), TypeName = "VARCHAR2")]
        public string BankName { get; set; }


        [ColumnSnackCase(nameof(BankBranchName), TypeName = "VARCHAR2")]
        public string BankBranchName { get; set; }

        [ColumnSnackCase(nameof(BankId))]
        [Comment("Id bank")]
        public int BankId { get; set; }
        public CoreBank CoreBank { get; set; }

        [ColumnSnackCase(nameof(Status))]
        [MaxLength(1)]
        [Comment("Trang thai 1: kich hoat, 2 khong kich hoat")]
        public int? Status { get; set; }

        [ColumnSnackCase(nameof(IsDefault), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Tai khoan mac dinh")]
        public string IsDefault { get; set; }

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
