using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using EPIC.Utils.DataUtils;
using System.ComponentModel.DataAnnotations;
using EPIC.Entities;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Chính sách bán hàng
    /// </summary>
    [Table("RST_SELLING_POLICY", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(Status), nameof(TradingProviderId), nameof(OpenSellId), IsUnique = false, Name = "IX_RST_SELLING_POLICY")]
    public class RstSellingPolicy : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstSellingPolicy).ToSnakeUpperCase()}";
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(SellingPolicyTempId))]
        public int SellingPolicyTempId { get; set; }

        [ColumnSnackCase(nameof(OpenSellId))]
        public int OpenSellId { get; set; }

        /// <summary>
        /// Trạng thái A, D
        /// </summary>
        [Required]
        [MaxLength(1)]
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        public string Status { get; set; }
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
