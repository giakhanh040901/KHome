using EPIC.Entities;
using EPIC.Utils.Attributes;
using System.ComponentModel.DataAnnotations;
using System;
using EPIC.Utils.ConstantVariables.RealEstate;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Giỏ hàng
    /// </summary>
    [Table("RST_CART", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(OpenSellDetailId), nameof(InvestorId), nameof(Status), IsUnique = false, Name = "IX_RST_CART")]
    public class RstCart : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstCart).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Id chi tiết mở bán
        /// </summary>
        [ColumnSnackCase(nameof(OpenSellDetailId))]
        public int OpenSellDetailId { get; set; }

        [ColumnSnackCase(nameof(InvestorId))]
        public int InvestorId { get; set; }

        /// <summary>
        /// Trạng thái sản phẩm trong giỏ hàng<br/>
        /// <see cref="RstCartStatus"/>
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

        /// <summary>
        /// Ngày giao dịch
        /// </summary>
        [ColumnSnackCase(nameof(TransDate), TypeName = "DATE")]
        public DateTime? TransDate { get; set; }

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
