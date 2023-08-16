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

namespace EPIC.InvestEntities.DataEntities
{
    [Table("EP_INV_CONTRACT_TEMPLATE", Schema = DbSchemas.EPIC)]
    [Comment("Mau hop dong")]
    public class InvestContractTemplate : IFullAudited
    {
        public static string SEQ = $"SEQ_{nameof(InvestContractTemplate).ToSnakeUpperCase()}";
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Obsolete("bỏ")]
        [NotMapped]
        public string Code { get; set; }

        [Obsolete("bỏ")]
        [NotMapped]
        public string Name { get; set; }

        [Obsolete("bỏ")]
        [NotMapped]
        public string ContractTempUrl { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        [Comment("Id dai ly")]
        public int TradingProviderId { get; set; }

        [Obsolete("bỏ")]
        [NotMapped]
        public int DistributionId { get; set; }

        [Obsolete("bỏ")]
        [NotMapped]
        public string Type { get; set; }

        [ColumnSnackCase(nameof(DisplayType))]
        [MaxLength(1)]
        [Comment("Loai hien thi cua hop dong mau (B : Hien thi truoc khi hop dong duoc duyet; A : Hien thi sau khi hop dong duoc duyet)")]
        public string DisplayType { get; set; }

        #region audit

        [ColumnSnackCase(nameof(Deleted), TypeName = "VARCHAR2")]
        [Required]
        [MaxLength(1)]
        public string Deleted { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(ModifiedBy))]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [ColumnSnackCase(nameof(ModifiedDate), TypeName = "DATE")]
        public DateTime? ModifiedDate { get; set; }
        #endregion

        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Trang thai  (A : Kich hoat; D : Khong kich hoat)")]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(PolicyId))]
        [Comment("Id chinh sach")]
        public int PolicyId { get; set; }

        [ColumnSnackCase(nameof(ContractSource))]
        [Comment("Kieu hop dong (1: ONLINE, 2: OFFLINE)")]
        public int ContractSource { get; set; }

        [ColumnSnackCase(nameof(ContractTemplateTempId))]
        [Comment("Id hop dong mau")]
        public int? ContractTemplateTempId { get; set; }

        [ColumnSnackCase(nameof(ConfigContractId))]
        [Comment("Id cau truc ma")]
        public int ConfigContractId { get; set; }

        [ColumnSnackCase(nameof(StartDate), TypeName = "DATE")]
        [Comment("Ngay hieu luc")]
        public DateTime? StartDate { get; set; }

        [ColumnSnackCase(nameof(FileUploadName), TypeName = "NVARCHAR2")]
        [MaxLength(256)]
        public string FileUploadName { get; set; }

        [ColumnSnackCase(nameof(FileUploadInvestorUrl), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        public string FileUploadInvestorUrl { get; set; }

        [ColumnSnackCase(nameof(FileUploadBusinessCustomerUrl), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        public string FileUploadBusinessCustomerUrl { get; set; }

        [ColumnSnackCase(nameof(ContractType))]
        [Comment("Loai hop dong (1: ho so dat lenh, 2: ho so rut tien, 3: ho so tai tuc goc, 4: ho so rut tien app, 5: ho so tai tuc goc + loi nhuan)")]
        public int? ContractType { get; set; }
    }
}
    