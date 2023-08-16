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
    [Table("EP_CORE_SALE_TRADING_PROVIDER", Schema = DbSchemas.EPIC)]
    public class SaleTradingProvider : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_CORE_SALE_TRADING_PROVIDER";
            
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(SaleId))]
        public int SaleId { get; set; }

        [ColumnSnackCase(nameof(EmployeeCode))]
        public string EmployeeCode { get; set; }

        [ColumnSnackCase(nameof(SaleType))]
        public int SaleType { get; set; }

        [ColumnSnackCase(nameof(Status))]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(InvestorBankAccId))]
        public int? InvestorBankAccId { get; set; }

        [ColumnSnackCase(nameof(BusinessCustomerBankAccId))]
        public int? BusinessCustomerBankAccId { get; set; }

        [ColumnSnackCase(nameof(SaleParentId))]
        public int? SaleParentId { get; set; }

        [ColumnSnackCase(nameof(ContractCode))]
        public string ContractCode { get; set; }

        [ColumnSnackCase(nameof(DeactiveDate))]
        public DateTime? DeactiveDate { get; set; }

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
