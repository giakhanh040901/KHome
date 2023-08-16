using EPIC.Entities;
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

namespace EPIC.InvestEntities.DataEntities
{
    [Table("EP_INV_ORDER_CONTRACT_FILE", Schema = DbSchemas.EPIC)]
    public class OrderContractFile : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_INV_ORDER_CONTRACT_FILE";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }
        
        [ColumnSnackCase(nameof(OrderId))]
        public long? OrderId { get; set; }
        
        [ColumnSnackCase(nameof(ContractTempId))]
        public int? ContractTempId { get; set; }
        
        [ColumnSnackCase(nameof(FileTempUrl))]
        public string FileTempUrl { get; set; }

        [ColumnSnackCase(nameof(FileTempPdfUrl))]
        public string FileTempPdfUrl { get; set; }

        [ColumnSnackCase(nameof(FileSignatureUrl))]
        public string FileSignatureUrl { get; set; }

        [ColumnSnackCase(nameof(FileSignatureStampUrl), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        [Comment("Duong dan file scan")]
        public string FileSignatureStampUrl { get; set; }

        [ColumnSnackCase(nameof(FileScanUrl))]
        public string FileScanUrl { get; set; }

        [ColumnSnackCase(nameof(IsSign))]
        public string IsSign { get; set; }

        [ColumnSnackCase(nameof(PageSign))]
        public int PageSign { get; set; }

        [ColumnSnackCase(nameof(WithdrawalId))]
        public long? WithdrawalId { get; set; }

        [ColumnSnackCase(nameof(RenewalsId))]
        public long? RenewalsId { get; set; }

        [ColumnSnackCase(nameof(Times))]
        public int? Times { get; set; }

        [ColumnSnackCase(nameof(ContractCode))]
        public string ContractCode { get; set; }

        [ColumnSnackCase(nameof(ContractCodeGen))]
        public string ContractCodeGen { get; set; }

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
