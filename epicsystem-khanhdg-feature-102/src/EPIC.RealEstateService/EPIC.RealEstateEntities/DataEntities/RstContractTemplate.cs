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

namespace EPIC.RealEstateEntities.DataEntities
{
    [Table("RST_CONTRACT_TEMPLATE", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(TradingProviderId), nameof(PartnerId), nameof(ContractTemplateTempId), nameof(ConfigContractId),nameof(PolicyId), IsUnique = false, Name = "IX_RST_CONFIG_CONTRACT_CODE")]
    public class RstContractTemplate : IFullAudited
    {
        public static string SEQ = $"SEQ_{nameof(RstContractTemplate).ToSnakeUpperCase()}";
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        [Comment("Id dai ly")]
        public int? TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        [Comment("Id doi tac")]
        public int? PartnerId { get; set; }

        [ColumnSnackCase(nameof(PolicyId))]
        [Comment("Id chinh sach")]
        public int PolicyId { get; set; }

        [ColumnSnackCase(nameof(DisplayType))]
        [MaxLength(1)]
        [Comment("Loai hien thi cua hop dong mau (B : Hien thi truoc khi hop dong duoc duyet; A : Hien thi sau khi hop dong duoc duyet)")]
        public string DisplayType { get; set; }

        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Trang thai  (A : Kich hoat; D : Khong kich hoat)")]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(ContractSource))]
        [Comment("Kieu hop dong (1: ONLINE, 2: OFFLINE, 3 ALL)")]
        public int ContractSource { get; set; }

        [ColumnSnackCase(nameof(ContractTemplateTempId))]
        [Comment("Id hop dong mau")]
        public int ContractTemplateTempId { get; set; }

        [ColumnSnackCase(nameof(ConfigContractId))]
        [Comment("Id cau truc ma")]
        public int ConfigContractId { get; set; }

        [ColumnSnackCase(nameof(StartDate), TypeName = "DATE")]
        [Comment("Ngay hieu luc")]
        public DateTime? StartDate { get; set; }

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
