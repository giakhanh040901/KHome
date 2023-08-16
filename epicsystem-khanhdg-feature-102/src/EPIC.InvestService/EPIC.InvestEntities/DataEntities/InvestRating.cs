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
    /// <summary>
    /// Đánh giá
    /// </summary>
    [Table("EP_INV_RATING", Schema = DbSchemas.EPIC)]
    [Index(nameof(Deleted), nameof(InvestorId), nameof(OrderId), IsUnique = false, Name = "IX_INV_RATING")]
    public class InvestRating : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(InvestRating).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(InvestorId))]
        public int InvestorId { get; set; }

        [ColumnSnackCase(nameof(OrderId))]
        public long OrderId { get; set; }

        [ColumnSnackCase(nameof(Rate))]
        public int Rate { get; set; }

        [ColumnSnackCase(nameof(ProductExperience))]
        public int ProductExperience { get; set; }

        [MaxLength(1024)]
        [ColumnSnackCase(nameof(Feedback))]
        public string Feedback { get; set; }

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
