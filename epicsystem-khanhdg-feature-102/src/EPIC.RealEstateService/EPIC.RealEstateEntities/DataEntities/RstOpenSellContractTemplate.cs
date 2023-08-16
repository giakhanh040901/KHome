using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.DataEntities
{
    [Table("RST_OPEN_SELL_CONTRACT_TEMPLATE", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(OpenSellId), nameof(ContractTemplateTempId), nameof(DistributionPolicyId),  nameof(TradingProviderId), IsUnique = false, Name = "IX_RST_OPEN_SELL_CONTRACT_TEMPLATE")]
    public class RstOpenSellContractTemplate : IFullAudited
    {
        public static string SEQ = $"SEQ_{nameof(RstOpenSellContractTemplate).ToSnakeUpperCase()}";
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Id của mở bán
        /// </summary>
        [ColumnSnackCase(nameof(OpenSellId))]
        public int OpenSellId { get; set; }

        /// <summary>
        /// Id đại lý
        /// </summary>
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        /// <summary>
        /// Mau hop dong
        /// </summary>
        [ColumnSnackCase(nameof(ContractTemplateTempId))]
        public int ContractTemplateTempId { get; set; }

        ///// <summary>
        ///// Chính sách mở bán 
        ///// </summary>
        //[Obsolete("Bỏ vì sử dụng chính sách của đối tác - distributionPolicy")]
        //[ColumnSnackCase(nameof(SellingPolicyId))]
        //public int SellingPolicyId { get; set; }

        /// <summary>
        /// Chính sách của đối tác
        /// </summary>
        [ColumnSnackCase(nameof(DistributionPolicyId))]
        public int DistributionPolicyId { get; set; }

        /// <summary>
        /// Cấu trúc mã 
        /// </summary>
        [ColumnSnackCase(nameof(ConfigContractCodeId))]
        public int ConfigContractCodeId { get; set; }

        /// <summary>
        /// Trạng thái Hủy kích hoạt, Kích hoạt (A/D)
        /// </summary>
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Required]
        public string Status { get; set; }

        /// <summary>
        /// Loai hien thi cua hop dong mau (B : Hien thi truoc khi hop dong duoc duyet; A : Hien thi sau khi hop dong duoc duyet)
        /// </summary>
        [ColumnSnackCase(nameof(DisplayType))]
        [MaxLength(1)]
        public string DisplayType { get; set; }

        /// <summary>
        /// Ngày hiệu lực
        /// </summary>
        [ColumnSnackCase(nameof(EffectiveDate), TypeName = "DATE")]
        public DateTime EffectiveDate { get; set; }

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
