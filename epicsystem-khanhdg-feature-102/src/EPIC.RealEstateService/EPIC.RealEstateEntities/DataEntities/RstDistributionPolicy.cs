using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
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
    /// <summary>
    /// Cài chính sách phân phối - đối tác cài
    /// </summary>
    [Table("RST_DISTRIBUTION_POLICY", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(PartnerId), nameof(Status), IsUnique = false, Name = "IX_RST_DISTRIBUTION_POLICY")]
    public class RstDistributionPolicy : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstDistributionPolicy).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }

        [ColumnSnackCase(nameof(DistributionId))]
        public int DistributionId { get; set; }

        [Required]
        [MaxLength(256)]
        [ColumnSnackCase(nameof(Code))]
        public string Code { get; set; }

        [Required]
        [MaxLength(256)]
        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }

        /// <summary>
        /// Loại thanh toán 1: Thanh toán thường, 2: Trả góp ngân hàng, 3: Trả trước<br/>
        /// <see cref="RstProjectDistributionPolicyPaymentTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(PaymentType))]
        public int PaymentType { get; set; }

        /// <summary>
        /// Loại hình đặt cọc 1: Theo giá trị căn hộ, 2: Giá cố định<br/>
        /// <see cref="RstDistributionPolicyTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(DepositType))]
        public int DepositType { get; set; }

        /// <summary>
        /// Giá trị đặt cọc (%/VNĐ)
        /// </summary>
        [ColumnSnackCase(nameof(DepositValue))]
        public decimal DepositValue { get; set; }

        /// <summary>
        /// Loại hình lock căn hộ 1: Theo giá trị căn hộ, 2: Giá cố định<br/>
        /// <see cref="RstDistributionPolicyTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(LockType))]
        public int LockType { get; set; }

        /// <summary>
        /// Giá trị lock căn hộ (%/VNĐ)
        /// </summary>
        [ColumnSnackCase(nameof(LockValue))]
        public decimal LockValue { get; set; }

        /// <summary>
        /// Trạng thái A / D
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public string Status { get; set; }

        /// <summary>
        /// Mô tả 
        /// </summary>
        [MaxLength(512)]
        [ColumnSnackCase(nameof(Description), TypeName = "VARCHAR2")]
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
