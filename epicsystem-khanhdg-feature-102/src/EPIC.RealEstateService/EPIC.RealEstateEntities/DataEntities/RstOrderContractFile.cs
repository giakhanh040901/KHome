using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Utils;
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

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Hợp đồng sổ lệnh
    /// </summary>
    [Table("RST_ORDER_CONTRACT_FILE", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(TradingProviderId), nameof(OrderId), nameof(ContractTempId), IsUnique = false, Name = "RST_ORDER_CONTRACT_FILE")]
    public class RstOrderContractFile : IFullAudited
    {
        public static string SEQ = $"SEQ_{nameof(RstOrderContractFile).ToSnakeUpperCase()}";
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        /// <summary>
        /// Id đại lý
        /// </summary>
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }
        /// <summary>
        /// ID Order
        /// </summary>
        [ColumnSnackCase(nameof(OrderId))]
        public int OrderId { get; set; }
        /// <summary>
        /// ID mẫu hợp đồng
        /// </summary>
        [ColumnSnackCase(nameof(ContractTempId))]
        public int ContractTempId { get; set; }
        /// <summary>
        /// Đường dẫn file mẫu
        /// </summary>
        [ColumnSnackCase(nameof(FileTempUrl), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        public string FileTempUrl { get; set; }
        /// <summary>
        /// Đường dẫn file pdf mẫu
        /// </summary>
        [ColumnSnackCase(nameof(FileTempPdfUrl), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        public string FileTempPdfUrl { get; set; }
        /// <summary>
        /// Đường dẫn file đã ký
        /// </summary>
        [ColumnSnackCase(nameof(FileSignatureUrl), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        public string FileSignatureUrl { get; set; }
        /// <summary>
        /// Đường dẫn file scan
        /// </summary>
        [ColumnSnackCase(nameof(FileScanUrl), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        public string FileScanUrl { get; set; }
        /// <summary>
        /// Đã ký hay chưa (Y: YES, N: NO)
        /// </summary>
        [Required]
        [ColumnSnackCase(nameof(IsSign), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        public string IsSign { get; set; }
        /// <summary>
        /// Trang ký
        /// </summary>
        [ColumnSnackCase(nameof(PageSign))]
        public int? PageSign { get; set; }
        /// <summary>
        /// Mã hợp đồng được gen
        /// </summary>
        [ColumnSnackCase(nameof(ContractCodeGen), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string ContractCodeGen { get; set; }

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
