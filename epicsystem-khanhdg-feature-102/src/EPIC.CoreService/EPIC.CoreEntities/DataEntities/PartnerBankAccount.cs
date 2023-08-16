using EPIC.Entities;
using EPIC.Entities.DataEntities;
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

namespace EPIC.CoreEntities.DataEntities
{
    /// <summary>
    /// Tài khoản ngân hàng của đối tác
    /// </summary>
    [Table("EP_CORE_PARTNER_BANK_ACCOUNT", Schema = DbSchemas.EPIC)]
    [Index(nameof(Deleted), nameof(PartnerId), nameof(IsDefault), nameof(Status), IsUnique = false, Name = "IX_PARTNER_BANK_ACCOUNT")]
    public class PartnerBankAccount : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(PartnerBankAccount).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Id đối tác
        /// </summary>
        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }

        /// <summary>
        /// Tên chủ tài khoản
        /// </summary>
        [ColumnSnackCase(nameof(BankAccName), TypeName = "VARCHAR2")]
        [MaxLength(256)]
        [Required]
        public string BankAccName { get; set; }

        /// <summary>
        /// Số tài khoản ngân hàng
        /// </summary>
        [ColumnSnackCase(nameof(BankAccNo), TypeName = "VARCHAR2")]
        [MaxLength(128)]
        [Required]
        public string BankAccNo { get; set; }

        /// <summary>
        /// Id ngân hàng
        /// </summary>
        [ColumnSnackCase(nameof(BankId))]
        public int BankId { get; set; }

        /// <summary>
        /// Trạng thái: 1: kích hoạt, 2: không kích hoạt
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        [MaxLength(1)]
        public int Status { get; set; }

        /// <summary>
        /// Có là tài khoản mặc định
        /// </summary>
        [ColumnSnackCase(nameof(IsDefault), TypeName = "VARCHAR2")]
        [Required]
        [MaxLength(1)]
        public string IsDefault { get; set; }

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
