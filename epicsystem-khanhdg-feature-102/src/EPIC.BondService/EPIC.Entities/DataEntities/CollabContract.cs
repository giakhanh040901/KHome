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
    [Table("EP_CORE_SALE_COLLAB_CONTRACT", Schema = DbSchemas.EPIC)]
    public class CollabContract : IFullAudited
    {
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(SaleId))]
        public int SaleId { get; set; }

        [ColumnSnackCase(nameof(CollabContractTempId))]
        public int CollabContractTempId { get; set; }

        [ColumnSnackCase(nameof(FileTempUrl))]
        public string FileTempUrl { get; set; }

        [ColumnSnackCase(nameof(FileSignatureUrl))]
        public string FileSignatureUrl { get; set; }

        [ColumnSnackCase(nameof(FileScanUrl))]
        public string FileScanUrl { get; set; }

        [ColumnSnackCase(nameof(IsSign))]
        public string IsSign { get; set; }

        [ColumnSnackCase(nameof(PageSign))]
        public int PageSign { get; set; }

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
