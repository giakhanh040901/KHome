using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
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
    [Table("RST_CONTRACT_TEMPLATE_TEMP", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(TradingProviderId), nameof(PartnerId), IsUnique = false, Name = "IX_RST_CONTRACT_TEMPLATE_TEMP")]

    public class RstContractTemplateTemp
    {
        public static string SEQ = $"SEQ_{nameof(RstContractTemplateTemp).ToSnakeUpperCase()}";
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(Name))]
        [MaxLength(256)]
        [Comment("Ten hop dong mau")]
        public string Name { get; set; }


        [ColumnSnackCase(nameof(TradingProviderId))]
        [Comment("Id dai ly")]
        public int? TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        [Comment("Id doi tac")]
        public int? PartnerId { get; set; }

        /// <summary>
        /// <see cref="RstContractTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(ContractType))]
        [Comment("Kieu hop dong (1: HD ban BDS, 2: HD mua BDS, 3 HD thanh ly, 4; HĐ dat coc)")]
        public int ContractType { get; set; }

        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Required]
        [Comment("Trang thai  (A : Kich hoat; D : Khong kich hoat)")]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(ContractSource))]
        [Comment("Kieu hop dong (1: ONLINE, 2: OFFLINE, 3:ALL)")]
        public int ContractSource { get; set; }

        [ColumnSnackCase(nameof(FileInvestor), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        [Required]
        [Comment("Duong dan file mau hop dong khach hang ca nhan")]
        public string FileInvestor { get; set; }

        [ColumnSnackCase(nameof(FileBusinessCustomer), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        [Required]
        [Comment("Duong dan file mau hop dong khach hang doanh nghiep")]
        public string FileBusinessCustomer { get; set; }

        [ColumnSnackCase(nameof(Description), TypeName = "NVARCHAR2")]
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
