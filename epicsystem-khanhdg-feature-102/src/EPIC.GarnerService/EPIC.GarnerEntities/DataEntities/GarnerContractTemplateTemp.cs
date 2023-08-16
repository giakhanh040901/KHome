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
    [Table("GAN_CONTRACT_TEMPLATE_TEMP", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Mau hop dong mau")]
    public class GarnerContractTemplateTemp : IFullAudited
    {
        public static string SEQ = $"SEQ_{nameof(GarnerContractTemplateTemp).ToSnakeUpperCase()}";
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Obsolete("bỏ")]
        [NotMapped]
        public string Code { get; set; }

        [ColumnSnackCase(nameof(Name))]
        [MaxLength(256)]
        [Comment("Ten hop dong mau")]
        public string Name { get; set; }

        [Obsolete("bỏ")]
        [NotMapped]
        public string ContractTempUrl { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        [Comment("Id dai ly")]
        public int TradingProviderId { get; set; }

        [Obsolete("bỏ")]
        [NotMapped]
        public int PolicyTempId { get; set; }

        [Obsolete("bỏ")]
        [NotMapped]
        public string Type { get; set; }

        [ColumnSnackCase(nameof(ContractType))]
        [Comment("Loai hop dong (1: ho so dat lenh, 2: ho so rut tien, 3: ho so tai tuc, 4: ho so rut tien app)")]
        public int ContractType { get; set; }

        [Obsolete("bỏ")]
        [NotMapped]
        public string DisplayType { get; set; }

        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Required]
        [Comment("Trang thai  (A : Kich hoat; D : Khong kich hoat)")]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(ContractSource))]
        [Comment("Kieu hop dong (1: ONLINE, 2: OFFLINE)")]
        public int ContractSource { get; set; }

        [ColumnSnackCase(nameof(FileInvestor), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        [Required]
        [Comment("Duong dan file mau hop dong khach hang ca nhan")]
        public string FileInvestor { get; set; }

        [ColumnSnackCase(nameof(FileBusinessCustomer), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        [Required]
        [Comment("Duong dan file mau hop dong khach hang ca nhan")]
        public string FileBusinessCustomer { get; set; }

        [ColumnSnackCase(nameof(Description), TypeName = "VARCHAR2")]
        [MaxLength(2000)]
        [Comment("Mô tả mau hop dong ")]
        public string Description { get; set; }
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
