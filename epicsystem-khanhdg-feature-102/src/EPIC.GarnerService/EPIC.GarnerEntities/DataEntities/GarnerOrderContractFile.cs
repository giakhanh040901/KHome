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
    [Table("GAN_ORDER_CONTRACT_FILE", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Ho so khach hang dang ky (Ho so khach hang dang ky)")]
    public class GarnerOrderContractFile : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(GarnerOrderContractFile)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        [Comment("Id dai ly")]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(OrderId))]
        [Comment("Id So lenh")]
        public long OrderId { get; set; }
        public GarnerOrder Order { get; set; }

        [ColumnSnackCase(nameof(ContractTempId))]
        [Comment("Id Hop dong mau")]
        public int ContractTempId { get; set; }
        public GarnerContractTemplateTemp ContractTemp { get; set; }

        [ColumnSnackCase(nameof(WithdrawalId))]
        [Comment("Id rut doi voi truong hop rut tien")]
        public long? WithdrawalId { get; set; }
        public GarnerWithdrawal Withdrawal { get; set; }

        [ColumnSnackCase(nameof(FileTempUrl), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        [Comment("Duong dan file mau")]
        public string FileTempUrl { get; set; }

        [ColumnSnackCase(nameof(FileTempPdfUrl), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        [Comment("Duong dan file pdf mau")]
        public string FileTempPdfUrl { get; set; }

        [ColumnSnackCase(nameof(FileSignatureUrl), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        [Comment("Duong dan file da ky")]
        public string FileSignatureUrl { get; set; }

        [ColumnSnackCase(nameof(FileScanUrl), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        [Comment("Duong dan file scan")]   
        public string FileScanUrl { get; set; }

        [Required]
        [ColumnSnackCase(nameof(IsSign), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Da ky dien tu hay chua? (Y = Da ky, N = Chua ky)")]
        public string IsSign { get; set; }

        [ColumnSnackCase(nameof(PageSign))]
        [Comment("Trang ky dien tu")]
        public int PageSign { get; set; }

        /// <summary>
        /// Mã hợp đồng được gen
        /// </summary>
        [ColumnSnackCase(nameof(ContractCodeGen), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string ContractCodeGen { get; set; }

        [ColumnSnackCase(nameof(FileSignatureStampUrl), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        public string FileSignatureStampUrl { get; set; }

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
